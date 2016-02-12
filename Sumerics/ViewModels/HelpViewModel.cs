namespace Sumerics.ViewModels
{
    using Sumerics.Views;
    using System;
    using System.Text.RegularExpressions;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;
    using YAMP.Help;

    public sealed class HelpViewModel : BaseViewModel
    {
        #region Fields

        static readonly Regex EndOfSentence = new Regex(@"\.\s[A-Z]+", RegexOptions.Compiled);

        readonly HelpSection _help;
        readonly BitmapImage _icon;
        readonly ICommand _show;

        #endregion

        #region ctor

        public HelpViewModel(HelpSection entry, IComponents container)
            : base(container)
		{
			_help = entry;
            _icon = Icons.GetLowImage(entry.Topic);
            _show = new RelayCommand(x =>
            {
                var hw = StaticHelpers.GetWindow<HelpWindow>(Container);
                hw.Topic = _help;
            });
		}

        #endregion

        #region Properties

        public String Name
        {
            get { return _help.Name; }
        }

		public String Description
		{
			get { return _help.Description; }
		}

        public String ToolTip
        {
            get 
            {
                var str = _help.Description;

                if (EndOfSentence.IsMatch(str))
                {
                    var index = EndOfSentence.Match(str).Index;
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
            get { return _help; }
        }

        public BitmapImage Icon
		{
			get { return _icon; }
		}

        public ICommand ShowMore
        {
            get { return _show; }
        }

        #endregion
    }
}
