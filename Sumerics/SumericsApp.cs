using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumerics
{
    class SumericsApp : IApplication
    {
        readonly IConsole _console;
        readonly IVisualizer _visualizer;
        readonly IKernel _kernel;

        public SumericsApp(IConsole console, IVisualizer visualizer, IKernel kernel)
        {
            _console = console;
            _visualizer = visualizer;
            _kernel = kernel;
        }

        public void Shutdown()
        {
            App.Current.Shutdown();
        }

        public void ChangeTab(Int32 selectedIndex)
        {
            throw new NotImplementedException();
        }

        public IConsole Console
        {
            get { return _console; }
        }

        public IVisualizer Visualizer
        {
            get { return _visualizer; }
        }

        public IKernel Kernel
        {
            get { return _kernel; }
        }

        public void Open(Dialog value)
        {
            throw new NotImplementedException();
        }
    }
}
