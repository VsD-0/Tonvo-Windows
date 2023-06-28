using System.Globalization;
using System.Text.RegularExpressions;

namespace Tonvo.Themes.Validation
{
    public class PhoneValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string phoneNumber = (value ?? "").ToString();

            if (string.IsNullOrWhiteSpace(phoneNumber))
                return new ValidationResult(false, "Это обязательное поле");

            // Проверка на корректность номера телефона
            string phonePattern = @"^(\+7|8)\s?\(?(?:[0-9]{3})\)?(?:[0-9]{3}[\s-]?[0-9]{2}[\s-]?[0-9]{2})$";
            Regex regex = new Regex(phonePattern);

            if (!regex.IsMatch(phoneNumber))
                return new ValidationResult(false, "Некорректный номер телефона");

            return ValidationResult.ValidResult;
        }
    }
}
