namespace FastColoredTextBoxNS
{
    using System;
    using System.ComponentModel;

    sealed class FCTBDescriptionProvider : TypeDescriptionProvider
    {
        public FCTBDescriptionProvider(Type type)
            : base(GetDefaultTypeProvider(type))
        {
        }

        static TypeDescriptionProvider GetDefaultTypeProvider(Type type)
        {
            return TypeDescriptor.GetProvider(type);
        }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, Object instance)
        {
            var defaultDescriptor = base.GetTypeDescriptor(objectType, instance);
            return new FCTBTypeDescriptor(defaultDescriptor, instance);
        }
    }
}
