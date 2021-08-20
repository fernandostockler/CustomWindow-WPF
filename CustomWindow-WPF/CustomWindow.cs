namespace CustomWindow_WPF
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shell;

    [TemplatePart(Name = PART_TitleBar, Type = typeof(ButtonBase))]
    [TemplatePart(Name = PART_MaximizeRestoreButton, Type = typeof(ButtonBase))]
    public partial class CustomWindow : Window
    {
        /// <summary>
        /// Defines the MaximazedThickness.
        /// </summary>
        private Thickness MaximazedThickness = new(7);

        /// <summary>
        /// Defines the NormalThickness.
        /// </summary>
        private Thickness NormalThickness = new(0);

        static CustomWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomWindow), new FrameworkPropertyMetadata(typeof(CustomWindow)));
        }

        public CustomWindow()
        {
            _ = CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseWindow));
            _ = CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, MaximizeRestoreWindow, CanResizeWindow));
            _ = CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, MaximizeRestoreWindow, CanResizeWindow));
            _ = CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, MinimizeWindow, CanMinimizeWindow));

            Loaded += CustomWindow_Loaded;
            StateChanged += CustomWindow_StateChanged;
        }

        private void CustomWindow_Loaded(object sender, RoutedEventArgs e)
        {
            OnTitleBarHeightChanged(TitleBarHeight);
        }

        private void CustomWindow_StateChanged(object sender, EventArgs e)
        {
            bool WindowStateIsNormal = WindowState == WindowState.Normal;
            MaximizeRestoreButton.Content = WindowStateIsNormal ? RestoreGlyph : MaximizeGlyph;
            MaximizeRestoreButton.ToolTip = WindowStateIsNormal ? MaximizeToolTip : RestoreToolTip;
            Margin = WindowState == WindowState.Maximized ? MaximazedThickness : NormalThickness;
        }

        /// <summary>
        /// Gets or sets a FrameworkElement value that represents a non-client title bar area except the buttons area.
        /// </summary>
        [Category(Comum)]
        [Description("Gets or sets a FrameworkElement value that represents a non-client title bar area except the buttons area.")]
        public FrameworkElement TitleBar
        {
            get => (FrameworkElement)GetValue(TitleBarProperty);
            set => SetValue(TitleBarProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="TitleBar"/> dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the <see cref="TitleBar"/> dependency property.
        /// </returns>
        public static readonly DependencyProperty TitleBarProperty = DependencyProperty.Register(
            name: nameof(TitleBar),
            propertyType: typeof(FrameworkElement),
            ownerType: typeof(CustomWindow),
            typeMetadata: new PropertyMetadata(defaultValue: null));

        [Category(Comum)]
        [Description("Obtem ou define a altura da barra de título (parte não cliente).")]
        public double TitleBarHeight
        {
            get => (double)GetValue(TitleBarHeightProperty);
            set => SetValue(TitleBarHeightProperty, value);
        }

        public static readonly DependencyProperty TitleBarHeightProperty =
            DependencyProperty.Register(
                name: nameof(TitleBarHeight),
                propertyType: typeof(double),
                ownerType: typeof(CustomWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: 42.0,

                    propertyChangedCallback: (d, e) =>
                    {
                        CustomWindow win = (CustomWindow)d;
                        double newValue = (double)e.NewValue;
                        win.OnTitleBarHeightChanged(newValue);
                    },

                    coerceValueCallback: (d, baseValue) =>
                    {
                        return (baseValue is double value && value < 36.0) ? 36.0 : baseValue;
                    }),

                    validateValueCallback: (value) => value switch
                    {
                        double dvalue => !double.IsNaN(dvalue) &&
                            !double.IsNegativeInfinity(dvalue) &&
                            !double.IsPositiveInfinity(dvalue),
                        _ => false
                    });

        private void OnTitleBarHeightChanged(double newValue)
        {
            WindowChrome.SetWindowChrome(this, new WindowChrome()
            {
                CaptionHeight = newValue,
                CornerRadius = new CornerRadius(0),
                ResizeBorderThickness = new Thickness(6)
            });
        }

        [Description("Obtem ou define um pincel que descreve a cor do primeiro plano da barra de título da janela.")]
        public Brush TitleBarForeground
        {
            get => (Brush)GetValue(TitleBarForegroundProperty);
            set => SetValue(TitleBarForegroundProperty, value);
        }

        public static readonly DependencyProperty TitleBarForegroundProperty =
            DependencyProperty.Register(
                name: nameof(TitleBarForeground),
                propertyType: typeof(Brush),
                ownerType: typeof(CustomWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: Brushes.Black));

        [Description("Obtem ou define um pincel que descreve o plano de fundo da barra do título da janela.")]
        public Brush TitleBarBackground
        {
            get => (Brush)GetValue(TitleBarBackgroundProperty);
            set => SetValue(TitleBarBackgroundProperty, value);
        }

        public static readonly DependencyProperty TitleBarBackgroundProperty =
            DependencyProperty.Register(
                name: nameof(TitleBarBackground),
                propertyType: typeof(Brush),
                ownerType: typeof(CustomWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: Brushes.White,
                    propertyChangedCallback: (d,e) =>
                    {
                        CustomWindow win = (CustomWindow)d;
                        Brush newValue = (Brush)e.NewValue;
                        BackgroundToForegroundConverter converter = BackgroundToForegroundConverter.Instance;
                        Brush newIdealForeground = converter.Convert(newValue, typeof(Brush), null, CultureInfo.CurrentCulture) as Brush;
                        Brush oldValue = win.TitleBarForeground;
                        win.TitleBarForeground = newIdealForeground;
                        win.OnPropertyChanged(new(TitleBarForegroundProperty, oldValue, newIdealForeground));
                    }));

        [Description("Obtem ou define um pincel que representa o plano de fundo da camada que cobre a janela.")]
        public Brush OverlayBackground
        {
            get => (Brush)GetValue(OverlayBackgroundProperty);
            set => SetValue(OverlayBackgroundProperty, value);
        }

        public static readonly DependencyProperty OverlayBackgroundProperty =
            DependencyProperty.Register(
                name: nameof(OverlayBackground),
                propertyType: typeof(Brush),
                ownerType: typeof(CustomWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: Brushes.Gray));

        [Category(Aparência)]
        [Description("Obtem ou define a visibilidade da camada que cobre a janela.")]
        public bool ShowMessage { get => (bool)GetValue(ShowMessageProperty); set => SetValue(ShowMessageProperty, value); }

        public static readonly DependencyProperty ShowMessageProperty =
            DependencyProperty.Register(
                name: nameof(ShowMessage),
                propertyType: typeof(bool),
                ownerType: typeof(CustomWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: false));

        public FrameworkElement Message
        {
            get => (FrameworkElement)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(
                name: nameof(Message),
                propertyType: typeof(FrameworkElement),
                ownerType: typeof(CustomWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: null));

        public Brush MessageBackground
        {
            get => (Brush)GetValue(MessageBackgroundProperty);
            set => SetValue(MessageBackgroundProperty, value);
        }

        public static readonly DependencyProperty MessageBackgroundProperty =
            DependencyProperty.Register(
                name: nameof(MessageBackground),
                propertyType: typeof(Brush),
                ownerType: typeof(CustomWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: Brushes.DarkBlue));

        private void CloseWindow(object sender, ExecutedRoutedEventArgs e) => Close();

        private void CanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = ResizeMode != ResizeMode.NoResize;

        private void CanResizeWindow(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = ResizeMode is ResizeMode.CanResize or ResizeMode.CanResizeWithGrip;

        private void MinimizeWindow(object sender, ExecutedRoutedEventArgs e) => SystemCommands.MinimizeWindow(this);

        private void MaximizeRestoreWindow(object sender, ExecutedRoutedEventArgs e)
        {
            if ( sender is Window window && window != null )
            {
                window.WindowState = (window.WindowState == WindowState.Normal)
                    ? WindowState.Maximized
                    : WindowState.Normal;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            MaximizeRestoreButton = GetTemplateChild<Button>(PART_MaximizeRestoreButton);
        }

        /// <summary>
        /// Returns the named element in the visual tree of an instantiated System.Windows.Controls.ControlTemplate.
        /// </summary>
        /// <typeparam name="T">Type of the child to find.</typeparam>
        /// <param name="childName">Name of the child to find.</param>
        /// <returns>The requested element. May be null if no element of the requested name exists.</returns>
        /// <exception cref="MissingTemplatePartException"> will be lanched if the child is null.</exception>
        protected T GetTemplateChild<T>(string childName) where T : DependencyObject
        {
            T child = (T)GetTemplateChild(childName);
            return child is null ? throw new MissingTemplatePartException(childName, typeof(T)) : child;
        }

        internal Button MaximizeRestoreButton { get; private set; }

        internal Border OutterBorder { get; private set; }

        private const string MaximizeGlyph = "\uE923";

        private const string RestoreGlyph = "\uE922";

        private const string MaximizeToolTip = "Maximizar";

        private const string RestoreToolTip = "Restaurar";

        private const string Aparência = "Aparência";

        private const string Comum = "Comum";

        private const string PART_TitleBar = "PART_TitleBar";

        private const string PART_MaximizeRestoreButton = "PART_MaximizeRestoreButton";
    }
}