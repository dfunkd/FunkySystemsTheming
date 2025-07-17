using Microsoft.Win32;
using RecipesWithFunk.Data.Services;
using RecipesWithFunk.Model;
using RecipesWithFunk.ViewModels;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace RecipesWithFunk.UserControls;

public partial class ViewRecipesControl : UserControl
{
    #region Properties
    private IRecipeService recipeService;
    private CancellationToken cancellationToken = default;
    private bool _suspendChangeHandlers = false;
    #endregion

    #region Routed Commands
    #region AddCommand
    private static readonly RoutedCommand addCommand = new();
    public static RoutedCommand AddCommand = addCommand;
    private void CanExecuteAddCommand(object sender, CanExecuteRoutedEventArgs e)
        => e.CanExecute = e.Source is Control;
    private void ExecutedAddCommand(object sender, ExecutedRoutedEventArgs e)
    {
        ((App)Application.Current).SelectedRecipe = null;
    }
    #endregion

    #region CancelCommand
    private static readonly RoutedCommand cancelCommand = new();
    public static RoutedCommand CancelCommand = cancelCommand;
    private void CanExecuteCancelCommand(object sender, CanExecuteRoutedEventArgs e)
        => e.CanExecute = e.Source is Control;
    private void ExecutedCancelCommand(object sender, ExecutedRoutedEventArgs e)
    {
        if (DataContext is ViewRecipesControlViewModel vm)
        {
            vm.SelectedRecipe = null;
            lvRecipes.SelectedIndex = 1;
        }
    }
    #endregion

    #region DeleteRecipeCommand
    private static readonly RoutedCommand deleteRecipeCommand = new();
    public static RoutedCommand DeleteRecipeCommand = deleteRecipeCommand;
    private void CanExecuteDeleteRecipeCommand(object sender, CanExecuteRoutedEventArgs e)
        => e.CanExecute = e.Source is Control;
    private void ExecutedDeleteRecipeCommand(object sender, ExecutedRoutedEventArgs e)
    {
        if (e.OriginalSource is Control ctrl && ctrl.Parent is Grid grd && grd.DataContext is Recipe recipe)
        {
            ((App)Application.Current).SelectedRecipe = null;
            recipeService.DeleteRecipe(recipe.RecipeId, cancellationToken);
            //titleControl.ChangeContent(new ViewRecipesControl();
        }
    }
    #endregion

    #region EditCommand
    private static readonly RoutedCommand editCommand = new();
    public static RoutedCommand EditCommand = editCommand;
    private void CanExecuteEditCommand(object sender, CanExecuteRoutedEventArgs e)
        => e.CanExecute = e.Source is Control && DataContext is ViewRecipesControlViewModel vm && vm.SelectedRecipe is not null;
    private void ExecutedEditCommand(object sender, ExecutedRoutedEventArgs e)
    {
        if (DataContext is ViewRecipesControlViewModel vm)
        {
            ((App)Application.Current).SelectedRecipe = vm.SelectedRecipe;
            //titleControl.ChangeContent(new AddRecipeControl(recipeWindow, titleControl));
        }
    }
    #endregion

    #region ExportCommand
    private static readonly RoutedCommand exportCommand = new();
    public static RoutedCommand ExportCommand = exportCommand;
    private void CanExecuteExportCommand(object sender, CanExecuteRoutedEventArgs e)
        => e.CanExecute = e.Source is Control && DataContext is ViewRecipesControlViewModel vm && vm.Recipes.Count > 0;
    private void ExecutedExportCommand(object sender, ExecutedRoutedEventArgs e)
    {
        if (DataContext is not ViewRecipesControlViewModel vm)
            return;

        OpenFolderDialog dialog = new() { Title = "Select Path To Save Excel" };
        string? path = dialog.ShowDialog() == true ? dialog.FolderName : null;
        if (path is null)
            return;

        var json = vm.Recipes.ToJson();
        if (json is not null)
            File.WriteAllText(Path.Combine(path, "RecipeBackup.json"), json);
    }
    #endregion

    #region ImportCommand
    private static readonly RoutedCommand importCommand = new();
    public static RoutedCommand ImportCommand = importCommand;
    private void CanExecuteImportCommand(object sender, CanExecuteRoutedEventArgs e)
        => e.CanExecute = e.Source is Control && DataContext is ViewRecipesControlViewModel vm;
    private async void ExecutedImportCommand(object sender, ExecutedRoutedEventArgs e)
    {
        if (DataContext is not ViewRecipesControlViewModel vm)
            return;
        OpenFileDialog openFileDialog = new();
        if (openFileDialog.ShowDialog() != true)
            return;

        List<Recipe> skipped = [];
        using StreamReader sr = new(openFileDialog.FileName);
        var json = sr.ReadToEnd();
        List<Recipe>? recipes = json.FromJson<List<Recipe>>();
        foreach (Recipe recipe in recipes)
            if (await recipeService.RecipeExists(recipe.Name, cancellationToken) == false)
                await recipeService.AddRecipe(recipe.GetAddRecipeViewModel(), cancellationToken);
            else
                skipped.Add(recipe);
        if (skipped.Any())
        {
            //show popup
        }
        LoadData();
    }
    #endregion
    #endregion

    public ViewRecipesControl()
    {
        InitializeComponent();

        recipeService = new RecipeService();
        DataContext = new ViewRecipesControlViewModel();
    }

    #region Events
    private void OnGotFocus(object sender, RoutedEventArgs e)
    {
        if (sender is TextBox textBox)
            textBox.SelectAll();
        if (sender is PasswordBox passwordBox)
            passwordBox.SelectAll();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
        => LoadData();

    private void OnSearchChanged(object sender, TextChangedEventArgs e)
        => PopulateRecipes();

    private void OnTypeChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is ViewRecipesControlViewModel vm)
        {
            if (sender is ToggleButton tb && tb.Content is TextBlock txt)
            {
                if (tb.IsChecked == true)
                {
                    vm.TypesSearch.Add(txt.Text.ToString());
                    vm.GroupFilter.AddFilter(new Predicate<object>(o => ((Recipe)o).Name.Contains(tb.Content.ToString(), StringComparison.InvariantCultureIgnoreCase)));
                }
                else
                {
                    vm.TypesSearch.Remove(txt.Text.ToString());
                    vm.GroupFilter.RemoveFilter(new Predicate<object>(o => ((Recipe)o).Name.Contains(tb.Content.ToString(), StringComparison.InvariantCultureIgnoreCase)));
                }
            }

            PopulateRecipes();
        }
    }
    #endregion

    #region Functions
    private async void LoadData()
    {
        if (DataContext is ViewRecipesControlViewModel vm)
        {
            vm.Recipes = [.. await recipeService.GetAllRecipes()];

            PopulateRecipes();
        }
    }

    private void PopulateRecipes()
    {
        if (DataContext is ViewRecipesControlViewModel vm)
        {
            lvRecipes.Items.Clear();
            IEnumerable<Recipe> recipes = vm.Recipes.Where(w => w.Name.Contains(vm.SearchString, StringComparison.OrdinalIgnoreCase));
            foreach (var type in vm.TypesSearch)
                recipes = recipes.Where(w => vm.TypesSearch.Contains(w.Type));

            foreach (var recipe in recipes)
                lvRecipes.Items.Add(recipe);
        }
    }
    #endregion
}
