using System;
using System.Threading.Tasks;

using Xamarin.Forms;

using XamarinSentryApp.Shared;

namespace XamarinSentryApp
{
    public class CountPage : ContentPage
    {
        #region Constructors
        public CountPage()
        {
            Stepper = new Stepper
            {
                Minimum = double.MinValue,
                Increment = 1,
                Value = 0,
                AutomationId = AutomationIdConstants.Stepper,
            };

            CountLabel = new Label
            {
                Text = "0",
                HorizontalTextAlignment = TextAlignment.Center,
                AutomationId = AutomationIdConstants.CountLabel
            };

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 20,

                Children = {
                    CountLabel,
                    Stepper
                }
            };

            Title = PageTitleConstants.CountPage;
        }
        #endregion

        #region Properties
        public Stepper Stepper { get; }
        public Label CountLabel { get; }
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();

            Stepper.ValueChanged += HandleStepperValueChanged;
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

                CountLabel.Text = e.NewValue.ToString();
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
                Stepper.Value = 0;
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

                AnalyticsService.TrackEvent(AnalyticsConstants.UserFeedbackSent);
            }
            finally
            {
                Stepper.Value = 10;
            }
        }
        #endregion
    }
}
