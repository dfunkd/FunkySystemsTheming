using RecipesWithFunk.Model;
using System.Windows;

namespace RecipesWithFunk;

public partial class App : Application
{
    public Recipe? SelectedRecipe { get; set; }
}
