namespace Sumerics.Controls
{
    using micautLib;
    using Sumerics.Resources;
    using System;
    using System.Diagnostics;

    public class MathInputPanelWrapper
    {
        readonly MathInputControl _panel;

        public event EventHandler<String> OnInsertPressed;

        public MathInputPanelWrapper(String caption = null)
        {
            caption = caption ?? Messages.DrawExpression;

            try
            {
                _panel = new MathInputControl();
                _panel.SetCaptionText(caption);
                _panel.EnableExtendedButtons(true);
                _panel.Insert += InsertMathInputPanel;
                _panel.Close += Close;
                IsAvailable = true;
            }
            catch(Exception ex)
            {
                Trace.WriteLine("MATH INPUT PANEL COULD NOT BE LOADED... See exception for details.");
                Trace.WriteLine(ex);
                IsAvailable = false;
            }
        }

        public Boolean IsAvailable
        {
            get;
            private set;
        }

        public void Open()
        {
            if (IsAvailable)
            {
                _panel.Show();
            }
        }

        public void Close()
        {
            if (IsAvailable)
            {
                _panel.Hide();
            }
        }

        void InsertMathInputPanel(String query)
        {
            _panel.Clear();

            if (OnInsertPressed != null)
            {
                OnInsertPressed(this, query);
            }
        }
    }
}
