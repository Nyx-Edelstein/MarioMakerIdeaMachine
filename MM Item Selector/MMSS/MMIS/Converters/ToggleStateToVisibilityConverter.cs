using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using MMIM.Enum;

namespace MMIM.Converters
{
    public class ToggleStateToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ItemToggleStates)) return DependencyProperty.UnsetValue;
            var toggleState = (ItemToggleStates)value;

            return toggleState == ItemToggleStates.Disabled ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
