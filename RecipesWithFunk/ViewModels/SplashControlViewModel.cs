namespace RecipesWithFunk.ViewModels;

public class SplashControlViewModel : BaseViewModel
{
    private string title = "Keep your friends close"; //"Funky Recipes";
    public string Title
    {
        get => title;
        set
        {
            if (title != value)
            {
                title = value;
                OnPropertyChanged();
            }
        }
    }

    private string subTitle = "and your recipes closer!"; //"Keep your friends close and your recipes closer!";
    public string SubTitle
    {
        get => subTitle;
        set
        {
            if (subTitle != value)
            {
                subTitle = value;
                OnPropertyChanged();
            }
        }
    }
}
