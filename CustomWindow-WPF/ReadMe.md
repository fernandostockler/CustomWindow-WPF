# [CustomWindow](Docs/CustomWindow.md)
It is a window that allows customization of the non-client area, has a kiosk mode and has a mechanism for displaying modal content.

<table>
  <tr>
    <td>
        <img src="Images/CustomWindow v1 Customizable area in orange.png" style="width:100%;height: auto;" />
    </td>
    <td>
        <img src="Images/CustomWindow v1 CustomWindow sample1.png" style="width:100%;height: auto;" />
    </td>
  </tr>

  <tr>
    <td>
        <img src="Images/CustomWindow v1 CustomWindow search.png" style="width:100%;height: auto;" />
    </td>
    <td>
        <img src="Images/CustomWindow v2 dark with search and like button.png" style="width:100%;height: auto;" />
    </td>
  </tr>
  <!--<tr>
    <td colspan="2">
        <img src="Images/CustomWindow v1 CustomWindow search xaml.png" />
    </td>
  </tr>-->
<table>



Markup code in CustomWindowSample.xaml
![Custom Window V2 Markup Code](Images/CustomWindow%20v2%20markup%20code.png)

Code behind


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
