using RecipesWithFunk.Data.Repositories;
using RecipesWithFunk.Model;
using RecipesWithFunk.Model.RequestModels;

namespace RecipesWithFunk.Data.Services;

public interface IRecipeService
{
    Task<Recipe?> AddRecipe(AddRecipeRequest recipe, CancellationToken cancellationToken = default);
    Task<bool> DeleteRecipe(int recipeId, CancellationToken cancellationToken = default);
    Task<List<Recipe>> GetAllRecipes(CancellationToken cancellationToken = default);
    Task<List<string>> GetAllTypes(CancellationToken cancellationToken = default);
    Task<Recipe?> GetRecipeById(int recipeId, CancellationToken cancellationToken = default);
    Task<bool> RecipeExists(string recipeName, CancellationToken cancellationToken = default);
    Task<bool> UpdateRecipe(int recipeId, UpdateRecipeRequest recipe, CancellationToken cancellationToken = default);
}

public class RecipeService : IRecipeService
{
    private IRecipeRepository repo;

    public RecipeService()
    {
        repo = new RecipeRepository();
    }

    public async Task<Recipe?> AddRecipe(AddRecipeRequest recipe, CancellationToken cancellationToken = default)
        => await repo.AddRecipe(recipe, cancellationToken);

    public async Task<bool> DeleteRecipe(int recipeId, CancellationToken cancellationToken = default)
        => await repo.DeleteRecipe(recipeId, cancellationToken);

    public async Task<List<Recipe>> GetAllRecipes(CancellationToken cancellationToken = default)
        => await repo.GetAllRecipes(cancellationToken);

    public async Task<List<string>> GetAllTypes(CancellationToken cancellationToken = default)
        => await repo.GetAllTypes(cancellationToken);

    public async Task<Recipe?> GetRecipeById(int recipeId, CancellationToken cancellationToken = default)
        => await repo.GetRecipeById(recipeId, cancellationToken);

    public async Task<bool> RecipeExists(string recipeName, CancellationToken cancellationToken = default)
        => await repo.RecipeExists(recipeName, cancellationToken);

    public async Task<bool> UpdateRecipe(int recipeId, UpdateRecipeRequest recipe, CancellationToken cancellationToken = default)
        => await repo.UpdateRecipe(recipeId, recipe, cancellationToken);
}
