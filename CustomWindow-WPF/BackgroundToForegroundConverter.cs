namespace CustomWindow_WPF
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// Defines the <see cref="BackgroundToForegroundConverter" />.
    /// </summary>
    public class BackgroundToForegroundConverter : IValueConverter, IMultiValueConverter
    {
        /// <summary>
        /// Defines the _instance.
        /// </summary>
        private static BackgroundToForegroundConverter? _instance;

        // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        /// <summary>
        /// Initializes static members of the <see cref="BackgroundToForegroundConverter"/> class.
        /// </summary>
        static BackgroundToForegroundConverter()
        {
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="BackgroundToForegroundConverter"/> class from being created.
        /// </summary>
        private BackgroundToForegroundConverter()
        {
        }

        /// <summary>
        /// Gets the Instance.
        /// </summary>
        public static BackgroundToForegroundConverter Instance => _instance ??= new BackgroundToForegroundConverter();

        /// <summary>
        /// The Convert.
        /// </summary>
        /// <param name="value">The value<see cref="object"/>.</param>
        /// <param name="targetType">The targetType<see cref="Type"/>.</param>
        /// <param name="parameter">The parameter<see cref="object"/>.</param>
        /// <param name="culture">The culture<see cref="CultureInfo"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if ( value is SolidColorBrush brush && brush != null )
            {
                Color idealForegroundColor = IdealTextColor(brush.Color);
                SolidColorBrush? foreGroundBrush = new(idealForegroundColor);
                foreGroundBrush.Freeze();
                return foreGroundBrush;
            }
            return Brushes.White;
        }

        /// <summary>
        /// The ConvertBack.
        /// </summary>
        /// <param name="value">The value<see cref="object"/>.</param>
        /// <param name="targetType">The targetType<see cref="Type"/>.</param>
        /// <param name="parameter">The parameter<see cref="object"/>.</param>
        /// <param name="culture">The culture<see cref="CultureInfo"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

        /// <summary>
        /// The Convert.
        /// </summary>
        /// <param name="values">The values<see cref="object"/>.</param>
        /// <param name="targetType">The targetType<see cref="Type"/>.</param>
        /// <param name="parameter">The parameter<see cref="object"/>.</param>
        /// <param name="culture">The culture<see cref="CultureInfo"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Brush? bgBrush = values.Length > 0 ? values[0] as Brush : null;
            Brush? titleBrush = values.Length > 1 ? values[1] as Brush : null;
#pragma warning disable CS8604 // Possível argumento de referência nula.
            // bgBrush may be null however the converter always returns a non-null Brush.
            return titleBrush ?? Convert(bgBrush, targetType, parameter, culture);
#pragma warning restore CS8604 // Possível argumento de referência nula.
        }

        /// <summary>
        /// The ConvertBack.
        /// </summary>
        /// <param name="value">The value<see cref="object"/>.</param>
        /// <param name="targetTypes">The targetTypes<see cref="Type"/>.</param>
        /// <param name="parameter">The parameter<see cref="object"/>.</param>
        /// <param name="culture">The culture<see cref="CultureInfo"/>.</param>
        /// <returns>The <see cref="object"/> array.</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return targetTypes.Select(t => DependencyProperty.UnsetValue).ToArray();
        }

        /// <summary>
        /// Determining Ideal Text Color Based on Specified Background Color
        /// http://www.codeproject.com/KB/GDI-plus/IdealTextColor.aspx.
        /// </summary>
        /// <param name="bg">The bg<see cref="Color"/>.</param>
        /// <returns>.</returns>
        private static Color IdealTextColor(Color bg)
        {
            const int nThreshold = 86; //105;
            int bgDelta = System.Convert.ToInt32((bg.R * 0.299) + (bg.G * 0.587) + (bg.B * 0.114));
            Color foreColor = (255 - bgDelta < nThreshold) ? Colors.Black : Colors.White;
            return foreColor;
        }
    }
}
