using System.Globalization;

namespace Tonvo.Themes.Validation
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string valueString = (value ?? "").ToString();

            if (string.IsNullOrWhiteSpace(valueString) || !IsValidString(valueString))
                return new ValidationResult(false, "Некорректные данные");

            return ValidationResult.ValidResult;
        }
        private bool IsValidString(string valueString)
        {
            foreach (char c in valueString)
            {
                if (!char.IsLetter(c) && c != ' ')
                    return false;
            }
            return true;
        }
    }
}
