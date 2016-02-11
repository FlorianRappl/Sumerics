namespace Sumerics
{
    using System;
    using System.Threading.Tasks;

    public interface IKernel
    {
        Task RunAsync(String query);

        void StopAll();

        Task LoadWorkspaceAsync(String fileName);

        Task SaveWorkspaceAsync(String fileName);
    }
}
