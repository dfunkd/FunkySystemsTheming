using System.Windows;
using System.Windows.Controls;

namespace FunkySystemsTheming.UserControls;

public partial class HorizontalHeaderedTextBox : UserControl
{
    public static readonly DependencyProperty ButtonTextProperty = DependencyProperty.Register("ButtonText", typeof(string), typeof(HorizontalHeaderedTextBox));
    public string ButtonText
    {
        get => (string)GetValue(ButtonTextProperty);
        set => SetValue(ButtonTextProperty, value);
    }

    public static readonly DependencyProperty ButtonVisibilityProperty = DependencyProperty.Register("ButtonVisibility", typeof(Visibility), typeof(HorizontalHeaderedTextBox));
    public Visibility ButtonVisibility
    {
        get => (Visibility)GetValue(ButtonVisibilityProperty);
        set => SetValue(ButtonVisibilityProperty, value);
    }

    public static readonly DependencyProperty ButtonWidthProperty = DependencyProperty.Register("ButtonWidth", typeof(double), typeof(HorizontalHeaderedTextBox));
    public double ButtonWidth
    {
        get => (double)GetValue(ButtonWidthProperty);
        set => SetValue(ButtonWidthProperty, value);
    }

    public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register("HeaderText", typeof(string), typeof(HorizontalHeaderedTextBox));
    public string HeaderText
    {
        get => (string)GetValue(HeaderTextProperty);
        set => SetValue(HeaderTextProperty, value);
    }

    public HorizontalHeaderedTextBox()
    {
        InitializeComponent();
    }
}
