using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using TestManager.Lib.ViewModels;

namespace TestManager.Converters
{
    public class HttpMethodToBoolConverter : IValueConverter
    {
        public Method SelectedMethod { get; set; }

        public HttpMethodToBoolConverter()
        {
            this.SelectedMethod = Method.Get;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Binding.DoNothing;

            if (value is Method == false)
                return Binding.DoNothing;

            Method input = (Method)value;

            return input == this.SelectedMethod;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
