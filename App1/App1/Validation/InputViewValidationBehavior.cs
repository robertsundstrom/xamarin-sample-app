using System.Linq;

using App1.Effects;
using App1.ViewModels;

using Xamarin.Forms;

namespace App1.Validation
{
    public class InputViewValidationBehavior : Behavior<InputView>
    {
        private InputView? _associatedObject;
        private Label? validationLabel;

        protected override void OnAttachedTo(InputView bindable)
        {
            base.OnAttachedTo(bindable);

            _associatedObject = bindable;

            _associatedObject.TextChanged += _associatedObject_TextChanged;
        }

        private void _associatedObject_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_associatedObject?.BindingContext is ValidationBase source && !string.IsNullOrEmpty(PropertyName))
            {
                Process(source);
            }
        }

        private void Process(ValidationBase source)
        {
            if (_associatedObject == null || validationLabel == null)
            {
                return;
            }

            if (validationLabel == null)
            {
                validationLabel = new Label
                {
                    Text = string.Empty,
                    TextColor = Color.Red,
                    FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
                };

                var layout = _associatedObject.Parent as StackLayout;
                if (layout != null)
                {
                    var index = layout.Children.IndexOf(_associatedObject);
                    layout.Children.Insert(index + 1, validationLabel);
                }
            }

            var errors = source.GetErrors(PropertyName!).Cast<string>();
            if (errors != null && errors.Any())
            {
                var borderEffect = _associatedObject.Effects.FirstOrDefault(eff => eff is BorderEffect);
                if (borderEffect == null)
                {
                    _associatedObject.Effects.Add(new BorderEffect());
                }

                _associatedObject.TextColor = Color.Red;

                validationLabel.Text = errors.First();
            }
            else
            {
                var borderEffect = _associatedObject.Effects.FirstOrDefault(eff => eff is BorderEffect);
                if (borderEffect != null)
                {
                    _associatedObject.Effects.Remove(borderEffect);
                }

                _associatedObject.TextColor = Color.Black;

                validationLabel.Text = string.Empty;
            }
        }

        protected override void OnDetachingFrom(InputView bindable)
        {
            if (_associatedObject == null || validationLabel == null)
            {
                return;
            }

            base.OnDetachingFrom(bindable);

            _associatedObject.TextChanged -= _associatedObject_TextChanged;

            _associatedObject = null;
        }

        public string? PropertyName { get; set; }
    }
}
