﻿using Xamarin.Forms;

namespace XamarinSentryApp
{
    public class App : Application
    {
        public App()
        {
            AnalyticsService.Init();
            MainPage = new CountPage();
        }
    }
}