using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace FestivalStoreApp.Common
{
    class NumerableToCommaSeparatedStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
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

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
