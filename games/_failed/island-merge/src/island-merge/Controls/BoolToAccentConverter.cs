using System.Globalization;

namespace IslandMerge.Controls;

/// <summary>
/// Bool -> renk donusumu. True: altin vurgu (#F5A623), False: ormana yakin (#1F7A6C).
/// </summary>
public sealed class BoolToAccentConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var on = value is bool b && b;
        var hex = on ? "#F5A623" : "#1F7A6C";
        if (targetType == typeof(Color) || targetType.Name == "Brush")
        {
            return Color.FromArgb(hex);
        }
        return hex;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is Color c && c == Color.FromArgb("#F5A623");
}
