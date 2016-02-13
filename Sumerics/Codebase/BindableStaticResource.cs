namespace Sumerics
{
    using System;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;

	public class BindableStaticResource : StaticResourceExtension
	{
        static readonly DependencyProperty DummyProperty = DependencyProperty.RegisterAttached(
            "Dummy", typeof(Object), typeof(DependencyObject), new UIPropertyMetadata(null));

		public Binding MyBinding 
        { 
            get; 
            set; 
        }

		public BindableStaticResource()
		{
		}

		public BindableStaticResource(Binding binding)
		{
			MyBinding = binding;
		}

		public override Object ProvideValue(IServiceProvider serviceProvider)
		{
            var type = typeof(IProvideValueTarget);
			var target = (IProvideValueTarget)serviceProvider.GetService(type);
			var targetObject = (FrameworkElement)target.TargetObject;
			MyBinding.Source = targetObject.DataContext;
			var dummy = new DependencyObject();
			BindingOperations.SetBinding(dummy, DummyProperty, MyBinding);
			ResourceKey = dummy.GetValue(DummyProperty);
			return base.ProvideValue(serviceProvider);
		}
	}
}
