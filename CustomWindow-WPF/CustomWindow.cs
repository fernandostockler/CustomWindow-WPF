namespace CustomWindow_WPF
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shell;

    /// <summary>
    /// Defines the <see cref="CustomWindow" />.
    /// </summary>
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

        /// <summary>
        /// Defines the OriginalTitleBarHeight.
        /// </summary>
        private double OriginalTitleBarHeight = 42.0;

        /// <summary>
        /// Initializes static members of the <see cref="CustomWindow"/> class.
        /// </summary>
        static CustomWindow() => DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomWindow), new FrameworkPropertyMetadata(typeof(CustomWindow)));

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomWindow"/> class.
        /// </summary>
        public CustomWindow()
        {
            _ = CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseWindow));
            _ = CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, MaximizeRestoreWindow, CanResizeWindow));
            _ = CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, MaximizeRestoreWindow, CanResizeWindow));
            _ = CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, MinimizeWindow, CanMinimizeWindow));

            Loaded += CustomWindow_Loaded;
            StateChanged += CustomWindow_StateChanged;
        }

        /// <summary>
        /// The CustomWindow_Loaded.
        /// </summary>
        /// <param name="sender">The sender<see cref="Nullable{Object}"/>.</param>
        /// <param name="e">The e<see cref="RoutedEventArgs"/>.</param>
        private void CustomWindow_Loaded(object? sender, RoutedEventArgs e)
        {
            // don't change the order of the next 3 lines
            OriginalTitleBarHeight = TitleBarHeight;
            OnTitleBarHeightChanged(TitleBarHeight);
            OnKioskModeChanged(KioskMode);
        }

        /// <summary>
        /// The CustomWindow_StateChanged.
        /// </summary>
        /// <param name="sender">The sender<see cref="Nullable{Object}"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void CustomWindow_StateChanged(object? sender, EventArgs e)
        {
            bool WindowStateIsNormal = WindowState == WindowState.Normal;
            MaximizeRestoreButton.Content = WindowStateIsNormal ? RestoreGlyph : MaximizeGlyph;
            MaximizeRestoreButton.ToolTip = WindowStateIsNormal ? MaximizeToolTip : RestoreToolTip;
            Margin = WindowState == WindowState.Maximized ? MaximazedThickness : NormalThickness;
        }

        /// <summary>
        /// Identifies the <see cref="TitleBar"/> dependency property..
        /// </summary>
        public static readonly DependencyProperty TitleBarProperty = DependencyProperty.Register(
        name: nameof(TitleBar),
        propertyType: typeof(FrameworkElement),
        ownerType: typeof(CustomWindow),
        typeMetadata: new PropertyMetadata(defaultValue: null));

        /// <summary>
        /// Defines the TitleBarHeightProperty.
        /// </summary>
        public static readonly DependencyProperty TitleBarHeightProperty = DependencyProperty.Register(
        name: nameof(TitleBarHeight),
        propertyType: typeof(double),
        ownerType: typeof(CustomWindow),
        typeMetadata: new PropertyMetadata(defaultValue: 42.0,

        propertyChangedCallback: (d, e) =>
        {
            CustomWindow win = (CustomWindow)d;
            double newValue = (double)e.NewValue;
            win.OnTitleBarHeightChanged(newValue);
        },

        coerceValueCallback: (d, baseValue) =>
        {
            CustomWindow win = (CustomWindow)d;
            return (baseValue is double value && value < win.MinTitleBarHeight) ? win.MinTitleBarHeight : baseValue;
        }),

        validateValueCallback: (value) => value switch
        {
            double dvalue => !double.IsNaN(dvalue) &&
                !double.IsNegativeInfinity(dvalue) &&
                !double.IsPositiveInfinity(dvalue),
            _ => false
        });

        /// <summary>
        /// The OnTitleBarHeightChanged.
        /// </summary>
        /// <param name="newValue">The newValue<see cref="double"/>.</param>
        private void OnTitleBarHeightChanged(double newValue)
        {
            WindowChrome.SetWindowChrome(this, new WindowChrome()
            {
                CaptionHeight = newValue,
                CornerRadius = new CornerRadius(0),
                ResizeBorderThickness = new Thickness(6)
            });
        }

        /// <summary>
        /// Defines the TitleBarForegroundProperty.
        /// </summary>
        public static readonly DependencyProperty TitleBarForegroundProperty = DependencyProperty.Register(
        name: nameof(TitleBarForeground),
        propertyType: typeof(Brush),
        ownerType: typeof(CustomWindow),
        typeMetadata: new PropertyMetadata(
        defaultValue: Brushes.Black));

        /// <summary>
        /// Identifies the <see cref="TitleBarForegroundIsAutomated"/> dependency property..
        /// </summary>
        public static readonly DependencyProperty TitleBarForegroundIsAutomatedProperty = DependencyProperty.Register(
        name: nameof(TitleBarForegroundIsAutomated),
        propertyType: typeof(bool),
        ownerType: typeof(CustomWindow),
        typeMetadata: new PropertyMetadata(
        defaultValue: true));

        /// <summary>
        /// Defines the TitleBarBackgroundProperty.
        /// </summary>
        public static readonly DependencyProperty TitleBarBackgroundProperty = DependencyProperty.Register(
        name: nameof(TitleBarBackground),
        propertyType: typeof(Brush),
        ownerType: typeof(CustomWindow),
        typeMetadata: new PropertyMetadata(
        defaultValue: Brushes.White,
        propertyChangedCallback: (d, e) =>
        {
            CustomWindow win = (CustomWindow)d;
            Brush newValue = (Brush)e.NewValue;

            Brush? newIdealForeground = BackgroundToForegroundConverter.Instance
                .Convert(newValue, typeof(Brush), new object(), CultureInfo.CurrentCulture) as Brush;

            win.TitleBarForeground = win.TitleBarForegroundIsAutomated
                ? newIdealForeground ?? SystemColors.HotTrackBrush
                : win.Foreground;
        }));

        /// <summary>
        /// Defines the OverlayBackgroundProperty.
        /// </summary>
        public static readonly DependencyProperty OverlayBackgroundProperty = DependencyProperty.Register(
        name: nameof(OverlayBackground),
        propertyType: typeof(Brush),
        ownerType: typeof(CustomWindow),
        typeMetadata: new PropertyMetadata(
        defaultValue: Brushes.Gray));

        /// <summary>
        /// Identifies the <see cref="ShowCustomDialog"/> dependency property..
        /// </summary>
        public static readonly DependencyProperty ShowCustomDialogProperty = DependencyProperty.Register(
        name: nameof(ShowCustomDialog),
        propertyType: typeof(bool),
        ownerType: typeof(CustomWindow),
        typeMetadata: new PropertyMetadata(
        defaultValue: false));

        /// <summary>
        /// Identifies the <see cref="CustomDialog"/> dependency property..
        /// </summary>
        public static readonly DependencyProperty CustomDialogProperty = DependencyProperty.Register(
        name: nameof(CustomDialog),
        propertyType: typeof(FrameworkElement),
        ownerType: typeof(CustomWindow),
        typeMetadata: new PropertyMetadata(
        defaultValue: null));

        /// <summary>
        /// Identifies the <see cref="CustomDialogBackground"/> dependency property..
        /// </summary>
        public static readonly DependencyProperty CustomDialogBackgroundProperty = DependencyProperty.Register(
        name: nameof(CustomDialogBackground),
        propertyType: typeof(Brush),
        ownerType: typeof(CustomWindow),
        typeMetadata: new PropertyMetadata(
        defaultValue: Brushes.DarkBlue));

        /// <summary>
        /// Identifies the <see cref="MinTitleBarHeight"/> dependency property..
        /// </summary>
        public static readonly DependencyProperty MinTitleBarHeightProperty = DependencyProperty.Register(
        name: nameof(MinTitleBarHeight),
        propertyType: typeof(double),
        ownerType: typeof(CustomWindow),
        typeMetadata: new PropertyMetadata(
        defaultValue: 36.0));

        /// <summary>
        /// Identifies the <see cref="KioskMode"/> dependency property..
        /// </summary>
        public static readonly DependencyProperty KioskModeProperty = DependencyProperty.Register(
        name: nameof(KioskMode),
        propertyType: typeof(bool),
        ownerType: typeof(CustomWindow),
        typeMetadata: new FrameworkPropertyMetadata(
        defaultValue: false,
        propertyChangedCallback: (d, e) =>
        {
            bool newValue = (bool)e.NewValue;
            CustomWindow customWindow = (CustomWindow)d;
            customWindow.OnKioskModeChanged(newValue);
        }));

        /// <summary>
        /// The OnKioskModeChanged.
        /// </summary>
        /// <param name="newValue">The newValue<see cref="bool"/>.</param>
        private void OnKioskModeChanged(bool newValue)
        {
            if ( IsLoaded == false )
            {
                return;
            }

            if ( newValue )
            {
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Normal;
                WindowState = WindowState.Maximized;
                MinTitleBarHeight = 0.0;
                OriginalTitleBarHeight = TitleBarHeight;
                TitleBarHeight = 0.0;
            }
            else
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
                MinTitleBarHeight = 36.0;
                TitleBarHeight = OriginalTitleBarHeight;
            }
        }

        /// <summary>
        /// Identifies the <see cref="KioskModeExitKeyGesture"/> dependency property..
        /// </summary>
        public static readonly DependencyProperty KioskModeExitKeyGestureProperty = DependencyProperty.Register(
        name: nameof(KioskModeExitKeyGesture),
        propertyType: typeof(KioskExitKeyGesture),
        ownerType: typeof(CustomWindow),
        typeMetadata: new PropertyMetadata(
        defaultValue: new KioskExitKeyGesture(Key.End, new ModifierKeys[] { ModifierKeys.Shift, ModifierKeys.Alt })));

        /// <summary>
        /// The CloseWindow.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="ExecutedRoutedEventArgs"/>.</param>
        private void CloseWindow(object sender, ExecutedRoutedEventArgs e) => Close();

        /// <summary>
        /// The CanMinimizeWindow.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="CanExecuteRoutedEventArgs"/>.</param>
        private void CanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = ResizeMode != ResizeMode.NoResize;

        /// <summary>
        /// The CanResizeWindow.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="CanExecuteRoutedEventArgs"/>.</param>
        private void CanResizeWindow(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = ResizeMode is ResizeMode.CanResize or ResizeMode.CanResizeWithGrip;

        /// <summary>
        /// The MinimizeWindow.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="ExecutedRoutedEventArgs"/>.</param>
        private void MinimizeWindow(object sender, ExecutedRoutedEventArgs e) => SystemCommands.MinimizeWindow(this);

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
        /// Allows keyboard navigation.
        /// </summary>
        /// <param name="e">A <see cref="KeyEventArgs"/>.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            CheckKioskExitKeyGesture(e);
            CheckForbiddenKeys(e);

            void CheckForbiddenKeys(KeyEventArgs e)
            {
                // TODO: estudar como evitar
                if ( e.Key == Key.LWin || e.SystemKey == Key.LWin || e.Key == Key.RWin || e.SystemKey == Key.RWin )
                {
                    e.Handled = true;
                }
            }

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
        /// Defines the Comum.
        /// </summary>
        private const string Comum = "Comum";

        /// <summary>
        /// Defines the PART_TitleBar.
        /// </summary>
        private const string PART_TitleBar = "PART_TitleBar";

        /// <summary>
        /// Defines the PART_MaximizeRestoreButton.
        /// </summary>
        private const string PART_MaximizeRestoreButton = "PART_MaximizeRestoreButton";
    }
}