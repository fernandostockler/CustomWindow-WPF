namespace CustomWindow_WPF;

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

/// <summary>
/// Defines the <see cref="DoubleToGridlenghtConverter" />.
/// </summary>
internal class DoubleToGridlenghtConverter : IValueConverter
{
    /// <summary>
    /// The Convert.
    /// </summary>
    /// <param name="value">The value<see cref="object"/>.</param>
    /// <param name="targetType">The targetType<see cref="Type"/>.</param>
    /// <param name="parameter">The parameter<see cref="object"/>.</param>
    /// <param name="culture">The culture<see cref="CultureInfo"/>.</param>
    /// <returns>The <see cref="object"/>.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => (value is double height)
            ? new GridLength(height)
            : value;

    /// <summary>
    /// The ConvertBack.
    /// </summary>
    /// <param name="value">The value<see cref="object"/>.</param>
    /// <param name="targetType">The targetType<see cref="Type"/>.</param>
    /// <param name="parameter">The parameter<see cref="object"/>.</param>
    /// <param name="culture">The culture<see cref="CultureInfo"/>.</param>
    /// <returns>The <see cref="object"/>.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}