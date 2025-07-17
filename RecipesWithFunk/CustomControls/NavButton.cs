using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RecipesWithFunk.CustomControls;

public class NavButton : ListBoxItem
{
    public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NavButton));
    public event RoutedEventHandler Click
    {
        add => AddHandler(ClickEvent, value);
        remove => RemoveHandler(ClickEvent, value);
    }
    void RaiseClickEvent()
    {
        RoutedEventArgs e = new(ClickEvent);
        RaiseEvent(e);
    }
    void OnClick()
        => RaiseClickEvent();

    static NavButton()
        => DefaultStyleKeyProperty.OverrideMetadata(typeof(NavButton), new FrameworkPropertyMetadata(typeof(NavButton)));

    public static readonly DependencyProperty NavLinkProperty = DependencyProperty.Register("NavLink", typeof(Uri), typeof(NavButton), new PropertyMetadata(null));
    public Uri NavLink
    {
        get => (Uri)GetValue(NavLinkProperty);
        set => SetValue(NavLinkProperty, value);
    }

    public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(Geometry), typeof(NavButton), new PropertyMetadata(null));
    public Geometry Icon
    {
        get => (Geometry)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
}
