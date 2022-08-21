# CustomWindow
It is a window that allows customization of the non-client area, has a kiosk mode and has a mechanism for displaying modal content.

## Properties that affect the non-client area


Propriedade | Descrição  
---- | ----
TitleBar | Gets or sets a FrameworkElement value that represents a non-client title bar area except the buttons area.
TitleBarHeight | Gets or sets the height of the title bar.
TitleBarForeground | Gets or sets a brush that describes the foreground color of the window's title bar. Automatically calculated by OnTitleBarBackgroundChanged(d, e) when TitleBarForegroundIsAutomated is true.
TitleBarForegroundIsAutomated | Gets or sets a Boolean value representing whether or not the title bar foreground will automatically adapt to a new background.
TitleBarBackground | Gets or sets the brush that describes the background of the window's title bar.
TitleBarBorderThickness | Gets or sets the thickness of the border of the window's title bar.
TitleBarBorderBrush | Gets or sets the brush that describes the border of the window's title bar.

![CustomWindow v1 Customizable area in orange](https://user-images.githubusercontent.com/10555640/185812948-96980182-baa6-4441-975b-938b6fdb4abe.png)
