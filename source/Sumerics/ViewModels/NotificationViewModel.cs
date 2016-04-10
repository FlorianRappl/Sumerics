namespace Sumerics.ViewModels
{
    using System;
    using System.Windows.Media.Imaging;
    using YAMP;

    public sealed class NotificationViewModel : BaseViewModel
    {
        public NotificationViewModel(NotificationEventArgs e)
        {
            Time = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            Message = e.Message;
            Icon = IconFactory.GetMessageImage(e.Type);
        }

        public String Time 
        { 
            get;
            private set; 
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
