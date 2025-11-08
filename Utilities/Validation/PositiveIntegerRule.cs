using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CrosswordsPuzzleGenerator.Utilities.Validation
{
    // ValidationResult in both classes need to be pre appended with the actual namespace, otherwise they confilcit with eachoter.
    public class PositiveIntegerRule : ValidationRule
    {
        public override System.Windows.Controls.ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (int.TryParse(value?.ToString(), out int result) && result > 0)
                return System.Windows.Controls.ValidationResult.ValidResult;

            return new System.Windows.Controls.ValidationResult(false, "Enter a positive integer");
        }
    }

    public sealed class PositiveInteger : ValidationAttribute
    {
        protected override System.ComponentModel.DataAnnotations.ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (int.TryParse(value?.ToString(), out int result) && result > 0)
                return System.ComponentModel.DataAnnotations.ValidationResult.Success;

            return new System.ComponentModel.DataAnnotations.ValidationResult("Enter a positive integer");
        }
    }
}
