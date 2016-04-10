namespace FastColoredTextBoxNS
{
    using System;
    using System.ComponentModel;

    sealed class FooTextChangedDescriptor : EventDescriptor
    {
        public FooTextChangedDescriptor(MemberDescriptor desc)
            : base(desc)
        {
        }

        public override void AddEventHandler(Object component, Delegate value)
        {
            var tb = component as FastColoredTextBox;
            var ev = value as EventHandler;

            if (tb != null && ev != null)
            {
                tb.BindingTextChanged += ev;
            }
        }

        public override void RemoveEventHandler(Object component, Delegate value)
        {
            var tb = component as FastColoredTextBox;
            var ev = value as EventHandler;

            if (tb != null && ev != null)
            {
                tb.BindingTextChanged -= ev;
            }
        }

        public override Type ComponentType
        {
            get { return typeof(FastColoredTextBox); }
        }

        public override Type EventType
        {
            get { return typeof(EventHandler); }
        }

        public override Boolean IsMulticast
        {
            get { return true; }
        }
    }
}
