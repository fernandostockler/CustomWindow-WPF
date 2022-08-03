namespace CustomWindow_WPF.SampleApp;

using System.Windows;
using System.Windows.Controls;

/// <summary>
/// Lógica interna para CustomWindowSample.xaml
/// </summary>
public partial class CustomWindowSample : CustomWindow
{
    public CustomWindowSample()
    {
        InitializeComponent(); //utilities
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