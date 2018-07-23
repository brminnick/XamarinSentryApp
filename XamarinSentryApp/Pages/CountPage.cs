using Xamarin.Forms;

namespace XamarinSentryApp
{
    public class CountPage : ContentPage
    {
        readonly Stepper _stepper;
        readonly Label _countLabel;

        public CountPage()
        {
            _stepper = new Stepper
            {
                Minimum = double.MinValue,
                Increment = 1,
                Value = 0
            };

            _countLabel = new Label
            {
                Text = "0",
                HorizontalTextAlignment = TextAlignment.Center
            };

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 20,

                Children = {
                    _countLabel,
                    _stepper
                }
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _stepper.ValueChanged += HandleStepperValueChanged;
        }

        async void HandleStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (e.NewValue < 0)
            {
                var shouldCrashApp = await DisplayAlert("Crash App?", "Tapping OK will crash the app", "OK", "Cancel");

                if (shouldCrashApp)
                    AnalyticsService.CrashApp();
            }
            else
            {
                if (e.NewValue > e.OldValue)
                    AnalyticsService.TrackEvent(AnalyticsConstants.Increment, AnalyticsConstants.NewValue, e.NewValue.ToString());
                else if (e.NewValue < e.OldValue)
                    AnalyticsService.TrackEvent(AnalyticsConstants.Decrement, AnalyticsConstants.NewValue, e.NewValue.ToString());

                _countLabel.Text = e.NewValue.ToString();
            }
        }
    }
}
