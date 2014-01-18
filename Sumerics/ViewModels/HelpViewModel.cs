using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using YAMP;
using YAMP.Help;

namespace Sumerics
{
    class HelpViewModel : BaseViewModel
    {
        #region Members

        HelpSection help;
        BitmapImage icon;
        Regex endOfSentence = new Regex(@"\.\s[A-Z]+", RegexOptions.Compiled);

        #endregion

        #region ctor

        public HelpViewModel(HelpSection entry)
		{
			help = entry;
            icon = Icons.GetLowImage(entry.Topic);
		}

        #endregion

        #region Properties

        public string Name
        {
            get { return help.Name; }
        }

		public string Description
		{
			get { return help.Description; }
		}

        public string ToolTip
        {
            get 
            {
                var str = help.Description;

                if (endOfSentence.IsMatch(str))
                {
                    var index = endOfSentence.Match(str).Index;
                    str = str.Substring(0, index + 1);
                }

                if (str.Length > 70)
                    str = str.Substring(0, 66) + " ...";

                return str;
            }
        }

		public HelpSection Help
        {
            get { return help; }
        }

        public BitmapImage Icon
		{
			get
			{
				return icon;
			}
		}

        public ICommand ShowMore
        {
            get
            {
                return new RelayCommand(x =>
                {
					var hw = StaticHelpers.GetWindow<HelpWindow>();
                    hw.Topic = help;
                });
            }
        }

        #endregion
    }
}
