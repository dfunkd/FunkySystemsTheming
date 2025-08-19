using RecipesWithFunk.Data.Services;
using RecipesWithFunk.Model;
using RecipesWithFunk.Model.RequestModels;

namespace RecipesWithFunk.ViewModels;

public class AddRecipeControlViewModel : BaseViewModel
{
    IRecipeService service;

    public AddRecipeControlViewModel()
    {
        service = new RecipeService();
    }

    private int rating = 5;
    public int Rating
    {
        get => rating;
        set
        {
            if (rating != value)
            {
                rating = value;
                OnPropertyChanged();
            }
        }
    }

    private int recipeId = -1;
    public int RecipeId
    {
        get => recipeId;
        set
        {
            if (recipeId != value)
            {
                recipeId = value;
                OnPropertyChanged();
            }
        }
    }

    private int servings = 1;
    public int Servings
    {
        get => servings;
        set
        {
            if (servings != value)
            {
                servings = value;
                OnPropertyChanged();
            }
        }
    }

    private string description;
    public string Description
    {
        get => description;
        set
        {
            if (description != value)
            {
                description = value;
                OnPropertyChanged();
            }
        }
    }

    private string name;
    public string Name
    {
        get => name;
        set
        {
            if (name != value)
            {
                name = value;
                OnPropertyChanged();
            }
        }
    }

    private string note;
    public string Note
    {
        get => note;
        set
        {
            if (note != value)
            {
                note = value;
                OnPropertyChanged();
            }
        }
    }

    private string type;
    public string Type
    {
        get => type;
        set
        {
            if (type != value)
            {
                type = value;
                OnPropertyChanged();
            }
        }
    }

    private string directions;
    public string Directions
    {
        get => directions;
        set
        {
            if (directions != value)
            {
                directions = value;
                OnPropertyChanged();
            }
        }
    }

    private string ingredients;
    public string Ingredients
    {
        get => ingredients;
        set
        {
            if (ingredients != value)
            {
                ingredients = value;
                OnPropertyChanged();
            }
        }
    }

    public async Task<bool> UpsertRecipe(CancellationToken cancellationToken)
    {
        if (RecipeId == -1)
        {
            //add
            AddRecipeRequest addReq = new()
            {
                Description = Description,
                Directions = Directions,
                Ingredients = Ingredients,
                Name = Name,
                Note = Note,
                Rating = Rating,
                Servings = Servings,
                Type = Type
            };
            Recipe? recipe = await service.AddRecipe(addReq, cancellationToken);
            return recipe is not null;
        }
        else
        {
            //update
            UpdateRecipeRequest updateReq = new()
            {
                Description = Description,
                Directions = Directions,
                Ingredients = Ingredients,
                Name = Name,
                Note = Note,
                Rating = Rating,
                Servings = Servings,
                Type = Type
            };
            return await service.UpdateRecipe(RecipeId, updateReq, cancellationToken);
        }
    }
}
