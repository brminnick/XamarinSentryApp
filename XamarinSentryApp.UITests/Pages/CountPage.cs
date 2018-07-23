using System.Linq;

using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.iOS;

using XamarinSentryApp.Shared;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace XamarinSentryApp.UITests
{
    class CountPage : BasePage
    {
        readonly Query _countLabel, _stepper;

        public CountPage(IApp app) : base(app, PageTitleConstants.CountPage)
        {
            _countLabel = x => x.Marked(AutomationIdConstants.CountLabel);
            _stepper = x => x.Marked(AutomationIdConstants.Stepper);
        }

        public string CountLabelText
        {
            get
            {
                var countLabel = App.Query(_countLabel).FirstOrDefault();
                return countLabel?.Text ?? string.Empty;
            }
        }

        public void SetStepperValue(int stepperValue)
        {
            switch (App)
            {
                case iOSApp iOSApp:
                    iOSApp.Invoke("setStepperValue:", stepperValue.ToString());
                    break;

                case AndroidApp androidApp:
                    androidApp.Invoke("SetStepperValue", stepperValue);
                    break;
            }
        }

        public void IncrementStepper()
        {
            switch (App)
            {
                case iOSApp iOSApp:
                    iOSApp.Tap(x => x.Marked("Increment"));
                    break;

                case AndroidApp androidApp:
                    androidApp.Tap(x => x.Class("android.widget.Button").Text("+"));
                    break;
            }
        }

        public void DecrementStepper()
        {
            switch (App)
            {
                case iOSApp iOSApp:
                    iOSApp.Tap(x => x.Marked("Decrement"));
                    break;

                case AndroidApp androidApp:
                    androidApp.Tap(x => x.Class("android.widget.Button").Text("-"));
                    break;
            }
        }
    }
}
