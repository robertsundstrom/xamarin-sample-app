using System.Linq;

using App1.ViewModels;

using Xamarin.Forms;

namespace App1.Behaviors
{

    public class ViewValidationBehavior : Behavior<View>
    {
        private View? _associatedObject;
        private Label? validationLabel;
        private ValidationBase? currentBindingContext = null;

        protected override void OnAttachedTo(View bindable)
        {
            base.OnAttachedTo(bindable);

            _associatedObject = bindable;

            _associatedObject.BindingContextChanged += _associatedObject_BindingContextChanged;
        }

        private void _associatedObject_BindingContextChanged(object sender, System.EventArgs e)
        {
            if (currentBindingContext is ValidationBase oldBindingContext)
            {
                oldBindingContext.ErrorsChanged -= Source_ErrorsChanged;
            }
            if (_associatedObject?.BindingContext is ValidationBase newBindingContext)
            {
                newBindingContext.ErrorsChanged += Source_ErrorsChanged;
                currentBindingContext = newBindingContext;
            }
        }

        private void Source_ErrorsChanged(object sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            if (currentBindingContext != null && !string.IsNullOrEmpty(PropertyName))
            {
                Process(currentBindingContext);
            }
        }

        private void Process(ValidationBase source)
        {
            if (_associatedObject == null)
            {
                return;
            }

            if (validationLabel == null)
            {
                validationLabel = new Label
                {
                    Text = string.Empty,
                    TextColor = Color.Red,
                    FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                    IsVisible = false
                };

                var layout = _associatedObject.Parent as StackLayout;
                if (layout != null)
                {
                    int index = layout.Children.IndexOf(_associatedObject);
                    layout.Children.Insert(index + 1, validationLabel);
                }
            }

            var errors = source.GetErrors(PropertyName!).Cast<string>();
            if (errors != null && errors.Any())
            {
                validationLabel.Text = errors.First();
                validationLabel.IsVisible = true;
            }
            else
            {
                validationLabel.Text = string.Empty;
                validationLabel.IsVisible = false;
            }
        }

        protected override void OnDetachingFrom(View bindable)
        {
            if (_associatedObject == null || validationLabel == null)
            {
                return;
            }

            base.OnDetachingFrom(bindable);

            _associatedObject.BindingContextChanged -= _associatedObject_BindingContextChanged;

            if (currentBindingContext != null)
            {
                currentBindingContext.ErrorsChanged -= Source_ErrorsChanged;
            }

            _associatedObject = null;
        }

        public string? PropertyName { get; set; }
    }
}
