using micautLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumerics.Controls
{
    public class MathInputPanelWrapper
    {
        MathInputControl mip;

        public event EventHandler<string> OnInsertPressed;

        public MathInputPanelWrapper(string caption = "Draw expression")
        {
            try
            {
                mip = new MathInputControl();
                mip.SetCaptionText(caption);
                mip.EnableExtendedButtons(true);
                mip.Insert += InsertMathInputPanel;
                mip.Close += Close;
                IsAvailable = true;
            }
            catch(Exception ex)
            {
                Trace.WriteLine("MATH INPUT PANEL COULD NOT BE LOADED... See exception for details.");
                Trace.WriteLine(ex);
                IsAvailable = false;
            }
        }

        public bool IsAvailable
        {
            get;
            private set;
        }

        public void Open()
        {
            if(IsAvailable)
                mip.Show();
        }

        public void Close()
        {
            if(IsAvailable)
                mip.Hide();
        }

        void InsertMathInputPanel(string query)
        {
            mip.Clear();

            if (OnInsertPressed != null)
                OnInsertPressed(this, query);
        }
    }
}
