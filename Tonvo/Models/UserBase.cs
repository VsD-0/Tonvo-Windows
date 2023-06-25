using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Tonvo.Core;
using EventManager = Tonvo.Core.EventManager;

namespace Tonvo.Models
{
    internal abstract class UserBase : IUser, INotifyDataErrorInfo
    {
        protected readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();

        protected UserBase()
        {
            ValidateApplicantEmail = new TargetRelayCommand(OnValidateApplicantEmail, CanValidateApplicantEmail);
            ValidateApplicantPassword = new TargetRelayCommand(OnValidateApplicantPassword, CanValidateApplicantPassword);

            EventManager.Validated += OnValidateApplicantEmail;
            EventManager.Validated += OnValidateApplicantPassword;
        }

        public bool HasErrors => _errorsByPropertyName.Any();

        public abstract string Email { get; set; }
        public abstract string Password { get; set; }


        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsByPropertyName.ContainsKey(propertyName) ?
            _errorsByPropertyName[propertyName] : null;
        }

        protected void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        protected void AddError(string propertyName, string error)
        {
            if (!_errorsByPropertyName.ContainsKey(propertyName))
                _errorsByPropertyName[propertyName] = new List<string>();

            if (!_errorsByPropertyName[propertyName].Contains(error))
            {
                _errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        protected void ClearErrors(string propertyName)
        {
            if (_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }

        public TargetRelayCommand ValidateApplicantEmail { get; set; }
        async private void OnValidateApplicantEmail()
        {
            ClearErrors(nameof(Email));
            if (string.IsNullOrWhiteSpace(Email))
            {
                AddError(nameof(Email), "E-mail не может быть пустым");
                return;
            }
            if (!new EmailAddressAttribute().IsValid(Email ?? throw new ArgumentNullException()))
            {
                AddError(nameof(Email), "Некоректная электронная почта");
                return;
            }

            // TODO: Проверка на существование почты
            //var applicants = await _applicantService.GetList();
            //if (applicants != null)
            //{
            //    foreach (ApplicantModel item in applicants)
            //    {
            //        if (Email.Equals(item.Email))
            //        {
            //            AddError(nameof(Email), "Эта почта уже занята. Попробуйте другую");
            //        }
            //    }
            //}
        }
        private bool CanValidateApplicantEmail() { return true; }

        public TargetRelayCommand ValidateApplicantPassword { get; set; }
        private void OnValidateApplicantPassword()
        {
            ClearErrors(nameof(Password));
            if (string.IsNullOrWhiteSpace(Password))
            {
                AddError(nameof(Password), "Пароль не может быть пустым");
                return;
            }
            if (!Password.Any(char.IsPunctuation))
            {
                AddError(nameof(Password), "Пароль должен содержать спецсимволы");
                return;
            }
            if (!Password.Any(char.IsDigit))
            {
                AddError(nameof(Password), "Пароль должен содержать цифры");
                return;
            }
            if (!Password.Any(char.IsLetter))
            {
                AddError(nameof(Password), "Пароль должен содержать буквы");
                return;
            }
            if (!Password.Any(char.IsUpper))
            {
                AddError(nameof(Password), "Пароль должен содержать прописные буквы");
                return;
            }
            if (!Password.Any(char.IsLower))
            {
                AddError(nameof(Password), "Пароль должен содержать строчные буквы");
                return;
            }
            if (Password == null || Password?.Length <= 7)
            {
                AddError(nameof(Password), "Пароль должен быть больше 8 символов");
            }
        }
        private bool CanValidateApplicantPassword() { return true; }
    }
}
