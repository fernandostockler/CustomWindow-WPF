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

    /// <summary>
    /// Defines the <see cref="CWindow" />.
    /// </summary>
    [TemplatePart(Name = PART_Icon, Type = typeof(ContentControl))]
    [TemplatePart(Name = PART_LeftArea, Type = typeof(ContentControl))]
    [TemplatePart(Name = PART_Title, Type = typeof(ContentControl))]
    [TemplatePart(Name = PART_RightArea, Type = typeof(ContentControl))]
    [TemplatePart(Name = PART_MinimizeButton, Type = typeof(ButtonBase))]
    [TemplatePart(Name = PART_MaximizeRestoreButton, Type = typeof(ButtonBase))]
    [TemplatePart(Name = PART_CloseButton, Type = typeof(ButtonBase))]
    public partial class CWindow : Window
    {
        /// <summary>
        /// Initializes static members of the <see cref="CWindow"/> class.
        /// </summary>
        static CWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CWindow), new FrameworkPropertyMetadata(typeof(CWindow)));
        }

        /// <summary>
        /// Gets or sets the IconTemplate.
        /// </summary>
        public DataTemplate IconTemplate { get => (DataTemplate)GetValue(IconTemplateProperty); set => SetValue(IconTemplateProperty, value); }

        /// <summary>
        /// Defines the IconTemplateProperty.
        /// </summary>
        public static readonly DependencyProperty IconTemplateProperty =
            DependencyProperty.Register(
                name: nameof(IconTemplate),
                propertyType: typeof(DataTemplate),
                ownerType: typeof(CWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: null));

        /// <summary>
        /// Gets or sets the IconArea.
        /// </summary>
        [Category(Comum)]
        public FrameworkElement IconArea { get => (FrameworkElement)GetValue(IconAreaProperty); set => SetValue(IconAreaProperty, value); }

        /// <summary>
        /// Defines the IconAreaProperty.
        /// </summary>
        public static readonly DependencyProperty IconAreaProperty =
            DependencyProperty.Register(
                name: nameof(IconArea),
                propertyType: typeof(FrameworkElement),
                ownerType: typeof(CWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: null));

        /// <summary>
        /// Gets or sets the TitleBarHeight.
        /// </summary>
        [Category(Comum)]
        [Description("Obtem ou define a altura da barra de título (parte não cliente).")]
        public double TitleBarHeight { get => (double)GetValue(TitleBarHeightProperty); set => SetValue(TitleBarHeightProperty, value); }

        /// <summary>
        /// Defines the TitleBarHeightProperty.
        /// </summary>
        public static readonly DependencyProperty TitleBarHeightProperty =
            DependencyProperty.Register(
                name: nameof(TitleBarHeight),
                propertyType: typeof(double),
                ownerType: typeof(CWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: 42.0,
                    propertyChangedCallback: OnTitleBarHeightChanged,
                    coerceValueCallback: OnCoerceTitleBarHeight),
                    validateValueCallback: OnValidateTitleBarHeight);

        /// <summary>
        /// The OnCoerceTitleBarHeight.
        /// </summary>
        /// <param name="d">The d<see cref="DependencyObject"/>.</param>
        /// <param name="baseValue">The baseValue<see cref="object"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        private static object OnCoerceTitleBarHeight(DependencyObject d, object baseValue)
        {
            return (baseValue is double value && value < 36.0) ? 36.0 : baseValue;
        }

        /// <summary>
        /// The OnTitleBarHeightChanged.
        /// </summary>
        /// <param name="d">The d<see cref="DependencyObject"/>.</param>
        /// <param name="e">The e<see cref="DependencyPropertyChangedEventArgs"/>.</param>
        private static void OnTitleBarHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CWindow win = (CWindow)d;
            double newValue = (double)e.NewValue;
            WindowChrome.SetWindowChrome(win, new WindowChrome()
            {
                CaptionHeight = newValue,
                CornerRadius = new CornerRadius(0),
                ResizeBorderThickness = new Thickness(6)
            });
        }

        /// <summary>
        /// The OnValidateTitleBarHeight.
        /// </summary>
        /// <param name="value">The value<see cref="object"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private static bool OnValidateTitleBarHeight(object value)
        {
            return value switch
            {
                double dvalue => !double.IsNaN(dvalue) &&
                    !double.IsNegativeInfinity(dvalue) &&
                    !double.IsPositiveInfinity(dvalue),
                _ => false
            };
        }

        /// <summary>
        /// Gets or sets the TitleBarLeftArea.
        /// </summary>
        [Category(Comum)]
        [Description("Obtem ou define um FrameworkElement personalizado localizado na parte esquerda da barra de título, logo após o ícone da janela.")]
        public FrameworkElement TitleBarLeftArea { get => (FrameworkElement)GetValue(TitleBarLeftAreaProperty); set => SetValue(TitleBarLeftAreaProperty, value); }

        /// <summary>
        /// Defines the TitleBarLeftAreaProperty.
        /// </summary>
        public static readonly DependencyProperty TitleBarLeftAreaProperty =
            DependencyProperty.Register(
                name: nameof(TitleBarLeftArea),
                propertyType: typeof(FrameworkElement),
                ownerType: typeof(CWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: null));

        /// <summary>
        /// Gets or sets the TitleBarRightArea.
        /// </summary>
        [Category(Comum)]
        [Description("Obtem ou define um FrameworkElement personalizado localizado na parte direita da barra de título, antes dos botões da janela.")]
        public FrameworkElement TitleBarRightArea { get => (FrameworkElement)GetValue(TitleBarRightAreaProperty); set => SetValue(TitleBarRightAreaProperty, value); }

        /// <summary>
        /// Defines the TitleBarRightAreaProperty.
        /// </summary>
        public static readonly DependencyProperty TitleBarRightAreaProperty =
            DependencyProperty.Register(
                name: nameof(TitleBarRightArea),
                propertyType: typeof(FrameworkElement),
                ownerType: typeof(CWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: null));

        /// <summary>
        /// Gets or sets the TitleBarForeground.
        /// </summary>
        [Description("Obtem ou define um pincel que descreve a cor do primeiro plano da barra de título da janela.")]
        public Brush TitleBarForeground { get => (Brush)GetValue(TitleBarForegroundProperty); set => SetValue(TitleBarForegroundProperty, value); }

        /// <summary>
        /// Defines the TitleBarForegroundProperty.
        /// </summary>
        public static readonly DependencyProperty TitleBarForegroundProperty =
            DependencyProperty.Register(
                name: nameof(TitleBarForeground),
                propertyType: typeof(Brush),
                ownerType: typeof(CWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: Brushes.Black));

        /// <summary>
        /// Gets or sets the TitleBarBackground.
        /// </summary>
        [Description("Obtem ou define um pincel que descreve o plano de fundo da barra do título da janela.")]
        public Brush TitleBarBackground { get => (Brush)GetValue(TitleBarBackgroundProperty); set => SetValue(TitleBarBackgroundProperty, value); }

        /// <summary>
        /// Defines the TitleBarBackgroundProperty.
        /// </summary>
        public static readonly DependencyProperty TitleBarBackgroundProperty =
            DependencyProperty.Register(
                name: nameof(TitleBarBackground),
                propertyType: typeof(Brush),
                ownerType: typeof(CWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: Brushes.White,
                    propertyChangedCallback: OnTitleBarBackgroundChanged));

        /// <summary>
        /// The OnTitleBarBackgroundChanged.
        /// </summary>
        /// <param name="d">The d<see cref="DependencyObject"/>.</param>
        /// <param name="e">The e<see cref="DependencyPropertyChangedEventArgs"/>.</param>
        private static void OnTitleBarBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CWindow win = (CWindow)d;
            Brush newValue = (Brush)e.NewValue;
            BackgroundToForegroundConverter converter = BackgroundToForegroundConverter.Instance;
            Brush? newIdealForeground = converter.Convert(newValue, typeof(Brush), new object(), CultureInfo.CurrentCulture) as Brush;
            win.TitleBarForeground = newIdealForeground ?? new SolidColorBrush(Colors.White);
        }

        /// <summary>
        /// The OnTitleBarBackgroundChanged.
        /// </summary>
        /// <param name="newValue">The newValue<see cref="Brush"/>.</param>
        private void OnTitleBarBackgroundChanged(Brush newValue)
        {
            //Brush oldValue = TitleBarForeground;
            TitleBarForeground = newValue;
            //OnPropertyChanged(new(TitleBarForegroundProperty, oldValue, newValue));
        }

        /// <summary>
        /// Gets or sets the TitleTemplate.
        /// </summary>
        [Category(Comum)]
        [Description("Obtem ou define um DataTemplate para a propriedade Title.")]
        public DataTemplate TitleTemplate { get => (DataTemplate)GetValue(TitleTemplateProperty); set => SetValue(TitleTemplateProperty, value); }

        /// <summary>
        /// Defines the TitleTemplateProperty.
        /// </summary>
        public static readonly DependencyProperty TitleTemplateProperty =
            DependencyProperty.Register(
                name: nameof(TitleTemplate),
                propertyType: typeof(DataTemplate),
                ownerType: typeof(CWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: null));

        /// <summary>
        /// Gets or sets the OverlayBackground.
        /// </summary>
        [Description("Obtem ou define um pincel que representa o plano de fundo da camada que cobre a janela.")]
        public Brush OverlayBackground { get => (Brush)GetValue(OverlayBackgroundProperty); set => SetValue(OverlayBackgroundProperty, value); }

        /// <summary>
        /// Defines the OverlayBackgroundProperty.
        /// </summary>
        public static readonly DependencyProperty OverlayBackgroundProperty =
            DependencyProperty.Register(
                name: nameof(OverlayBackground),
                propertyType: typeof(Brush),
                ownerType: typeof(CWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: Brushes.Gray));

        /// <summary>
        /// Gets or sets a value indicating whether ShowMessage.
        /// </summary>
        [Category(Aparência)]
        [Description("Obtem ou define a visibilidade da camada que cobre a janela.")]
        public bool ShowMessage { get => (bool)GetValue(ShowMessageProperty); set => SetValue(ShowMessageProperty, value); }

        /// <summary>
        /// Defines the ShowMessageProperty.
        /// </summary>
        public static readonly DependencyProperty ShowMessageProperty =
            DependencyProperty.Register(
                name: nameof(ShowMessage),
                propertyType: typeof(bool),
                ownerType: typeof(CWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: false));

        /// <summary>
        /// Gets or sets the Message.
        /// </summary>
        public FrameworkElement Message { get => (FrameworkElement)GetValue(MessageProperty); set => SetValue(MessageProperty, value); }

        /// <summary>
        /// Defines the MessageProperty.
        /// </summary>
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(
                name: nameof(Message),
                propertyType: typeof(FrameworkElement),
                ownerType: typeof(CWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: null));

        /// <summary>
        /// Gets or sets the MessageBackground.
        /// </summary>
        public Brush MessageBackground { get => (Brush)GetValue(MessageBackgroundProperty); set => SetValue(MessageBackgroundProperty, value); }

        /// <summary>
        /// Defines the MessageBackgroundProperty.
        /// </summary>
        public static readonly DependencyProperty MessageBackgroundProperty =
            DependencyProperty.Register(
                name: nameof(MessageBackground),
                propertyType: typeof(Brush),
                ownerType: typeof(CWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: Brushes.DarkBlue));

        /// <summary>
        /// Initializes a new instance of the <see cref="CWindow"/> class.
        /// </summary>
        public CWindow()
        {
            _ = CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseWindow));
            _ = CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, MaximizeRestoreWindow, CanResizeWindow));
            _ = CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, MaximizeRestoreWindow, CanResizeWindow));
            _ = CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, MinimizeWindow, CanMinimizeWindow));
             StateChanged += CWindow_StateChanged;
        }

        /// <summary>
        /// The CWindow_StateChanged.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void CWindow_StateChanged(object? sender, EventArgs e)
        {
            bool WindowStateIsNormal = WindowState == WindowState.Normal;
            MaximizeRestoreButton.Content = WindowStateIsNormal ? RestoreGlyph : MaximizeGlyph;
            MaximizeRestoreButton.ToolTip = WindowStateIsNormal ? MaximizeToolTip : RestoreToolTip;
            // Corrige o problema que ocorre quando a janela é maximizada
            Margin = WindowState == WindowState.Maximized ? MaximazedThickness : NormalThickness;
        }

        /// <summary>
        /// The CloseWindow.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="ExecutedRoutedEventArgs"/>.</param>
        private void CloseWindow(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// The CanMinimizeWindow.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="CanExecuteRoutedEventArgs"/>.</param>
        private void CanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode != ResizeMode.NoResize;
        }

        /// <summary>
        /// The CanResizeWindow.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="CanExecuteRoutedEventArgs"/>.</param>
        private void CanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode is ResizeMode.CanResize or ResizeMode.CanResizeWithGrip;
        }

        /// <summary>
        /// The MinimizeWindow.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="ExecutedRoutedEventArgs"/>.</param>
        private void MinimizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        /// <summary>
        /// The MaximizeRestoreWindow.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="ExecutedRoutedEventArgs"/>.</param>
        private void MaximizeRestoreWindow(object sender, ExecutedRoutedEventArgs e)
        {
            if ( sender is Window window && window != null )
            {
                window.WindowState = (window.WindowState == WindowState.Normal)
                    ? WindowState.Maximized
                    : WindowState.Normal;
            }
        }

        /// <summary>
        /// The OnApplyTemplate.
        /// </summary>
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

        /// <summary>
        /// Gets the MaximizeRestoreButton.
        /// </summary>
        internal Button MaximizeRestoreButton { get; private set; } = new();

        /// <summary>
        /// Gets the OutterBorder.
        /// </summary>
        internal Border OutterBorder { get; private set; } = new();

        /// <summary>
        /// Defines the NormalThickness.
        /// </summary>
        private Thickness NormalThickness = new(0);

        /// <summary>
        /// Defines the MaximazedThickness.
        /// </summary>
        private Thickness MaximazedThickness = new(7);

        /// <summary>
        /// Defines the MaximizeGlyph.
        /// </summary>
        private const string MaximizeGlyph = "\uE923";

        /// <summary>
        /// Defines the RestoreGlyph.
        /// </summary>
        private const string RestoreGlyph = "\uE922";

        /// <summary>
        /// Defines the MaximizeToolTip.
        /// </summary>
        private const string MaximizeToolTip = "Maximizar";

        /// <summary>
        /// Defines the RestoreToolTip.
        /// </summary>
        private const string RestoreToolTip = "Restaurar";

        /// <summary>
        /// Defines the Aparência.
        /// </summary>
        private const string Aparência = "Aparência";

        /// <summary>
        /// Defines the Comum.
        /// </summary>
        private const string Comum = "Comum";

        /// <summary>
        /// Defines the PART_Icon.
        /// </summary>
        private const string PART_Icon = "PART_Icon";

        /// <summary>
        /// Defines the PART_Title.
        /// </summary>
        private const string PART_Title = "PART_Title";

        /// <summary>
        /// Defines the PART_LeftArea.
        /// </summary>
        private const string PART_LeftArea = "PART_LeftArea";

        /// <summary>
        /// Defines the PART_RightArea.
        /// </summary>
        private const string PART_RightArea = "PART_RightArea";

        /// <summary>
        /// Defines the PART_MinimizeButton.
        /// </summary>
        private const string PART_MinimizeButton = "PART_MinimizeButton";

        /// <summary>
        /// Defines the PART_MaximizeRestoreButton.
        /// </summary>
        private const string PART_MaximizeRestoreButton = "PART_MaximizeRestoreButton";

        /// <summary>
        /// Defines the PART_CloseButton.
        /// </summary>
        private const string PART_CloseButton = "PART_CloseButton";
    }
}
