namespace Sumerics
{
    using System;
    using System.Text.RegularExpressions;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;
    using YAMP.Help;

    sealed class HelpViewModel : BaseViewModel
    {
        #region Fields

        HelpSection help;
        BitmapImage icon;
        Regex endOfSentence = new Regex(@"\.\s[A-Z]+", RegexOptions.Compiled);

        #endregion

        #region ctor

        public HelpViewModel(HelpSection entry, IContainer container)
            : base(container)
		{
			help = entry;
            icon = Icons.GetLowImage(entry.Topic);
		}

        #endregion

        #region Properties

        public String Name
        {
            get { return help.Name; }
        }

		public String Description
		{
			get { return help.Description; }
		}

        public String ToolTip
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
                {
                    str = str.Substring(0, 66) + " ...";
                }

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
					var hw = StaticHelpers.GetWindow<HelpWindow>(Container);
                    hw.Topic = help;
                });
            }
        }

        #endregion
    }
}
