using System.ComponentModel.DataAnnotations;

using App1.Resources;

namespace App1.ViewModels
{
    internal class NewItemViewModel : ViewModelBase
    {
        private string? text;
        private string? description;
        private bool isClean = true;

        [Required(ErrorMessageResourceName = nameof(AppResources.FieldRequiredMessage), ErrorMessageResourceType = typeof(AppResources))]
        public string? Text
        {
            get => text;
            set
            {
                if (ValidateProperty(value, false))
                {
                    RemoveErrors();
                }
                SetProperty(ref text, value);
                isClean = false;
                OnPropertyChanged(nameof(CanSubmit));
            }
        }

        [Required(ErrorMessageResourceName = nameof(AppResources.FieldRequiredMessage), ErrorMessageResourceType = typeof(AppResources))]
        public string? Description
        {
            get => description;
            set
            {
                if (ValidateProperty(value, false))
                {
                    RemoveErrors();
                }
                SetProperty(ref description, value);
                isClean = false;
                OnPropertyChanged(nameof(CanSubmit));
            }
        }

        public bool CanSubmit => !isClean && Validate();
    }
}
