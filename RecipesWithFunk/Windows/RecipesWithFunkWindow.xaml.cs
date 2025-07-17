using RecipesWithFunk.UserControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace RecipesWithFunk.Windows;

public partial class RecipesWithFunkWindow : Window
{
    #region Routed Commands
    #region Close Command
    private static readonly RoutedCommand closeCommand = new();
    public static RoutedCommand CloseCommand = closeCommand;
    private void CanExecuteCloseCommand(object sender, CanExecuteRoutedEventArgs e)
        => e.CanExecute = e.Source is Control;
    private void ExecutedCloseCommand(object sender, ExecutedRoutedEventArgs e)
        => Close();
    #endregion
    #endregion

    public RecipesWithFunkWindow()
    {
        InitializeComponent();
    }

    #region Events
    private void OnDrag(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            DragMove();
    }

    private void OnLoad(object sender, RoutedEventArgs e)
    {
        ChangeContent(new SplashControl());
    }

    private void OnMenuToggle(object sender, RoutedEventArgs e)
    {
        if (sender is not ToggleButton tbSender)
            return;

        string val = tbSender.Name;
        foreach (var control in wpMenu.Children)
        {
            if (control is not ToggleButton tb)
                continue;

            if (tb.Name != tbSender.Name && tb.IsChecked == true)
                tb.IsChecked = false;

            if (tbSender.IsChecked == false)
            {
                tbHome.IsChecked = true;
                val = "tbHome";
            }
        }

        switch (val)
        {
            case "tbHome":
                ChangeContent(new SplashControl());
                break;
            case "tbView":
                ChangeContent(new ViewRecipesControl());
                break;
            case "tbAdd":
                ChangeContent(new AddRecipeControl());
                break;
        }
    }
    #endregion

    #region Functions
    public void ChangeContent(Control control)
    {
        dpContent.Children.Clear();

        if (control is null)
        {
            tbHome.IsChecked = true;
            dpContent.Children.Add(new SplashControl());
            return;
        }

        dpContent.Children.Add(control);
    }
    #endregion
}
