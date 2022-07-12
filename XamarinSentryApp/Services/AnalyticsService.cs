using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Sentry;
using Xamarin.Essentials;

namespace XamarinSentryApp
{
    public static class AnalyticsService
    {
        public static void Init()
        {
            SentryXamarin.Init(o =>
            {
                o.AddXamarinFormsIntegration();
                o.DisableXamarinWarningsBreadcrumbs();
                o.StackTraceMode = StackTraceMode.Enhanced;
                o.Dsn = AnalyticsConstants.SentryDsn;
                o.Debug = true;
                o.TracesSampleRate = 1.0;
                o.AttachScreenshots = true;
                o.Release = AppInfo.VersionString;
            });
        }

        [Conditional("DEBUG")]
        public static void CrashApp() => throw new Exception("Auto-Generated Exception");

        public static void TrackEvent(string message, IDictionary<string, string>? data = null) =>
            SentrySdk.AddBreadcrumb(message, data: data);

        public static void TrackEvent(string message, string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key) && string.IsNullOrWhiteSpace(value))
                TrackEvent(message);
            else
                TrackEvent(message, new Dictionary<string, string> { { key, value } });
        }

        public static void SendUserFeedback(string comments)
        {
            var eventId = SentrySdk.CaptureMessage("An event that will receive user feedback.");

            SentrySdk.CaptureUserFeedback(new UserFeedback(eventId, "Anonymous User", "anonymous@user.com", comments));
        }

        public static void Report(Exception exception,
                                    [CallerMemberName] string callerMemberName = "",
                                    [CallerLineNumber] int lineNumber = 0,
                                    [CallerFilePath] string filePath = "")
        {
            PrintException(exception, callerMemberName, lineNumber, filePath);

            SentrySdk.CaptureException(exception);
        }

        [Conditional("DEBUG")]
        static void PrintException(Exception exception, string callerMemberName, int lineNumber, string filePath)
        {
            var fileName = System.IO.Path.GetFileName(filePath);

            Debug.WriteLine(exception.GetType());
            Debug.WriteLine($"Error: {exception.Message}");
            Debug.WriteLine($"Line Number: {lineNumber}");
            Debug.WriteLine($"Caller Name: {callerMemberName}");
            Debug.WriteLine($"File Name: {fileName}");
        }
    }
}
