using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace CrosswordsPuzzleGenerator.Utilities.Converters
{
    class ValidationErrorsToStringConverter : IMultiValueConverter
    {
        public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values ==  null || values.Length < 2)
            {
                return null;
            }

            bool hasError = values[0] as bool? ?? false;
            var errors = values[1] as System.Collections.IEnumerable;

            if (!hasError || errors == null)
            {
                return null;
            }

            foreach ( var error in errors )
            {
                if (error is ValidationError ve)
                    return ve.ErrorContent?.ToString();
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
