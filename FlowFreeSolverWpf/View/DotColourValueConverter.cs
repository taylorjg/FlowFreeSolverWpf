using System;
using System.Globalization;
using System.Windows.Data;
using FlowFreeSolverWpf.Model;

namespace FlowFreeSolverWpf.View
{
    public class DotColourValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dotColour = (DotColour) value;
            return dotColour.ToWpfColour();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
