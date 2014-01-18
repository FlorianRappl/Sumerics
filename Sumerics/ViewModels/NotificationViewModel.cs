using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using YAMP;

namespace Sumerics
{
    class NotificationViewModel : BaseViewModel
    {
        DateTime created;

        public NotificationViewModel(NotificationEventArgs e)
        {
            created = DateTime.Now;
            Message = e.Message;
            Icon = Icons.GetMessageImage(e.Type);
        }

        public string Time { get { return string.Format("{0} {1}", created.ToShortDateString(), created.ToShortTimeString()); } }

        public string Message { get; private set; }

        public BitmapImage Icon { get; private set; }
    }
}
