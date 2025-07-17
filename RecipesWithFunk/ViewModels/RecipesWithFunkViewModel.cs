using System.Windows;

namespace RecipesWithFunk.ViewModels;

public class RecipesWithFunkViewModel : BaseViewModel
{
    private string copyRight = $"Funky Designs © {DateTime.Now:yyyy}";
    public string CopyRight
    {
        get => copyRight;
        set
        {
            if (copyRight != value)
            {
                copyRight = value;
                OnPropertyChanged();
            }
        }
    }
}
