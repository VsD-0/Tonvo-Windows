using System.Globalization;
using System.Text.RegularExpressions;

namespace Tonvo.Themes.Validation
{
    public class EmailValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string email = (value ?? "").ToString();

            if (string.IsNullOrWhiteSpace(email))
                return new ValidationResult(false, "Это обязательное поле");

            // Проверка на корректность электронной почты
            string emailPattern = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
            Regex regex = new Regex(emailPattern);

            if (!regex.IsMatch(email))
                return new ValidationResult(false, "Некорректный адрес электронной почты");

            return ValidationResult.ValidResult;
        }
    }
}