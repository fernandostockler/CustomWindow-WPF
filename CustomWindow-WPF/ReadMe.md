# [CustomWindow](Docs/CustomWindow.md)
It is a window that allows customization of the non-client area, has a kiosk mode and has a mechanism for displaying modal content.

<table>
  <tr>
    <td>
      <img src="https://user-images.githubusercontent.com/10555640/185812976-08963675-f609-47cf-8bcc-88d998a408a0.png" style="width:100%;height: auto;" />
    </td>
    <td>
        <img src="https://user-images.githubusercontent.com/10555640/185813964-b44ac5ce-f0d3-4371-94b4-d103bce3c38b.png" style="width:100%;height: auto;" />
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
  
  <tr>
    <td>
        <img src="https://user-images.githubusercontent.com/10555640/185813481-1277314d-23d6-487b-a6b1-c94c515fec9d.png" style="width:100%;height: auto;" />
    </td>
    <td>
        <img src="https://user-images.githubusercontent.com/10555640/185814081-7c20bbe6-5978-4b4c-97d4-fb68cf789704.png" style="width:100%;height: auto;" />    
    </td>
  </tr>
<table>
  
## Sample
![CustomWindow v2 dark with search and like button](https://user-images.githubusercontent.com/10555640/185814020-45254966-7396-4e27-b6a4-0670bf0cbce9.png)

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
