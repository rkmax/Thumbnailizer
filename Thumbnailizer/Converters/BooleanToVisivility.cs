using System.Windows.Data;

namespace Thumbnailizer.Converters
{
    class BooleanToVisivility : IValueConverter
    {

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var element = (bool)value ? "Visible" : "Hidden";
            return element.ToString();
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
