using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Skitter.ViewModel.ViewModels.Configuration;

namespace Skitter.Wpf.Converters
{
    class TypeParticipantConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ConfigurationTournoiViewModel.TypeParticipantToString(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
