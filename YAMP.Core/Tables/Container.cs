using System;
using YAMP.Core;

namespace YAMP.Tables
{
    class Container : INotifyOnChanged
    {
        #region Properties

        /// <summary>
        /// Gets or sets the notify method.
        /// </summary>
        public Notification Notify
        {
            get;
            set;
        }

        #endregion

        #region Helpers

        protected void RaiseChanged(string name, ChangeState state)
        {
            if (Notify != null)
                Notify(name, state);
        }

        #endregion
    }
}
