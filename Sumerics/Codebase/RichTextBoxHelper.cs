namespace Sumerics
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;

    public class RichTextBoxHelper : DependencyObject
    {
        public static FlowDocument GetBoundDocument(DependencyObject obj)
        {
            return (FlowDocument)obj.GetValue(BoundDocumentProperty);
        }

        public static void SetBoundDocument(DependencyObject obj, FlowDocument value)
        {
            obj.SetValue(BoundDocumentProperty, value);
        }

        public static readonly DependencyProperty BoundDocumentProperty =
          DependencyProperty.RegisterAttached(
            "BoundDocument",
            typeof(FlowDocument),
            typeof(RichTextBoxHelper),
            new FrameworkPropertyMetadata
            {
                BindsTwoWayByDefault = true,
                PropertyChangedCallback = (sender, e) =>
                {
                    var richTextBox = (RichTextBox)sender;
                    var document = GetBoundDocument(richTextBox);
                    richTextBox.Document = document;
                }
            });
    }
}
