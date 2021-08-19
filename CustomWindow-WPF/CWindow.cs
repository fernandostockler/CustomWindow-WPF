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

    [TemplatePart(Name = PART_Icon, Type = typeof(ContentControl))]
    [TemplatePart(Name = PART_LeftArea, Type = typeof(ContentControl))]
    [TemplatePart(Name = PART_Title, Type = typeof(ContentControl))]
    [TemplatePart(Name = PART_RightArea, Type = typeof(ContentControl))]
    [TemplatePart(Name = PART_MinimizeButton, Type = typeof(ButtonBase))]
    [TemplatePart(Name = PART_MaximizeRestoreButton, Type = typeof(ButtonBase))]
    [TemplatePart(Name = PART_CloseButton, Type = typeof(ButtonBase))]
    public partial class CWindow : Window
    {
        private const string ClassName = nameof(CWindow);

        static CWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CWindow), new FrameworkPropertyMetadata(typeof(CWindow)));
        }

        public DataTemplate IconTemplate
        {
            get => (DataTemplate)GetValue(IconTemplateProperty);
            set => SetValue(IconTemplateProperty, value);
        }
        public static readonly DependencyProperty IconTemplateProperty =
            DependencyProperty.Register(
                name: nameof(IconTemplate),
                propertyType: typeof(DataTemplate),
                ownerType: typeof(CWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: null));

        [Category(Comum)]
        public FrameworkElement IconArea
        {
            get => (FrameworkElement)GetValue(IconAreaProperty);
            set => SetValue(IconAreaProperty, value);
        }
        public static readonly DependencyProperty IconAreaProperty =
            DependencyProperty.Register(
                name: nameof(IconArea),
                propertyType: typeof(FrameworkElement),
                ownerType: typeof(CWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: null));

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
                ownerType: typeof(CWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: 42.0,
                    propertyChangedCallback: OnTitleBarHeightChanged,
                    coerceValueCallback: OnCoerceTitleBarHeight),
                    validateValueCallback: OnValidateTitleBarHeight);

        private static object OnCoerceTitleBarHeight(DependencyObject d, object baseValue)
        {
            return (baseValue is double value && value < 36.0) ? 36.0 : baseValue;
        }

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

        [Category(Comum)]
        [Description("Obtem ou define um FrameworkElement personalizado localizado na parte esquerda da barra de título, logo após o ícone da janela.")]
        public FrameworkElement TitleBarLeftArea
        {
            get => (FrameworkElement)GetValue(TitleBarLeftAreaProperty);
            set => SetValue(TitleBarLeftAreaProperty, value);
        }
        public static readonly DependencyProperty TitleBarLeftAreaProperty =
            DependencyProperty.Register(
                name: nameof(TitleBarLeftArea),
                propertyType: typeof(FrameworkElement),
                ownerType: typeof(CWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: null));

        [Category(Comum)]
        [Description("Obtem ou define um FrameworkElement personalizado localizado na parte direita da barra de título, antes dos botões da janela.")]
        public FrameworkElement TitleBarRightArea
        {
            get => (FrameworkElement)GetValue(TitleBarRightAreaProperty);
            set => SetValue(TitleBarRightAreaProperty, value);
        }
        public static readonly DependencyProperty TitleBarRightAreaProperty =
            DependencyProperty.Register(
                name: nameof(TitleBarRightArea),
                propertyType: typeof(FrameworkElement),
                ownerType: typeof(CWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: null));

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
                ownerType: typeof(CWindow),
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
                ownerType: typeof(CWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: Brushes.White,
                    propertyChangedCallback: OnTitleBarBackgroundChanged));

        private static void OnTitleBarBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CWindow win = (CWindow)d;
            Brush newValue = (Brush)e.NewValue;
            BackgroundToForegroundConverter converter = BackgroundToForegroundConverter.Instance;
            Brush newIdealForeground = converter.Convert(newValue, typeof(Brush), null, CultureInfo.CurrentCulture) as Brush;
            win.OnTitleBarBackgroundChanged(newIdealForeground);
        }

        private void OnTitleBarBackgroundChanged(Brush newValue)
        {
            Brush oldValue = TitleBarForeground;
            TitleBarForeground = newValue;
            OnPropertyChanged(new(TitleBarForegroundProperty, oldValue, newValue));
        }

        [Category(Comum)]
        [Description("Obtem ou define um DataTemplate para a propriedade Title.")]
        public DataTemplate TitleTemplate
        {
            get => (DataTemplate)GetValue(TitleTemplateProperty);
            set => SetValue(TitleTemplateProperty, value);
        }
        public static readonly DependencyProperty TitleTemplateProperty =
            DependencyProperty.Register(
                name: nameof(TitleTemplate),
                propertyType: typeof(DataTemplate),
                ownerType: typeof(CWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: null));

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
                ownerType: typeof(CWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: Brushes.Gray));

        [Category(Aparência)]
        [Description("Obtem ou define a visibilidade da camada que cobre a janela.")]
        public bool ShowMessage
        {
            get => (bool)GetValue(ShowMessageProperty);
            set => SetValue(ShowMessageProperty, value);
        }
        public static readonly DependencyProperty ShowMessageProperty =
            DependencyProperty.Register(
                name: nameof(ShowMessage),
                propertyType: typeof(bool),
                ownerType: typeof(CWindow),
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
                ownerType: typeof(CWindow),
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
                ownerType: typeof(CWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: Brushes.DarkBlue));


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
            ownerType: typeof(CWindow),
            typeMetadata: new PropertyMetadata(defaultValue: new CornerRadius(5)));

        public CWindow()
        {
            _ = CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseWindow));
            _ = CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, MaximizeRestoreWindow, CanResizeWindow));
            _ = CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, MaximizeRestoreWindow, CanResizeWindow));
            _ = CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, MinimizeWindow, CanMinimizeWindow));
            StateChanged += CWindow_StateChanged;
            CornerRadius temp = CornerRadius;
            CornerRadius = new CornerRadius(0);
            CornerRadius = temp;
        }

        private void CWindow_StateChanged(object sender, EventArgs e)
        {
            bool WindowStateIsNormal = (WindowState == WindowState.Normal);
            MaximizeRestoreButton.Content = WindowStateIsNormal ? RestoreGlyph : MaximizeGlyph;
            MaximizeRestoreButton.ToolTip = WindowStateIsNormal ? MaximizeToolTip : RestoreToolTip;
            // Corrige o problema que ocorre quando a janela é maximizada
            Margin = WindowState == WindowState.Maximized ? MaximazedThickness : NormalThickness;
        }

        private void CloseWindow(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void CanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode != ResizeMode.NoResize;
        }

        private void CanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode is ResizeMode.CanResize or ResizeMode.CanResizeWithGrip;
        }

        private void MinimizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

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
            MaximizeRestoreButton = GetRequiredTemplateChild<Button>(PART_MaximizeRestoreButton);
        }

        public T GetRequiredTemplateChild<T>(string childName) where T : DependencyObject
        {
            return (T)GetTemplateChild(childName);
        }

        internal Button MaximizeRestoreButton { get; private set; }
        internal Border OutterBorder { get; private set; }

        private Thickness NormalThickness = new(0);
        private Thickness MaximazedThickness = new(7);

        private const string MaximizeGlyph = "\uE923";
        private const string RestoreGlyph = "\uE922";
        private const string MaximizeToolTip = "Maximizar";
        private const string RestoreToolTip = "Restaurar";
        private const string Aparência = "Aparência";
        private const string Comum = "Comum";
        private const string PART_Icon = "PART_Icon";
        private const string PART_Title = "PART_Title";
        private const string PART_LeftArea = "PART_LeftArea";
        private const string PART_RightArea = "PART_RightArea";
        private const string PART_MinimizeButton = "PART_MinimizeButton";
        private const string PART_MaximizeRestoreButton = "PART_MaximizeRestoreButton";
        private const string PART_CloseButton = "PART_CloseButton";
    }

}
