using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using MMIM.Enum;

namespace MMIM.Converters
{
    public class ToggleStateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ItemToggleStates)) return DependencyProperty.UnsetValue;
            var toggleState = (ItemToggleStates) value;

            return toggleState == ItemToggleStates.NotSelected ? Brushes.PaleGoldenrod
                : toggleState == ItemToggleStates.Selected ? Brushes.DarkGreen
                : toggleState == ItemToggleStates.Enabled ? Brushes.DarkBlue
                : toggleState == ItemToggleStates.Disabled ? Brushes.DarkRed
                : Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
