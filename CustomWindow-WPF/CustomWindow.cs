namespace CustomWindow_WPF
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Runtime.Serialization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shell;

    /// <summary>
    /// CustomWindow é uma janela que permite a personalização da área não-cliente, possuí um modo kiosk e tem um mecanismo para exibição de conteúdo modal.
    /// </summary>
    /// <remarks>
    /// A barra de título foi dividida em quatro regiões: Icone, entre o icone e o título e depois do título e antes dos botões da janela.
    /// Para interagir com estes elementos na área não-cliente acrescente o atributo <c>WindowChrome.IsHitTestVisibleInChrome="True"</c>.
    /// </remarks>
    [TemplatePart(Name = PART_Icon, Type = typeof(ContentControl))]
    [TemplatePart(Name = PART_LeftArea, Type = typeof(ContentControl))]
    [TemplatePart(Name = PART_Title, Type = typeof(ContentControl))]
    [TemplatePart(Name = PART_RightArea, Type = typeof(ContentControl))]
    [TemplatePart(Name = PART_MinimizeButton, Type = typeof(ButtonBase))]
    [TemplatePart(Name = PART_MaximizeRestoreButton, Type = typeof(ButtonBase))]
    [TemplatePart(Name = PART_CloseButton, Type = typeof(ButtonBase))]
    public partial class CustomWindow : Window
    {
        static CustomWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomWindow), new FrameworkPropertyMetadata(typeof(CustomWindow)));
        }

        /// <summary>
        /// Public constructor
        /// </summary>
        public CustomWindow()
        {
            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;
            StateChanged += CustomWindow_StateChanged;
            Loaded += CustomWindow_Loaded;
            _ = CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseWindow));
            _ = CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, MaximizeRestoreWindow, CanResizeWindow));
            _ = CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, MaximizeRestoreWindow, CanResizeWindow));
            _ = CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, MinimizeWindow, CanMinimizeWindow));
        }

        private void CustomWindow_Loaded(object sender, RoutedEventArgs e)
        {
            OriginalTitleBarHeight = TitleBarHeight;

            // Do not change the order of lines of code below.
            // OnKioskChanged() affects the value of the TitleBarHeight property.
            OnTitleBarHeightChanged(TitleBarHeight);

            OnKioskModeChanged(KioskMode);

            CustomDialog ??= GetDefaultCustomDialog();

            StackPanel GetDefaultCustomDialog()
            {
                StackPanel stackPanel = new()
                {
                    Orientation = Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(20)
                };
                TextBlock textBlock = new()
                {
                    Text = "Hello!",
                    FontSize = 30,
                    Foreground = Brushes.Violet,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                TextBlock textBlock1 = new()
                {
                    Text = "This is just an example of a CustomDialog. " +
                           "Use the CustomDialog property to define a custom CustomDialog with any FrameworkElement. " +
                           "To display it just set the ShowCustomDialog property to True. " +
                           "If no CustomDialog is set, this message will be showed.",
                    FontSize = 14,
                    Margin = new Thickness(50, 20, 50, 20),
                    Foreground = Brushes.Silver,
                    TextWrapping = TextWrapping.Wrap,
                    TextTrimming = TextTrimming.WordEllipsis,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                Button button = new()
                {
                    Content = "Enter",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = Brushes.Silver,
                    Background = Brushes.Transparent,
                    Padding = new Thickness(10, 5, 10, 5),
                    Margin = new Thickness(0, 10, 0, 0)
                };
                button.Click += CustomDialogEnterButton_Click;
                ;
                _ = stackPanel.Children.Add(textBlock);
                _ = stackPanel.Children.Add(textBlock1);
                _ = stackPanel.Children.Add(button);
                return stackPanel;
            }
        }

        /// <summary>
        /// Sets CustomDialog visibility to Visibility.Collapsed
        /// </summary>
        /// <remarks>
        /// This event handler is only used thru GetDefaultCustomDialog() function.
        /// </remarks>
        private void CustomDialogEnterButton_Click(object sender, RoutedEventArgs e) => ShowCustomDialog = false;

        /// <summary>
        /// Shadows the WindowStyle property to prevent it from being changed from WindowStyle.None . </summary>
        /// <remarks>
        /// Any attempt to modify this property will launch an exception: </remarks>
        /// <exception>
        /// cref="Error MC3080  The property 'CustomWindow.WindowStyle' cannot be set because it does not have an accessible set accessor."
        /// </exception>
        public new WindowStyle WindowStyle
        {
            get => (WindowStyle)GetValue(WindowStyleProperty);
            private set => SetValue(WindowStyleProperty, value);
        }

        /// <summary>
        /// Shadows the AllowsTransparency property to prevent it from being changed from AllowTransparency = True.
        /// </summary>
        /// <remarks>
        /// Any attempt to modify this property will launch an exception:
        /// </remarks>
        /// <exception>
        /// cref="Error MC3080  The property 'CustomWindow.AllowsTransparency' cannot be set because it does not have an accessible set accessor."
        /// </exception>
        public new bool AllowsTransparency
        {
            get => (bool)GetValue(AllowsTransparencyProperty);
            private set => SetValue(AllowsTransparencyProperty, value);
        }

        /// <summary>
        /// Gets or sets a Boolean value representing whether KioskMode is turned on/off.
        /// </summary>
        [Category(ClassName)]
        [Description("Gets or sets a Boolean value representing whether KioskMode is turned on/off.")]
        public bool KioskMode
        {
            get => (bool)GetValue(KioskModeProperty);
            set => SetValue(KioskModeProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="KioskMode"/> dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the <see cref="KioskMode"/> dependency property.
        /// </returns>
        public static readonly DependencyProperty KioskModeProperty =
            DependencyProperty.Register(
                name: nameof(KioskMode),
                propertyType: typeof(bool),
                ownerType: typeof(CustomWindow),
                typeMetadata: new FrameworkPropertyMetadata(false,
                    propertyChangedCallback: (d, e) =>
                    {
                        bool newValue = (bool)e.NewValue;
                        CustomWindow customWindow = (CustomWindow)d;
                        customWindow.OnKioskModeChanged(newValue);
                    }));

        private double OriginalTitleBarHeight = 42.0;

        private void OnKioskModeChanged(bool newValue)
        {
            if ( IsLoaded == false )
            {
                return;
            }

            if ( newValue )
            {
                WindowState = WindowState.Normal;
                WindowState = WindowState.Maximized;
                MinTitleBarHeight = 0.0;
                OriginalTitleBarHeight = TitleBarHeight;
                TitleBarHeight = 0.0;
            }
            else
            {
                MinTitleBarHeight = 36.0;
                TitleBarHeight = OriginalTitleBarHeight;
            }
        }

        /// <summary>
        /// Gets or sets a key combination that turns off kiosk mode.
        /// </summary>
        [Category(ClassName)]
        [Description("Gets or sets a key combination that turns off kiosk mode.")]
        public KioskExitKeyGesture KioskModeExitKeyGesture
        {
            get => (KioskExitKeyGesture)GetValue(KioskModeExitKeyGestureProperty);
            set => SetValue(KioskModeExitKeyGestureProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="KioskModeExitKeyGesture"/> dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the <see cref="KioskModeExitKeyGesture"/> dependency property.
        /// </returns>
        public static readonly DependencyProperty KioskModeExitKeyGestureProperty =
            DependencyProperty.Register(
                name: nameof(KioskModeExitKeyGesture),
                propertyType: typeof(KioskExitKeyGesture),
                ownerType: typeof(CustomWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: new KioskExitKeyGesture(Key.End, new ModifierKeys[] { ModifierKeys.Shift, ModifierKeys.Alt })));

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

        /// <summary>
        /// The default value for ResizeBorderThickness dependency property.
        /// </summary>
        protected const int ResizeBorderThicknessDefaultValue = 6;

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
        /// Gets or sets the FrameworkElement located to the left of the title bar.
        /// </summary>
        [Category(ClassName)]
        [Description("Gets or sets the FrameworkElement located to the left of the title bar.")]
        public FrameworkElement IconArea
        {
            get => (FrameworkElement)GetValue(IconAreaProperty);
            set => SetValue(IconAreaProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IconArea"/> dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the <see cref="IconArea"/> dependency property.
        /// </returns>
        public static readonly DependencyProperty IconAreaProperty =
            DependencyProperty.Register(
                name: nameof(IconArea),
                propertyType: typeof(FrameworkElement),
                ownerType: typeof(CustomWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: null));

        /// <summary>
        /// Gets or sets the FrameworkElement located in the left part of the title bar, just after the IconArea.
        /// </summary>
        [Category(ClassName)]
        [Description("Gets or sets the FrameworkElement located in the left part of the title bar, just after the IconArea.")]
        public FrameworkElement TitleBarLeftArea
        {
            get => (FrameworkElement)GetValue(TitleBarLeftAreaProperty);
            set => SetValue(TitleBarLeftAreaProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="TitleBarLeftArea"/> dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the <see cref="TitleBarLeftArea"/> dependency property.
        /// </returns>
        public static readonly DependencyProperty TitleBarLeftAreaProperty =
            DependencyProperty.Register(
                name: nameof(TitleBarLeftArea),
                propertyType: typeof(FrameworkElement),
                ownerType: typeof(CustomWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: null));

        /// <summary>
        /// Gets or sets the FrameworkElement located in the right part of the title bar, before the window buttons.
        /// </summary>
        [Category(ClassName)]
        [Description("Gets or sets the FrameworkElement located in the right part of the title bar, before the window buttons.")]
        public FrameworkElement TitleBarRightArea
        {
            get => (FrameworkElement)GetValue(TitleBarRightAreaProperty);
            set => SetValue(TitleBarRightAreaProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="TitleBarRightArea"/> dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the <see cref="TitleBarRightArea"/> dependency property.
        /// </returns>
        public static readonly DependencyProperty TitleBarRightAreaProperty =
            DependencyProperty.Register(
                name: nameof(TitleBarRightArea),
                propertyType: typeof(FrameworkElement),
                ownerType: typeof(CustomWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: null));

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

        /// <summary>
        /// Gets or sets the thickness of the border of the window's title bar.
        /// </summary>
        [Category(ClassName)]
        [Description("Gets or sets the thickness of the border of the window's title bar.")]
        public Thickness TitleBarBorderThickness
        {
            get => (Thickness)GetValue(TitleBarBorderThicknessProperty);
            set => SetValue(TitleBarBorderThicknessProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="TitleBarBorderThickness"/> dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the <see cref="TitleBarBorderThickness"/> dependency property.
        /// </returns>
        public static readonly DependencyProperty TitleBarBorderThicknessProperty =
            DependencyProperty.Register(
                name: nameof(TitleBarBorderThickness),
                propertyType: typeof(Thickness),
                ownerType: typeof(CustomWindow),
                typeMetadata: new PropertyMetadata(new Thickness(0, 0, 0, 1)));

        /// <summary>
        /// Gets or sets the brush that describes the border of the window's title bar.
        /// </summary>
        [Category(ClassName)]
        [Description("Gets or sets the brush that describes the border of the window's title bar.")]
        public Brush TitleBarBorderBrush
        {
            get => (Brush)GetValue(TitleBarBorderBrushProperty);
            set => SetValue(TitleBarBorderBrushProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="TitleBarBorderBrush"/> dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the <see cref="TitleBarBorderBrush"/> dependency property.
        /// </returns>
        public static readonly DependencyProperty TitleBarBorderBrushProperty =
            DependencyProperty.Register(
                name: nameof(TitleBarBorderBrush),
                propertyType: typeof(Brush),
                ownerType: typeof(CustomWindow),
                typeMetadata: new PropertyMetadata(Brushes.Black));

        /// <summary>
        /// Gets or sets the DataTemplate of the Title property.
        /// </summary>
        [Category(ClassName)]
        [Description("Gets or sets the DataTemplate of the Title property.")]
        public DataTemplate TitleTemplate
        {
            get => (DataTemplate)GetValue(TitleTemplateProperty);
            set => SetValue(TitleTemplateProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="TitleTemplate"/> dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the <see cref="TitleTemplate"/> dependency property.
        /// </returns>
        public static readonly DependencyProperty TitleTemplateProperty =
            DependencyProperty.Register(
                name: nameof(TitleTemplate),
                propertyType: typeof(DataTemplate),
                ownerType: typeof(CustomWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: null));

        /// <summary>
        /// Gets or sets a brush that represents the background of the layer covering the window.
        /// </summary>
        [Category(ClassName)]
        [Description("Gets or sets a brush that represents the background of the layer covering the window.")]
        public Brush OverlayBackground
        {
            get => (Brush)GetValue(OverlayBackgroundProperty);
            set => SetValue(OverlayBackgroundProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="TitleTemplate"/> dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the <see cref="TitleTemplate"/> dependency property.
        /// </returns>
        public static readonly DependencyProperty OverlayBackgroundProperty =
            DependencyProperty.Register(
                name: nameof(OverlayBackground),
                propertyType: typeof(Brush),
                ownerType: typeof(CustomWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: Brushes.Gray));

        /// <summary>
        /// Gets or sets the visibility of the layer that covers the window.
        /// </summary>
        [Category(ClassName)]
        [Description("Gets or sets the visibility of the layer that covers the window.")]
        public bool ShowCustomDialog
        {
            get => (bool)GetValue(ShowCustomDialogProperty);
            set => SetValue(ShowCustomDialogProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ShowCustomDialog"/> dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the <see cref="ShowCustomDialog"/> dependency property.
        /// </returns>
        public static readonly DependencyProperty ShowCustomDialogProperty =
            DependencyProperty.Register(
                name: nameof(ShowCustomDialog),
                propertyType: typeof(bool),
                ownerType: typeof(CustomWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: false));

        /// <summary>
        /// Gets or sets a FrameworkElement that represents an interactive modal control that will only be visible if the ShowCustomDialog property is true.
        /// </summary>
        [Category(ClassName)]
        [Description("Gets or sets a FrameworkElement that represents an interactive modal control that will only be visible if the ShowCustomDialog property is true.")]
        public FrameworkElement CustomDialog
        {
            get => (FrameworkElement)GetValue(CustomDialogProperty);
            set => SetValue(CustomDialogProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="CustomDialog"/> dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the <see cref="CustomDialog"/> dependency property.
        /// </returns>
        public static readonly DependencyProperty CustomDialogProperty =
            DependencyProperty.Register(
                name: nameof(CustomDialog),
                propertyType: typeof(FrameworkElement),
                ownerType: typeof(CustomWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: null));

        /// <summary>
        /// Gets or sets a brush representing the background of the CustomDialog element.
        /// </summary>
        [Category(ClassName)]
        [Description("Gets or sets a brush representing the background of the CustomDialog element.")]
        public Brush CustomDialogBackground
        {
            get => (Brush)GetValue(CustomDialogBackgroundProperty);
            set => SetValue(CustomDialogBackgroundProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="CustomDialogBackground"/> dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the <see cref="CustomDialogBackground"/> dependency property.
        /// </returns>
        public static readonly DependencyProperty CustomDialogBackgroundProperty =
            DependencyProperty.Register(
                name: nameof(CustomDialogBackground),
                propertyType: typeof(Brush),
                ownerType: typeof(CustomWindow),
                typeMetadata: new PropertyMetadata(
                    defaultValue: Brushes.DarkBlue));

        private static readonly Thickness NormalThickness = new(0);

        private static double WindowsTaskbarHeight
            => SystemParameters.PrimaryScreenHeight
            - SystemParameters.FullPrimaryScreenHeight
            - SystemParameters.WindowCaptionHeight;

        private static double WindowsTaskbarWidth
            => SystemParameters.PrimaryScreenWidth
            - SystemParameters.FullPrimaryScreenWidth;

        private void CustomWindow_StateChanged(object? sender, EventArgs e)
        {
            bool WindowStateIsNormal = WindowState == WindowState.Normal;

            MaximizeRestoreButton.Content = WindowStateIsNormal ? RestoreGlyph : MaximizeGlyph;
            MaximizeRestoreButton.ToolTip = WindowStateIsNormal ? MaximizeToolTip : RestoreToolTip;

            // Fixes the issue that occurs when the window is maximized.
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

        private enum TaskbarPosition
        {
            None,
            Top,
            Left,
            Bottom,
            Right
        }

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

        /// <summary>
        /// Allows keyboard navigation.
        /// </summary>
        /// <param name="e">A <see cref="KeyEventArgs"/>.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            CheckKioskExitKeyGesture(e);

            void CheckKioskExitKeyGesture(KeyEventArgs e)
            {
                KioskExitKeyGesture k = KioskModeExitKeyGesture;
                bool match = KioskModeExitKeyGesture.ModifierKeys.Length switch
                {
                    1 => k.ModifierKeys[0] == (e.KeyboardDevice.Modifiers & k.ModifierKeys[0]),

                    2 => k.ModifierKeys[0] == (e.KeyboardDevice.Modifiers & k.ModifierKeys[0]) &&
                         k.ModifierKeys[1] == (e.KeyboardDevice.Modifiers & k.ModifierKeys[1]),

                    3 => k.ModifierKeys[0] == (e.KeyboardDevice.Modifiers & k.ModifierKeys[0]) &&
                         k.ModifierKeys[1] == (e.KeyboardDevice.Modifiers & k.ModifierKeys[1]) &&
                         k.ModifierKeys[2] == (e.KeyboardDevice.Modifiers & k.ModifierKeys[2]),

                    //  Unselect the 3 lines below if you need to alert that only 3 first items will be processed.
                    //> 3 => throw new ArrayExceedsMaximumLengthException(
                    //       arrayName: "KioskExitKeyGesture.ModifierKeys[ ]",
                    //       message: $"There are {k.ModifierKeys.Length} items in KioskModeExitKeyGesture.ModifierKeys[ ] array. It can hold only 3 items."),

                    _ => false,
                };
                if ( match && (e.Key == KioskModeExitKeyGesture.Key || e.SystemKey == KioskModeExitKeyGesture.Key) )
                {
                    e.Handled = true;
                    KioskMode = false;
                }
            }
        }

        /// <summary>
        /// Builds the visual tree for the control when a new template is applied.
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

        internal Button MaximizeRestoreButton { get; private set; } = new();
        internal Border OutterBorder { get; private set; } = new();

        private const string MaximizeGlyph = "\uE923";
        private const string RestoreGlyph = "\uE922";
        private const string MaximizeToolTip = "Maximizar";
        private const string RestoreToolTip = "Restaurar";
        private const string ClassName = nameof(CustomWindow);

        private const string PART_Icon = "PART_Icon";
        private const string PART_Title = "PART_Title";
        private const string PART_LeftArea = "PART_LeftArea";
        private const string PART_RightArea = "PART_RightArea";
        private const string PART_MinimizeButton = "PART_MinimizeButton";
        private const string PART_MaximizeRestoreButton = "PART_MaximizeRestoreButton";
        private const string PART_CloseButton = "PART_CloseButton";

        [Serializable]
        private class ArrayExceedsMaximumLengthException : Exception
        {
            public string? ArrayName { get; private set; } = string.Empty;
            public ArrayExceedsMaximumLengthException(string? arrayName, string? message) : base(message)
            {
                ArrayName = arrayName;
            }
            protected ArrayExceedsMaximumLengthException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        }
    }

}
