namespace RecipesWithFunk.Model.RequestModels;

public class AddRecipeRequest
{
    public string? Description { get; set; }
    public required string Directions { get; set; }
    public required string Ingredients { get; set; }
    public required string Name { get; set; }
    public string? Note { get; set; }
    public string? Type { get; set; }

    public int Rating { get; set; } = 0;
    public int Servings { get; set; } = 1;
}
