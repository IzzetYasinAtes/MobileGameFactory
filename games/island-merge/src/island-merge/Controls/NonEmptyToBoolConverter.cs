using System.Globalization;

namespace IslandMerge.Controls;

/// <summary>
/// string -> bool: bos/null degilse true. IsVisible bindings icin.
/// </summary>
public sealed class NonEmptyToBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is string s && !string.IsNullOrWhiteSpace(s);

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
