using System;
using System.Globalization;
using System.Windows.Data;

namespace GeradorArquivo.Converters
{
    public class ConvertNumeric : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ret = System.Convert.ToInt32(value);
            return ret.ToString("N0");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
