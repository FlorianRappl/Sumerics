namespace FastColoredTextBoxNS
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    sealed class FCTBTypeDescriptor : CustomTypeDescriptor
    {
        readonly ICustomTypeDescriptor _parent;
        readonly Object _instance;

        public FCTBTypeDescriptor(ICustomTypeDescriptor parent, Object instance)
            : base(parent)
        {
            _parent = parent;
            _instance = instance;
        }

        public override String GetComponentName()
        {
            var ctrl = _instance as Control;
            return ctrl == null ? null : ctrl.Name;
        }

        public override EventDescriptorCollection GetEvents()
        {
            var coll = base.GetEvents();
            var list = new EventDescriptor[coll.Count];

            for (var i = 0; i < coll.Count; i++)
            {
                if (coll[i].Name == "TextChanged")//instead of TextChanged slip BindingTextChanged for binding
                {
                    list[i] = new FooTextChangedDescriptor(coll[i]);
                }
                else
                {
                    list[i] = coll[i];
                }
            }

            return new EventDescriptorCollection(list);
        }
    }
}
