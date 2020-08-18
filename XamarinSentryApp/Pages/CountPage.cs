using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Markup;

namespace XamarinSentryApp
{
    public class CountPage : ContentPage
    {
        readonly Stepper _stepper;
        readonly Label _countLabel;

        public CountPage()
        {
            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 20,

                Children =
                {
                    new Label { Text = "0" }.TextCenter().Assign(out _countLabel),

                    new Stepper
                    {
                        Minimum = double.MinValue,
                        Increment = 1,
                        Value = 0,
                    }.Center().Assign(out _stepper)
                     .Invoke(stepper => stepper.ValueChanged += HandleStepperValueChanged),

                    new Label { Text = "Crank it up to eleven\n...or drop it below zero", TextColor = Color.DimGray }.TextCenter()
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
                var shouldCrashApp = await DisplayAlert("Crash App?", "Tapping OK will crash the app", "OK", "Cancel");

                await AnalyticsService.Report(ex);

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
