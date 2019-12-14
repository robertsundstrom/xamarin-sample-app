using System.ComponentModel.DataAnnotations;

namespace App1.ViewModels
{
    internal class NewItemViewModel : BaseViewModel
    {
        private string text;
        private string description;
        private bool isClean = true;

        [Required]
        public string Text
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
        public string Description
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
