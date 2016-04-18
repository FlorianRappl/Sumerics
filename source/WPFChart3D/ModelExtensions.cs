namespace WPFChart3D
{
    using System.Windows.Media;
    using WPFChart3D.Data;

    static class ModelExtensions
    {
        public static Color Convert(this WpfColor color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}
