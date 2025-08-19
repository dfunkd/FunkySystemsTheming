using RecipesWithFunk.Model.RequestModels;

namespace RecipesWithFunk.Model;

public class Recipe : BaseModel, ICloneable
{
    public string? Description { get; set; }
    public required string Directions { get; set; } = string.Empty;
    public required string Ingredients { get; set; } = string.Empty;
    public required string Name { get; set; } = string.Empty;
    public string? Note { get; set; }
    public required string Type { get; set; } = string.Empty;

    public int Rating { get; set; }
    public int RecipeId { get; set; }
    public int Servings { get; set; }

    public Recipe Clone()
        => (Recipe)MemberwiseClone();
    object ICloneable.Clone()
        => Clone();

    public bool Equals(Recipe? Recipe)
        => Recipe?.Description == Description && Recipe?.Directions == Directions && Recipe?.Ingredients == Ingredients && Recipe?.Name == Name
        && Recipe?.Note == Note && Recipe?.Type == Type && Recipe?.Rating == Rating && Recipe?.RecipeId == RecipeId;

    public AddRecipeRequest GetAddRecipeViewModel()
        => new()
        {
            Description = Description,
            Directions = Directions,
            Ingredients = Ingredients,
            Name = Name,
            Note = Note,
            Type = Type
        };

    public UpdateRecipeRequest GetUpdateRecipeViewModel()
        => new()
        {
            Description = Description,
            Directions = Directions,
            Ingredients = Ingredients,
            Name = Name,
            Note = Note,
            Type = Type
        };
}
