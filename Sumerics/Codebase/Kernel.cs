namespace Sumerics
{
    using System;

    sealed class Kernel : IKernel
    {
        public void StopAll()
        {
            if (QueryResultViewModel.HasRunningQueries)
            {
                foreach (var query in QueryResultViewModel.RunningQueries)
                {
                    query.Cancel();
                }
            }
        }
    }
}
