using Xamarin.Forms;

namespace XamarinSentryApp
{
    public class App : Application
    {
        public App()
        {
            Device.SetFlags(new[] { "Markup_Experimental" });
            MainPage = new CountPage();
        }
    }
}