namespace Sumerics
{
    using Sumerics.ViewModels;
    using Sumerics.Views;
    using System.Windows;

    sealed class WindowFactory : TypeFactory<BaseViewModel, Window>
    {
        private WindowFactory()
        {
            Register<MainViewModel>(ctx => new MainWindow(ctx));
            Register<AboutViewModel>(ctx => new AboutWindow { DataContext = ctx });
            Register<ConsoleEnterViewModel>(ctx => new ConsoleEnterWindow { DataContext = ctx });
            Register<EditorViewModel>(ctx => new EditorWindow { DataContext = ctx });
            Register<FolderBrowseViewModel>(ctx => new FolderBrowseWindow { DataContext = ctx });
            Register<OpenFileViewModel>(ctx => new OpenFileWindow { DataContext = ctx });
            Register<SaveFileViewModel>(ctx => new SaveFileWindow { DataContext = ctx });
            Register<SaveImageViewModel>(ctx => new SaveImageWindow { DataContext = ctx });
            Register<InputViewModel>(ctx => new InputDialog { DataContext = ctx });
            Register<OutputViewModel>(ctx => new OutputDialog { DataContext = ctx });
            Register<HelpViewModel>(ctx => new HelpWindow { DataContext = ctx });
            Register<ContourViewModel>(ctx => new ContourSeriesWindow { DataContext = ctx });
            Register<HeatmapViewModel>(ctx => new HeatSeriesWindow { DataContext = ctx });
            Register<SeriesViewModel>(ctx => new PlotSeriesWindow { DataContext = ctx });
            Register<PlotSettingsViewModel>(ctx => new PlotSettingsWindow { DataContext = ctx });
            Register<SubPlotSettingsViewModel>(ctx => new SubPlotSettingsWindow { DataContext = ctx });
            Register<OptionsViewModel>(ctx => new OptionsWindow { DataContext = ctx });
            Register<DocumentationViewModel>(ctx => new HelpWindow { DataContext = ctx });
        }

        public static WindowFactory Instance = new WindowFactory();

        protected override Window CreateDefault()
        {
            return null;
        }
    }
}
