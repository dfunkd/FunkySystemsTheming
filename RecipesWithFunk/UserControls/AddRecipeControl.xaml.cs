using RecipesWithFunk.ViewModels;
using RecipesWithFunk.Windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RecipesWithFunk.UserControls;

public partial class AddRecipeControl : UserControl
{
    #region Routed Commands
    #region AddRecipeCommand
    private static readonly RoutedCommand addRecipeCommand = new();
    public static RoutedCommand AddRecipeCommand = addRecipeCommand;
    private void CanExecuteAddRecipeCommand(object sender, CanExecuteRoutedEventArgs e)
        => e.CanExecute = e.Source is Control;
    private void ExecutedAddRecipeCommand(object sender, ExecutedRoutedEventArgs e)
        => parent.ChangeContent(new AddRecipeControl(parent));
    #endregion

    #region CancelCommand
    private static readonly RoutedCommand cancelCommand = new();
    public static RoutedCommand CancelCommand = cancelCommand;
    private void CanExecuteCancelCommand(object sender, CanExecuteRoutedEventArgs e)
        => e.CanExecute = e.Source is Control;
    private void ExecutedCancelCommand(object sender, ExecutedRoutedEventArgs e)
        => parent.ChangeContent(new ViewRecipesControl(parent));
    #endregion

    #region DecreaseServingsCommand
    private static readonly RoutedCommand decreaseServingsCommand = new();
    public static RoutedCommand DecreaseServingsCommand = decreaseServingsCommand;
    private void CanExecuteDecreaseServingsCommand(object sender, CanExecuteRoutedEventArgs e)
        => e.CanExecute = e.Source is Control;
    private void ExecutedDecreaseServingsCommand(object sender, ExecutedRoutedEventArgs e)
        => parent.ChangeContent(new ViewRecipesControl(parent));
    #endregion

    #region IncreaseServingsCommand
    private static readonly RoutedCommand increaseServingsCommand = new();
    public static RoutedCommand IncreaseServingsCommand = increaseServingsCommand;
    private void CanExecuteIncreaseServingsCommand(object sender, CanExecuteRoutedEventArgs e)
        => e.CanExecute = e.Source is Control;
    private void ExecutedIncreaseServingsCommand(object sender, ExecutedRoutedEventArgs e)
        => parent.ChangeContent(new ViewRecipesControl(parent));
    #endregion

    #region RateCommand
    private static readonly RoutedCommand rateCommand = new();
    public static RoutedCommand RateCommand = rateCommand;
    private void CanExecuteRateCommand(object sender, CanExecuteRoutedEventArgs e)
        => e.CanExecute = e.Source is Control;
    private void ExecutedRateCommand(object sender, ExecutedRoutedEventArgs e)
    {
        if (sender is AddRecipeControl ctrl)
            if (DataContext is AddRecipeControlViewModel vm && int.TryParse(e.Parameter.ToString(), out int rating))
                vm.Rating = rating;
    }
    #endregion

    #region SaveCommand
    private static readonly RoutedCommand saveCommand = new();
    public static RoutedCommand SaveCommand = saveCommand;
    private void CanExecuteSaveCommand(object sender, CanExecuteRoutedEventArgs e)
    {
        if (DataContext is AddRecipeControlViewModel vm)
            e.CanExecute = vm.Directions?.Split(Environment.NewLine).Length > 0
                && vm.Ingredients?.Split(Environment.NewLine).Length > 0
                && !string.IsNullOrWhiteSpace(vm.Name)
                && !string.IsNullOrWhiteSpace(vm.Type);
    }
    private async void ExecutedSaveCommand(object sender, ExecutedRoutedEventArgs e)
    {
        if (DataContext is AddRecipeControlViewModel vm)
        {
            bool success = await vm.UpsertRecipe(cancellationToken);

            if (success)
                parent.ChangeContent(new ViewRecipesControl(parent));
        }
    }
    #endregion
    #endregion

    public AddRecipeControl(RecipesWithFunkWindow window)
    {
        InitializeComponent();

        parent = window;
    }

    #region Properties
    private bool isEdit;
    private CancellationToken cancellationToken = default;
    private RecipesWithFunkWindow parent = null;
    #endregion

    #region Events
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is AddRecipeControlViewModel vm)
        {
            var currentApp = (App)Application.Current;
            if (currentApp.SelectedRecipe is null)
            {
                isEdit = false;
                vm.Description = "";
                vm.Directions = "";
                vm.Ingredients = "";
                vm.Name = "";
                vm.Note = "";
                vm.Rating = 0;
                vm.RecipeId = -1;
                vm.Type = "";
            }
            else
            {
                isEdit = true;
                vm.Description = currentApp.SelectedRecipe.Description ?? "";
                vm.Directions = currentApp.SelectedRecipe.Directions;
                vm.Ingredients = currentApp.SelectedRecipe.Ingredients;
                vm.Name = currentApp.SelectedRecipe.Name;
                vm.Note = currentApp.SelectedRecipe.Note ?? "";
                vm.Rating = currentApp.SelectedRecipe.Rating;
                vm.RecipeId = currentApp.SelectedRecipe.RecipeId;
                vm.Type = currentApp.SelectedRecipe.Type ?? "";
            }
            isEdit = ((App)Application.Current).SelectedRecipe is not null;
        }
    }

    private void OnServingsChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        => e.Handled = e.NewValue is not null && int.TryParse(e.NewValue.ToString(), out int value) && value >= 1;

    private void OnServingsPreviewKeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.D0:
            case Key.NumPad0:
            case Key.D1:
            case Key.NumPad1:
            case Key.D2:
            case Key.NumPad2:
            case Key.D3:
            case Key.NumPad3:
            case Key.D4:
            case Key.NumPad4:
            case Key.D5:
            case Key.NumPad5:
            case Key.D6:
            case Key.NumPad6:
            case Key.D7:
            case Key.NumPad7:
            case Key.D8:
            case Key.NumPad8:
            case Key.D9:
            case Key.NumPad9:
                e.Handled = false;
                break;
            default:
                e.Handled = true;
                break;
        }
    }
    #endregion
}
