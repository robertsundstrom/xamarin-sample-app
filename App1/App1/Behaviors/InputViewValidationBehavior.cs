using System.Linq;

using App1.Effects;
using App1.ViewModels;

using Xamarin.Forms;

namespace App1.Behaviors
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
                var borderEffect = _associatedObject.Effects.FirstOrDefault(eff => eff is BorderEffect);
                if (borderEffect == null)
                {
                    _associatedObject.Effects.Add(new BorderEffect());
                }

                _associatedObject.TextColor = Color.Red;

                validationLabel.Text = errors.First();
                validationLabel.IsVisible = true;
            }
            else
            {
                var borderEffect = _associatedObject.Effects.FirstOrDefault(eff => eff is BorderEffect);
                if (borderEffect != null)
                {
                    _associatedObject.Effects.Remove(borderEffect);
                }

                SetTextColor();

                validationLabel.Text = string.Empty;
                validationLabel.IsVisible = false;
            }
        }

        private void SetTextColor()
        {
            if (App.Current.Resources is ResourceDictionary resourceDictionary)
            {
                if (resourceDictionary.TryGetValue("PrimaryTextColor", out object value))
                {
                    if (_associatedObject != null)
                    {
                        _associatedObject.TextColor = (Color)value;
                    }
                }
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
