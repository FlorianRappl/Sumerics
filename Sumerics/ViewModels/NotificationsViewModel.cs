namespace Sumerics
{
    using System;
    using System.Collections.ObjectModel;
    using System.Timers;
    using YAMP;

    sealed class NotificationsViewModel : BaseViewModel
    {
        readonly ObservableCollection<NotificationViewModel> _messages;
        readonly Timer _popupTimer;

        Boolean _available;

        public NotificationsViewModel()
        {
            _messages = new ObservableCollection<NotificationViewModel>();
            _popupTimer = new Timer();
            _popupTimer.Interval = 5000;
            _popupTimer.Elapsed += (s, e) =>
            {
                _popupTimer.Stop();
                App.Current.Dispatcher.Invoke(() => IsAvailable = false);
            };
        }

        public Boolean IsAvailable
        {
            get { return _available; }
            set
            {
                _available = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<NotificationViewModel> Messages
        {
            get { return _messages; }
        }

        public void Received(Object sender, NotificationEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                _messages.Insert(0, new NotificationViewModel(e));
                IsAvailable = true;
                _popupTimer.Stop();
                _popupTimer.Start();
            });
        }
    }
}
