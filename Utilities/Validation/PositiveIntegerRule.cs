using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CrosswordsPuzzleGenerator.Utilities.Validation
{
    public class PositiveIntegerRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (int.TryParse(value?.ToString(), out int result) && result > 0)
                return ValidationResult.ValidResult;

            return new ValidationResult(false, "Enter a positive integer");
        }
    }
}
