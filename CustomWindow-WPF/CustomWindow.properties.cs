namespace CustomWindow_WPF
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Defines the <see cref="CustomWindow" />.
    /// </summary>
    public partial class CustomWindow
    {
        /// <summary>
        /// Shadows the WindowStyle property to prevent it from being changed from WindowStyle.None ..
        /// </summary>
        public new WindowStyle WindowStyle 
        {
            get => (WindowStyle)GetValue(WindowStyleProperty);
            internal set => SetValue(WindowStyleProperty, value);
        }

        /// <summary>
        /// Shadows the AllowsTransparency property to prevent it from being changed from AllowTransparency = True..
        /// </summary>
        public new bool AllowsTransparency
        {
            get => (bool)GetValue(AllowsTransparencyProperty);
            internal set => SetValue(AllowsTransparencyProperty, value);
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
        /// Gets or sets the TitleBarHeight.
        /// </summary>
        [Category(Comum)]
        [Description("Obtem ou define a altura da barra de título (parte não cliente).")]
        public double TitleBarHeight
        {
            get => (double)GetValue(TitleBarHeightProperty);
            set => SetValue(TitleBarHeightProperty, value);
        }

        /// <summary>
        /// Gets or sets the TitleBarForeground.
        /// </summary>
        [Description("Obtem ou define um pincel que descreve a cor do primeiro plano da barra de título da janela.")]
        public Brush TitleBarForeground
        {
            get => (Brush)GetValue(TitleBarForegroundProperty);
            set => SetValue(TitleBarForegroundProperty, value);
        }

        /// <summary>
        /// Gets or sets a Boolean value representing whether or not the title bar foreground will automatically adapt to a new background...
        /// </summary>
        [Category(Comum)]
        [Description("Gets or sets a Boolean value representing whether or not the title bar foreground will automatically adapt to a new background.")]
        public bool TitleBarForegroundIsAutomated
        {
            get => (bool)GetValue(TitleBarForegroundIsAutomatedProperty);
            set => SetValue(TitleBarForegroundIsAutomatedProperty, value);
        }

        /// <summary>
        /// Gets or sets the TitleBarBackground.
        /// </summary>
        [Description("Obtem ou define um pincel que descreve o plano de fundo da barra do título da janela.")]
        public Brush TitleBarBackground
        {
            get => (Brush)GetValue(TitleBarBackgroundProperty);
            set => SetValue(TitleBarBackgroundProperty, value);
        }

        /// <summary>
        /// Gets or sets the OverlayBackground.
        /// </summary>
        [Description("Obtem ou define um pincel que representa o plano de fundo da camada que cobre a janela.")]
        public Brush OverlayBackground
        {
            get => (Brush)GetValue(OverlayBackgroundProperty);
            set => SetValue(OverlayBackgroundProperty, value);
        }

        /// <summary>
        /// Gets or sets the visibility of the layer that covers the window...
        /// </summary>
        [Category(Comum)]
        [Description("Gets or sets the visibility of the layer that covers the window.")]
        public bool ShowCustomDialog
        {
            get => (bool)GetValue(ShowCustomDialogProperty);
            set => SetValue(ShowCustomDialogProperty, value);
        }

        /// <summary>
        /// Gets or sets a FrameworkElement that represents an interactive modal control that will only be visible if the ShowCustomDialog property is true...
        /// </summary>
        [Category(Comum)]
        [Description("Gets or sets a FrameworkElement that represents an interactive modal control that will only be visible if the ShowCustomDialog property is true.")]
        public FrameworkElement CustomDialog
        {
            get => (FrameworkElement)GetValue(CustomDialogProperty);
            set => SetValue(CustomDialogProperty, value);
        }

        /// <summary>
        /// Gets or sets a brush representing the background of the CustomDialog element...
        /// </summary>
        [Description("Gets or sets a brush representing the background of the CustomDialog element.")]
        public Brush CustomDialogBackground
        {
            get => (Brush)GetValue(CustomDialogBackgroundProperty);
            set => SetValue(CustomDialogBackgroundProperty, value);
        }

        /// <summary>
        /// Gets or sets a double value representing the minimum title bar's height...
        /// </summary>
        [Category(Comum)]
        [Description("Gets or sets a double value representing the minimum title bar's height.")]
        public double MinTitleBarHeight
        {
            get => (double)GetValue(MinTitleBarHeightProperty);
            set => SetValue(MinTitleBarHeightProperty, value);
        }

        /// <summary>
        /// Gets or sets a Boolean value representing whether KioskMode is turned on/off...
        /// </summary>
        [Category(Comum)]
        [Description("Gets or sets a Boolean value representing whether KioskMode is turned on/off.")]
        public bool KioskMode
        {
            get => (bool)GetValue(KioskModeProperty);
            set => SetValue(KioskModeProperty, value);
        }

        /// <summary>
        /// Gets or sets a key combination that turns off kiosk mode...
        /// </summary>
        [Category(Comum)]
        [Description("Gets or sets a key combination that turns off kiosk mode.")]
        public KioskExitKeyGesture KioskModeExitKeyGesture
        {
            get => (KioskExitKeyGesture)GetValue(KioskModeExitKeyGestureProperty);
            set => SetValue(KioskModeExitKeyGestureProperty, value);
        }
    }
}