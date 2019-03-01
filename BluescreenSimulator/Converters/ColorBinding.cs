using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace BluescreenSimulator.Converters
{
    public class ColorBinding : Binding
    {
        public ColorBinding()
        {
            Initialize();
        }
        public ColorBinding(string path) : base(path)
        {
            Initialize();
        }

        private void Initialize()
        {
            Converter = new BrushConverter();
        }

        public class BrushConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is Color c)
                {
                    return new SolidColorBrush(c);
                }
                return DoNothing;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return (value as SolidColorBrush)?.Color;
            }
        }
    }
}