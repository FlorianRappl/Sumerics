namespace Sumerics
{
    using Sumerics.ViewModels;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public sealed class Queries : IEnumerable<QueryResultViewModel>
    {
        readonly List<QueryResultViewModel> _running;

        public Queries()
        {
            _running = new List<QueryResultViewModel>();
        }

        #region Events

        public event EventHandler Changed;

        #endregion

        public Boolean IsEmpty
        {
            get { return _running.Count == 0; }
        }

        public void Add(QueryResultViewModel qrvm)
        {
            var changed = Changed;
            qrvm.Running = true;
            _running.Add(qrvm);

            if (_running.Count == 1 && changed != null)
            {
                changed.Invoke(qrvm, EventArgs.Empty);
            }
        }

        public void Remove(QueryResultViewModel qrvm)
        {
            var changed = Changed;
            qrvm.Running = false;
            _running.Remove(qrvm);

            if (_running.Count == 0 && changed != null)
            {
                changed.Invoke(qrvm, EventArgs.Empty);
            }
        }

        public IEnumerator<QueryResultViewModel> GetEnumerator()
        {
            return _running.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
