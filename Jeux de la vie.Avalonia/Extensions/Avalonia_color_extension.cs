using Avalonia.Media;
using Avalonia.Media.Immutable;

namespace Jeux_de_la_vie.Avalonia.Extensions
{
    internal static class Avalonia_color_extension
    {
        public static System.Drawing.Color ToNativeColor(this Color color) =>
            System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        public static Color ToColor(this IBrush brush)
        {
            if (brush is SolidColorBrush solidColorBrush)
                return solidColorBrush.Color;
            else if (brush is ImmutableSolidColorBrush immutableSolidColorBrush)
                return immutableSolidColorBrush.Color;
            else
                return Colors.White;
        }
    }
}
