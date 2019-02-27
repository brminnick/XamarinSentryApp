using System;
using System.Threading.Tasks;
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
                Value = 0,
                HorizontalOptions = LayoutOptions.Center
            };
            _stepper.ValueChanged += HandleStepperValueChanged;

            _countLabel = new Label
            {
                Text = "0",
                HorizontalTextAlignment = TextAlignment.Center
            };

            var explainationLabel = new Label
            {
                Text = "Crank it up to eleven\n...or drop it below zero",
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.DimGray
            };

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 20,

                Children = {
                    _countLabel,
                    _stepper,
                    explainationLabel
                }
            };
        }

        async void HandleStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (e.NewValue < 0)
            {
                await PromptForCrash();
            }
            else if (e.NewValue > 10)
            {
                await GetFeedback();
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

        async Task PromptForCrash()
        {
            try
            {
                AnalyticsService.CrashApp();
            }
            catch (Exception ex)
            {
                var reportTask = Task.Run(() => AnalyticsService.Report(ex));

                var shouldCrashApp = await DisplayAlert("Crash App?", "Tapping OK will crash the app", "OK", "Cancel");

                await reportTask;

                if (shouldCrashApp)
                    throw;
            }
            finally
            {
                _stepper.Value = 0;
            }
        }

        async Task GetFeedback()
        {
            try
            {
                var isEnjoying = await DisplayAlert("Enjoying the app?", "Are you enjoying the app?", "Yes", "No");

                if (isEnjoying)
                    await AnalyticsService.SendUserFeedback("Great app!");
                else
                    await AnalyticsService.SendUserFeedback("Meh");
            }
            finally
            {
                _stepper.Value = 10;
            }
        }
    }
}
