/* ****************************************
   Publishing workflow
   -------------------

 - Update CHANGELOG.md
 - Run a normal build with Cake
 - Run a Publish build with Cake
   **************************************** */

#addin "Cake.FileHelpers"
#addin "Octokit"
#addin "Cake.Squirrel"
using Octokit;

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var isLocal = BuildSystem.IsLocalBuild;
var isRunningOnUnix = IsRunningOnUnix();
var isRunningOnWindows = IsRunningOnWindows();
var isRunningOnAppVeyor = AppVeyor.IsRunningOnAppVeyor;
var isPullRequest = AppVeyor.Environment.PullRequest.IsPullRequest;
var buildNumber = AppVeyor.Environment.Build.Number;
var releaseNotes = ParseReleaseNotes("./CHANGELOG.md");
var version = releaseNotes.Version.ToString();
var buildSourceDir = Directory("./source/Sumerics/bin") + Directory(configuration);
var buildResultDir = Directory("./build") + Directory(version);
var installerDir = Directory("./installer");
var nugetRoot = installerDir + Directory("nuget");
var chocolateyRoot = installerDir + Directory("chocolatey");
var releasesDir = installerDir + Directory("releases");

// Initialization
// ----------------------------------------

Setup(() =>
{
    Information("Building version {0} of Sumerics.", version);
    Information("For the publish target the following environment variables need to be set:");
    Information("  CHOCOLATEY_API_KEY, GITHUB_API_TOKEN");
});

// Tasks
// ----------------------------------------

Task("Clean")
    .Does(() =>
    {
        CleanDirectories(new DirectoryPath[] { buildResultDir });
    });

Task("Restore-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        NuGetRestore("./source/Sumerics.sln");
    });

Task("Update-Assembly-Version")
    .Does(() =>
    {
        var file = Directory("./source") + File("ProductInfo.cs");

        CreateAssemblyInfo(file, new AssemblyInfoSettings
        {
            Product = "Sumerics",
            Description = "Sensor Numerics for Intel Ultrabooks™ combines the power of numerical computations with the ability to perform experiments with the Intel Ultrabook™ over the integrated sensors.",
            Version = version,
            FileVersion = version,
            Company = "Florian Rappl",
            Trademark = "",
            Copyright = String.Format("Copyright © 2012 - {0}", DateTime.Now.Year)
        });
    });

Task("Build")
    .IsDependentOn("Restore-Packages")
    .IsDependentOn("Update-Assembly-Version")
    .Does(() =>
    {
        if (isRunningOnWindows)
        {
            MSBuild("./source/Sumerics.sln", new MSBuildSettings()
                .SetConfiguration(configuration)
                .SetVerbosity(Verbosity.Minimal)
            );
        }
        else
        {
            XBuild("./source/Sumerics.sln", new XBuildSettings()
                .SetConfiguration(configuration)
                .SetVerbosity(Verbosity.Minimal)
            );
        }
    });

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var settings = new NUnit3Settings
        {
            Work = buildResultDir.Path.FullPath
        };

        if (isRunningOnAppVeyor)
        {
            settings.Where = "cat != ExcludeFromAppVeyor";
        }

        NUnit3("./src/**/bin/" + configuration + "/*.Tests.dll", settings);
    });

Task("Copy-Files")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var target = nugetRoot + Directory("lib") + Directory("net45");
        CopyDirectory(buildSourceDir, target);
        DeleteFiles(GetFiles(target.Path.FullPath + "/*.pdb"));
        DeleteFiles(GetFiles(target.Path.FullPath + "/*.vshost.*"));
    });

Task("Create-Package")
    .IsDependentOn("Copy-Files")
    .Does(() =>
    {
        var nugetExe = GetFiles("./tools/**/nuget.exe").FirstOrDefault()
            ?? (isRunningOnAppVeyor ? GetFiles("C:\\Tools\\NuGet3\\nuget.exe").FirstOrDefault() : null);

        if (nugetExe == null)
        {            
            throw new InvalidOperationException("Could not find nuget.exe.");
        }

        var spec = nugetRoot + File("Sumerics.nuspec");

        NuGetPack(spec, new NuGetPackSettings
        {
            Version = version,
            BasePath = nugetRoot,
            OutputDirectory = nugetRoot,
            Symbols = false
        });
    });

Task("Create-Installer")
    .IsDependentOn("Create-Package")
    .Does(() => {
        var fileName = "Sumerics." + version + ".nupkg";
        var package = nugetRoot + File(fileName);
        
        Squirrel(package, new SquirrelSettings
        {
            Silent = true,
            NoMsi = true,
            ReleaseDirectory = releasesDir
        });
    });

Task("Create-Chocolatey")
    .IsDependentOn("Create-Package")
    .Does(() =>
    {
        var content = String.Format("$packageName = 'Sumerics'{1}$installerType = 'exe'{1}$url32 = 'https://github.com/FlorianRappl/Sumerics/releases/download/v{0}/Sumerics.exe'{1}$silentArgs = ''{1}{1}Install-ChocolateyPackage \"$packageName\" \"$installerType\" \"$silentArgs\" \"$url32\"", version, Environment.NewLine);
        var nuspec = chocolateyRoot + File("Sumerics.nuspec");
        var toolsDirectory = chocolateyRoot + Directory("tools");
        var scriptFile = toolsDirectory + File("chocolateyInstall.ps1");

        CreateDirectory(toolsDirectory);
        System.IO.File.WriteAllText(scriptFile.Path.FullPath, content);
        
        ChocolateyPack(nuspec, new ChocolateyPackSettings
        {
            Version = version,
            OutputDirectory = chocolateyRoot
        });
    });
    
Task("Publish-Release")
    .IsDependentOn("Create-Installer")
    .WithCriteria(() => isLocal)
    .Does(() =>
    {
        var githubToken = EnvironmentVariable("GITHUB_API_TOKEN");

        if (String.IsNullOrEmpty(githubToken))
        {
            throw new InvalidOperationException("Could not resolve Sumerics GitHub token.");
        }
        
        var github = new GitHubClient(new ProductHeaderValue("SumericsCakeBuild"))
        {
            Credentials = new Credentials(githubToken)
        };

        var release = github.Release.Create("FlorianRappl", "Sumerics", new NewRelease("v" + version) 
        {
            Name = version,
            Body = String.Join(Environment.NewLine, releaseNotes.Notes),
            Prerelease = false,
            TargetCommitish = "master"
        }).Result;

        var releaseFiles = GetFiles(releasesDir.Path.FullPath + "/*");

        foreach (var releaseFile in releaseFiles)
        {
            using (var contentStream = System.IO.File.OpenRead(releaseFile.FullPath))
            {
                var fileName = releaseFile.GetFilename().ToString();
                github.Release.UploadAsset(release, new ReleaseAssetUpload(fileName, "application/binary", contentStream, null)).Wait();
            }
        }
    });
    
Task("Publish-Package")
    .IsDependentOn("Create-Chocolatey")
    .IsDependentOn("Publish-Release")
    .WithCriteria(() => isLocal)
    .Does(() =>
    {
        var apiKey = EnvironmentVariable("CHOCOLATEY_API_KEY");
        var fileName = "Sumerics" + version + ".nupkg";
        var package = chocolateyRoot + File(fileName);

        if (String.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException("Could not resolve the Chocolatey API key.");
        }

        ChocolateyPush(package, new ChocolateyPushSettings
        { 
            Source = "https://chocolatey.org/packages",
            ApiKey = apiKey 
        });
    });
    
Task("Update-AppVeyor-Build-Number")
    .WithCriteria(() => isRunningOnAppVeyor)
    .Does(() =>
    {
        AppVeyor.UpdateBuildVersion(version);
    });
    
// Targets
// ----------------------------------------
    
Task("Package")
    .IsDependentOn("Run-Unit-Tests")
    .IsDependentOn("Create-Chocolatey")
    .IsDependentOn("Create-Installer");

Task("Default")
    .IsDependentOn("Package");

Task("Publish")
    .IsDependentOn("Publish-Package")
    .IsDependentOn("Publish-Release");
    
Task("AppVeyor")
    .IsDependentOn("Run-Unit-Tests")
    .IsDependentOn("Update-AppVeyor-Build-Number");

// Execution
// ----------------------------------------

RunTarget(target);