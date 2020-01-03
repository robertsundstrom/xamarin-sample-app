using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace App1.ViewModels
{
    public class ValidationBase : BindableBase, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        public ValidationBase()
        {
            ErrorsChanged += ValidationBase_ErrorsChanged;
        }

        private void ValidationBase_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasErrors));
            OnPropertyChanged(nameof(ErrorsList));
        }

        #region INotifyDataErrorInfo Members

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                if (_errors.ContainsKey(propertyName) && (_errors[propertyName].Any()))
                {
                    return _errors[propertyName].ToList();
                }
                else
                {
                    return new List<string>();
                }
            }
            else
            {
                return _errors.SelectMany(err => err.Value.ToList()).ToList();
            }
        }

        public bool HasErrors => _errors.Any(propErrors => propErrors.Value.Any());

        #endregion

        public virtual bool Validate()
        {
            var validationContext = new ValidationContext(this, null);

            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(this, validationContext, validationResults, true);

            _errors.Clear();

            HandleValidationResults(validationResults);

            return !HasErrors;
        }

        protected virtual bool ValidateProperty<T>(T value, bool updateErrors = true, [CallerMemberName] string propertyName = "")
        {
            if (propertyName == null)
            {
                throw new NullReferenceException(nameof(propertyName));
            }

            var validationContext = new ValidationContext(this, null)
            {
                MemberName = propertyName
            };

            var validationResults = new List<ValidationResult>();
            Validator.TryValidateProperty(value, validationContext, validationResults);

            if (updateErrors)
            {
                RemoveErrors(propertyName);

                HandleValidationResults(validationResults);
            }

            return !validationResults.Any();
        }

        protected void RemoveErrors([CallerMemberName] string propertyName = "")
        {
            if (_errors.ContainsKey(propertyName))
            {
                _errors.Remove(propertyName);
            }

            RaiseErrorsChanged(propertyName);
        }

        private void HandleValidationResults(List<ValidationResult> validationResults)
        {
            var resultsByPropertyName = from results in validationResults
                                        from memberNames in results.MemberNames
                                        group results by memberNames into groups
                                        select groups;

            foreach (var property in resultsByPropertyName)
            {
                _errors.Add(property.Key, property.Select(r => r.ErrorMessage).ToList());
                RaiseErrorsChanged(property.Key);
            }

        }

        private void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public IList<string> ErrorsList => GetErrors(string.Empty).Cast<string>().ToList();
    }
}
