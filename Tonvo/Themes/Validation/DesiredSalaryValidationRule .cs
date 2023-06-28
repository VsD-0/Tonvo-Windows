using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tonvo.Themes.Validation
{
    public class DesiredSalaryValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string salaryString = (value ?? "").ToString();

            if (string.IsNullOrWhiteSpace(salaryString))
                return new ValidationResult(false, "Это обязательное поле");

            if (!int.TryParse(salaryString, out int salary) || salary < 1000 || salary.ToString().Length > 7)
                return new ValidationResult(false, "Некорректная заработная плата");

            return ValidationResult.ValidResult;
        }
    }
}
