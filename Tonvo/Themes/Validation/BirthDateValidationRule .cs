using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tonvo.Themes.Validation
{
    public class BirthDateValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            DateTime currentDate = DateTime.Now;
            DateTime birthDate;

            if (value is DateTime date)
                birthDate = date;
            else if (DateTime.TryParse(value?.ToString(), out date))
                birthDate = date;
            else
                return new ValidationResult(false, "Некорректная дата рождения");

            if (birthDate > currentDate)
                return new ValidationResult(false, "Дата рождения не может быть в будущем");

            int age = currentDate.Year - birthDate.Year;
            if (currentDate.Month < birthDate.Month || (currentDate.Month == birthDate.Month && currentDate.Day < birthDate.Day))
                age--;

            if (age < 14)
                return new ValidationResult(false, "Пользователю должно быть больше 14 лет");

            return ValidationResult.ValidResult;
        }
    }
}
