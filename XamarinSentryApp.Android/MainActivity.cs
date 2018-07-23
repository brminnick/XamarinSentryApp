using Android.App;
using Android.Content.PM;
using Android.OS;

namespace XamarinSentryApp.Droid
{
    [Activity(Label = "XamarinSentryApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        #if DEBUG
        [Android.Runtime.Preserve]
        [Java.Interop.Export(nameof(SetStepperValue))]
        public void SetStepperValue(int stepperValue)
        {
            var countPage = Xamarin.Forms.Application.Current.MainPage as CountPage;

            countPage.Stepper.Value = stepperValue;
            countPage.CountLabel.Text = stepperValue.ToString();
        }
#endif

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
    }
}

