using App1.Validation;

namespace App1.ViewModels
{
    internal class NewItemViewModel : ViewModelBase
    {
        private string? text;
        private string? description;
        private bool isClean = true;

        [Required]
        public string? Text
        {
            get => text;
            set
            {
                ValidateProperty(value);
                SetProperty(ref text, value);
                isClean = false;
                OnPropertyChanged(nameof(CanSubmit));
            }
        }

        [Required]
        public string? Description
        {
            get => description;
            set
            {
                ValidateProperty(value);
                SetProperty(ref description, value);
                isClean = false;
                OnPropertyChanged(nameof(CanSubmit));
            }
        }

        public bool CanSubmit => !isClean && Validate();
    }
}
