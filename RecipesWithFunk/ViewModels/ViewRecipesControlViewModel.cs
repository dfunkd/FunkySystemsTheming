using RecipesWithFunk.Core.Filter;
using RecipesWithFunk.Data.Services;
using RecipesWithFunk.Model;
using System.Collections.ObjectModel;
using System.IO;

namespace RecipesWithFunk.ViewModels;

public class ViewRecipesControlViewModel : BaseViewModel
{
    IRecipeService service;

    public ViewRecipesControlViewModel()
    {
        service = new RecipeService();
    }

    private GroupFilter groupFilter = new();
    public GroupFilter GroupFilter
    {
        get => groupFilter;
        set
        {
            if (groupFilter != value)
            {
                groupFilter = value;
                OnPropertyChanged();
            }
        }
    }

    private string searchString = string.Empty;
    public string SearchString
    {
        get => searchString;
        set
        {
            if (searchString != value)
            {
                searchString = value;
                OnPropertyChanged();
            }
        }
    }

    private string typeSearch = string.Empty;
    public string TypeSearch
    {
        get => typeSearch;
        set
        {
            if (typeSearch != value)
            {
                typeSearch = value;
                OnPropertyChanged();
            }
        }
    }

    private List<string> typesSearch = [];
    public List<string> TypesSearch
    {
        get => typesSearch;
        set
        {
            if (typesSearch != value)
            {
                typesSearch = value;
                OnPropertyChanged();
            }
        }
    }

    private Recipe? selectedRecipe;
    public Recipe? SelectedRecipe
    {
        get => selectedRecipe;
        set
        {
            if (selectedRecipe != value)
            {
                selectedRecipe = value;
                OnPropertyChanged();
            }
        }
    }

    private ObservableCollection<Recipe> recipes = [];
    public ObservableCollection<Recipe> Recipes
    {
        get => recipes;
        set
        {
            if (recipes != value)
            {
                recipes = value;
                OnPropertyChanged();
            }
        }
    }

    private ObservableCollection<string> types = [];
    public ObservableCollection<string> Types
    {
        get => types;
        set
        {
            if (types != value)
            {
                types = value;
                OnPropertyChanged();
            }
        }
    }

    public async Task DeleteRecipe(int recipeId, CancellationToken cancellationToken)
        => await service.DeleteRecipe(recipeId, cancellationToken);

    public async Task ImportRecipes(string fileName, CancellationToken cancellationToken)
    {
        using StreamReader sr = new(fileName);
        string json = sr.ReadToEnd();
        foreach (Recipe recipe in json.FromJson<List<Recipe>>())
            if (await service.RecipeExists(recipe.Name, cancellationToken) == false)
                await service.AddRecipe(recipe.GetAddRecipeViewModel(), cancellationToken);
    }

    public async Task PopulateTypes()
        => Types = [.. await service.GetAllTypes(default)];

    public async Task PopulateRecipes()
        => Recipes = [.. await service.GetAllRecipes()];
}
