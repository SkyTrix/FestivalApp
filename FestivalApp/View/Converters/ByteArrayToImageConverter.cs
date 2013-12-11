using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace FestivalApp.View.Converters
{
    class ByteArrayToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return new BitmapImage(new Uri(@"pack://application:,,,/FestivalApp;component/View/images/noimage.png"));

            try
            {
                using (var ms = new System.IO.MemoryStream((byte[])value))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                    return image;
                }
            }
            catch(Exception)
            {
                return new BitmapImage(new Uri(@"pack://application:,,,/FestivalApp;component/View/images/noimage.png"));;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
