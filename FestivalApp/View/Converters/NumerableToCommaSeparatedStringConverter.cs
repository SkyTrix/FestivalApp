using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FestivalApp.View.Converters
{
    [ValueConversion(typeof(IEnumerable), typeof(string))]
    class NumerableToCommaSeparatedStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string output = "";
            IEnumerable collection = value as IEnumerable;

            if (collection == null) return "";
            
            foreach (object obj in (IEnumerable)value)
            {
                if (output.Length > 0) output += ", ";
                output += obj.ToString();
            }

            return output.Length > 0 ? output : "-";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
