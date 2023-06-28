using System.Globalization;

namespace Tonvo.Themes.Validation
{
    public class ExperienceValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string experienceString = (value ?? "").ToString();

            if (string.IsNullOrWhiteSpace(experienceString))
                return new ValidationResult(false, "Это обязательное поле");

            if (!int.TryParse(experienceString, out int experience) || experience < 0 || experience > 60)
                return new ValidationResult(false, "Некорректный опыт работы");

            return ValidationResult.ValidResult;
        }
    }

}
