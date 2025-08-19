using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace RecipesWithFunk.Converters;

public class RatingConverter : IValueConverter
{
    public LinearGradientBrush OnBrush { get; set; }
    public LinearGradientBrush OffBrush { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (int.TryParse(value.ToString(), out int rating) && int.TryParse(parameter.ToString(), out int number))
        {
            if (rating >= number)
                return OnBrush;
            return OffBrush;
        }

        return Brushes.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
