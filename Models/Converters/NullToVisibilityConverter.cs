using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace GeoMeasure.Models.Converters
{
    public class NullToVisibilityConverter : IValueConverter
    {
        public bool Invert { get; set; }
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s) return !string.IsNullOrEmpty(s) ^ Invert ? Visibility.Visible : Visibility.Collapsed;
            return value != null ^ Invert ? Visibility.Visible : Visibility.Collapsed;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
