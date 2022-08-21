# [CustomWindow](Docs/CustomWindow.md)
It is a window that allows customization of the non-client area, has a kiosk mode and has a mechanism for displaying modal content.

<table>
  <tr>
    <td>
      <img src="https://user-images.githubusercontent.com/10555640/185812976-08963675-f609-47cf-8bcc-88d998a408a0.png" style="width:100%;height: auto;" />
    </td>
    <td>
        <img src="https://user-images.githubusercontent.com/10555640/185813189-c8bc7b2e-d549-4ce9-919d-a0268268e814.png" style="width:100%;height: auto;" />
    </td>
  </tr>

  <tr>
    <td>
        <img src="https://user-images.githubusercontent.com/10555640/185813128-abed47b5-8fef-4d38-93ed-27397d2f4746.png" style="width:100%;height: auto;" />
    </td>
    <td>
        <img src="https://user-images.githubusercontent.com/10555640/185813130-5914cc9b-1a2c-4d2a-ab05-0afbdc5102e1.png" style="width:100%;height: auto;" />    
    </td>
  </tr>
<table>
  
![CustomWindow_Big_Violet_Dialog_Blue](https://user-images.githubusercontent.com/10555640/185813481-1277314d-23d6-487b-a6b1-c94c515fec9d.png)  

## Markup code
![CustomWindow v2 markup code](https://user-images.githubusercontent.com/10555640/185813349-ca8288a8-f882-49c2-9526-a5b91645b2c8.png)

## Code behind
```C#
public partial class CustomWindowSample : CustomWindow
{
    public CustomWindowSample()
    {
        InitializeComponent();
    }

    private void LikeButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            button.Content = button.Content.ToString() == "\uE00B"
                ? "\uE006"
                : "\uE00B";
        }
    }
}
```
