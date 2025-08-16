using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ConnectFour.Converters
{
    public class PlayerToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var p = value is Player pl ? pl : Player.None;
            return p switch
            {
                Player.Red => Brushes.Red,
                Player.Yellow => Brushes.Gold,
                _ => Brushes.White // empty cell
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
