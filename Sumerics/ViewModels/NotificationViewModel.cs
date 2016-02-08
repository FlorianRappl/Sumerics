namespace Sumerics
{
    using System;
    using System.Windows.Media.Imaging;
    using YAMP;

    sealed class NotificationViewModel : BaseViewModel
    {
        readonly DateTime created;

        public NotificationViewModel(NotificationEventArgs e, IContainer container)
            : base(container)
        {
            created = DateTime.Now;
            Message = e.Message;
            Icon = Icons.GetMessageImage(e.Type);
        }

        public String Time 
        { 
            get { return String.Format("{0} {1}", created.ToShortDateString(), created.ToShortTimeString()); } 
        }

        public String Message 
        { 
            get;
            private set; 
        }

        public BitmapImage Icon 
        { 
            get;
            private set; 
        }
    }
}
