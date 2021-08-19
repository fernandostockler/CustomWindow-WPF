namespace CustomWindow_WPF
{
    using System.ComponentModel;
    using System.Globalization;
using System.Windows;
    using System.Windows.Media;
    using System.Windows.Shell;

    public class CustomWindow : Window
    {
        private double OriginalTitleBarHeight = 42.0;
        private const string ClassName = nameof(CustomWindow);
        private enum TaskbarPosition
        {
            None,
            Top,
            Left,
            Bottom,
            Right
        }

        private static readonly Thickness NormalThickness = new(0);

        private static double WindowsTaskbarHeight
            => SystemParameters.PrimaryScreenHeight
            - SystemParameters.FullPrimaryScreenHeight
            - SystemParameters.WindowCaptionHeight;

        private static double WindowsTaskbarWidth
            => SystemParameters.PrimaryScreenWidth
            - SystemParameters.FullPrimaryScreenWidth;

        /// <summary>
        /// The default value for ResizeBorderThickness dependency property.
        /// </summary>
        protected const int ResizeBorderThicknessDefaultValue = 6;

        static CustomWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomWindow), new FrameworkPropertyMetadata(typeof(CustomWindow)));
        }

        public CustomWindow()
        {
            //WindowStyle = WindowStyle.None;
            //AllowsTransparency = true;
            Loaded += CustomWindow_Loaded;
            StateChanged += CustomWindow_StateChanged;
        }

        private void CustomWindow_StateChanged(object? sender, System.EventArgs e)
        {
            //TODO: sem efeito.

            Margin = WindowState == WindowState.Maximized ? GetMaximazedThickness() : NormalThickness;

            Thickness GetMaximazedThickness()
            {
                Thickness resizeBorderThickness = WindowChrome.GetWindowChrome(this).ResizeBorderThickness;

                Thickness thickness = new(
                    left: resizeBorderThickness.Left + BorderThickness.Left,
                    top: resizeBorderThickness.Top + BorderThickness.Top,
                    right: resizeBorderThickness.Right + BorderThickness.Right,
                    bottom: resizeBorderThickness.Bottom + BorderThickness.Bottom);

                switch ( GetTaskbarPosition() )
                {
                    case TaskbarPosition.Top:
                        thickness.Top += WindowsTaskbarHeight;
                        break;
                    case TaskbarPosition.Left:
                        thickness.Left += WindowsTaskbarWidth;
                        break;
                    case TaskbarPosition.Bottom:
                        thickness.Bottom += WindowsTaskbarHeight;
                        break;
                    case TaskbarPosition.Right:
                        thickness.Right += WindowsTaskbarWidth;
                        break;
                    case TaskbarPosition.None:
                        break;
                    default:
                        break;
                }
                return thickness;
            }

            TaskbarPosition GetTaskbarPosition()
            {
                if ( SystemParameters.WorkArea.Top > 0 )
                {
                    return TaskbarPosition.Top;
                }
                else if ( SystemParameters.WorkArea.Left > 0 )
                {
                    return TaskbarPosition.Left;
                }
                else if ( WindowsTaskbarWidth == 0 )
                {
                    return TaskbarPosition.Bottom;
                }
                else if ( WindowsTaskbarHeight < WindowChrome.GetWindowChrome(this).CaptionHeight )
                {
                    return TaskbarPosition.Right;
                }
                return TaskbarPosition.None;
            }
        }

        private void CustomWindow_Loaded(object sender, RoutedEventArgs e)
        {
            OriginalTitleBarHeight = TitleBarHeight;

            // Do not change the order of lines of code below.
            // OnKioskChanged() affects the value of the TitleBarHeight property.
            OnTitleBarHeightChanged(TitleBarHeight);
            //OnKioskModeChanged(KioskMode);
        }

        /// <summary>
        /// Gets or sets a double value representing the minimum title bar's height.
        /// </summary>
        [Category(ClassName)]
        [Description("Gets or sets a double value representing the minimum title bar's height.")]
        public double MinTitleBarHeight
        {
            get => (double)GetValue(MinTitleBarHeightProperty);
            set => SetValue(MinTitleBarHeightProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="MinTitleBarHeight"/> dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the <see cref="MinTitleBarHeight"/> dependency property.
        /// </returns>
        public static readonly DependencyProperty MinTitleBarHeightProperty =
            DependencyProperty.Register(
                name: nameof(MinTitleBarHeight),
                propertyType: typeof(double),
                ownerType: typeof(CustomWindow),
                typeMetadata: new PropertyMetadata(36.0));

        /// <summary>
        /// Gets or sets the height of the title bar (non-client area).
        /// </summary>
        [Category(ClassName)]
        [Description("Gets or sets the height of the title bar (non-client area).")]
        public double TitleBarHeight
        {
            get => (double)GetValue(TitleBarHeightProperty);
            set => SetValue(TitleBarHeightProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="TitleBarHeight"/> dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the <see cref="TitleBarHeight"/> dependency property.
        /// </returns>
        public static readonly DependencyProperty TitleBarHeightProperty =
            DependencyProperty.Register(
                name: nameof(TitleBarHeight),
                propertyType: typeof(double),
                ownerType: typeof(CustomWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: 42.0,
                    propertyChangedCallback: (d, e) =>
                    {
                        double newValue = (double)e.NewValue;
                        CustomWindow customWindow = (CustomWindow)d;
                        customWindow.OnTitleBarHeightChanged(newValue);
                    },
                    coerceValueCallback: (d, baseValue) =>
                    {
                        CustomWindow customWindow = (CustomWindow)d;
                        return (baseValue is double value && value < customWindow.MinTitleBarHeight) ? customWindow.MinTitleBarHeight : baseValue;
                    }),
                    validateValueCallback: (value) =>
                    {
                        return value switch
                        {
                            double dvalue => !double.IsNaN(dvalue) && !double.IsNegativeInfinity(dvalue) && !double.IsPositiveInfinity(dvalue),
                            _ => false
                        };
                    });

        private void OnTitleBarHeightChanged(double newValue)
        {
            WindowChrome.SetWindowChrome(this, new WindowChrome()
            {
                CaptionHeight = newValue,
                CornerRadius = new CornerRadius(0),
                ResizeBorderThickness = new Thickness(ResizeBorderThicknessDefaultValue)
            });
        }

        /// <summary>
        /// Gets or sets the window's corner radius.
        /// </summary>
        [Category(ClassName)]
        [Description("Gets or sets the window's corner radius.")]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the <see cref="CornerRadius"/> dependency property.
        /// </returns>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            name: nameof(CornerRadius),
            propertyType: typeof(CornerRadius),
            ownerType: typeof(CustomWindow),
            typeMetadata: new PropertyMetadata(defaultValue: new CornerRadius(5)));

        /// <summary>
        /// Gets or sets a brush that describes the foreground color of the window's title bar. Automatically calculated by OnTitleBarBackgroundChanged(d, e) when TitleBarForegroundIsAutomated is true.
        /// </summary>
        [Category(ClassName)]
        [Description("Gets or sets a brush that describes the foreground color of the window's title bar. Automatically calculated by OnTitleBarBackgroundChanged(d, e) when TitleBarForegroundIsAutomated is true.")]
        public Brush TitleBarForeground
        {
            get => (Brush)GetValue(TitleBarForegroundProperty);
            set => SetValue(TitleBarForegroundProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="TitleBarForeground"/> dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the <see cref="TitleBarForeground"/> dependency property.
        /// </returns>
        public static readonly DependencyProperty TitleBarForegroundProperty =
            DependencyProperty.Register(
                name: nameof(TitleBarForeground),
                propertyType: typeof(Brush),
                ownerType: typeof(CustomWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: Brushes.Black));

        /// <summary>
        /// Gets or sets a Boolean value representing whether or not the title bar foreground will automatically adapt to a new background.
        /// </summary>
        [Category(ClassName)]
        [Description("Gets or sets a Boolean value representing whether or not the title bar foreground will automatically adapt to a new background.")]
        public bool TitleBarForegroundIsAutomated
        {
            get => (bool)GetValue(TitleBarForegroundIsAutomatedProperty);
            set => SetValue(TitleBarForegroundIsAutomatedProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="TitleBarForegroundIsAutomated"/> dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the <see cref="TitleBarForegroundIsAutomated"/> dependency property.
        /// </returns>
        public static readonly DependencyProperty TitleBarForegroundIsAutomatedProperty =
            DependencyProperty.Register(
                name: nameof(TitleBarForegroundIsAutomated),
                propertyType: typeof(bool),
                ownerType: typeof(CustomWindow),
                typeMetadata: new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets the brush that describes the background of the window's title bar.
        /// </summary>
        [Category(ClassName)]
        [Description("Gets or sets the brush that describes the background of the window's title bar.")]
        public Brush TitleBarBackground
        {
            get => (Brush)GetValue(TitleBarBackgroundProperty);
            set => SetValue(TitleBarBackgroundProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="TitleBarBackground"/> dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the <see cref="TitleBarBackground"/> dependency property.
        /// </returns>
        public static readonly DependencyProperty TitleBarBackgroundProperty =
            DependencyProperty.Register(
                name: nameof(TitleBarBackground),
                propertyType: typeof(Brush),
                ownerType: typeof(CustomWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: Brushes.White,
                    propertyChangedCallback: (d, e) =>
                    {
                        CustomWindow? win = (CustomWindow)d;
                        Brush? newValue = (Brush)e.NewValue;
                        BackgroundToForegroundConverter? converter = BackgroundToForegroundConverter.Instance;
                        Brush? newIdealForeground = converter.Convert(newValue, typeof(Brush), new object(), CultureInfo.CurrentCulture) as Brush;
                        win.TitleBarForeground = win.TitleBarForegroundIsAutomated ? newIdealForeground ?? SystemColors.HotTrackBrush : win.Foreground;
                    }));

    }
}