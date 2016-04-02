namespace Sumerics.Managers
{
    using Sumerics.Dialogs;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    sealed class DialogManager : IDialogManager
    {
        #region Fields

        readonly Dictionary<Dialog, IDialogHandler> _handlers;

        #endregion

        #region ctor

        public DialogManager(IApplication container)
        {
            _handlers = new Dictionary<Dialog, IDialogHandler>();

            Assembly.GetExecutingAssembly().DefinedTypes.ForEach(type =>
            {
                var attribute = type.GetCustomAttribute<DialogTypeAttribute>();

                if (attribute != null && type.ImplementedInterfaces.Contains(typeof(IDialogHandler)))
                {
                    var handler = container.Get(type) as IDialogHandler;

                    if (handler != null)
                    {
                        _handlers.Add(attribute.Type, handler);
                    }
                }
            });
        }

        #endregion

        #region Methods

        public void Open(Dialog value, params Object[] parameters)
        {
            var handler = default(IDialogHandler);

            if (_handlers.TryGetValue(value, out handler))
            {
                App.Current.Dispatcher.Invoke(() => handler.Open(parameters));
            }
        }

        public void Close(Dialog value)
        {
            var handler = default(IDialogHandler);

            if (_handlers.TryGetValue(value, out handler))
            {
                App.Current.Dispatcher.Invoke(() => handler.Close());
            }
        }

        #endregion
    }
}
