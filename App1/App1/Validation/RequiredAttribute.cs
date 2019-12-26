using App1.Resources;

namespace App1.Validation
{
    public class RequiredAttribute : System.ComponentModel.DataAnnotations.RequiredAttribute
    {
        public RequiredAttribute()
        {
            ErrorMessageResourceName = nameof(AppResources.FieldRequiredMessage);
            ErrorMessageResourceType = typeof(AppResources);
        }
    }
}
