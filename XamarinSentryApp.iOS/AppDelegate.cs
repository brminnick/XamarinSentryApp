using Foundation;
using UIKit;

namespace XamarinSentryApp.iOS
{
    [Register(nameof(AppDelegate))]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
#if DEBUG
            Xamarin.Calabash.Start();
#endif

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(uiApplication, launchOptions);
        }

#if DEBUG
        [Preserve]
        [Export("setStepperValue:")]
        public NSString SetStepperValue(NSString stepperValueString)
        {
            var stepperValue = double.Parse(stepperValueString);

            var countPage = Xamarin.Forms.Application.Current.MainPage as CountPage;

            countPage.Stepper.Value = stepperValue;
            countPage.CountLabel.Text = stepperValue.ToString();

            return new NSString();
        }
#endif
    }
}
