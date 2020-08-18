using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using SharpRaven;
using SharpRaven.Data;

using Xamarin.Essentials;

namespace XamarinSentryApp
{
    public static class AnalyticsService
    {
        readonly static Lazy<RavenClient> _ravenClientHolder = new Lazy<RavenClient>(() => new RavenClient(AnalyticsConstants.RavenDsn) { Release = AppInfo.VersionString });

        static RavenClient RavenClient => _ravenClientHolder.Value;

        [Conditional("DEBUG")]
        public static void CrashApp() => throw new Exception("Auto-Generated Exception");

        public static void TrackEvent(string trackIdentifier, IDictionary<string, string>? table = null) =>
            RavenClient.AddTrail(new Breadcrumb(trackIdentifier) { Data = table });

        public static void TrackEvent(string trackIdentifier, string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key) && string.IsNullOrWhiteSpace(value))
                TrackEvent(trackIdentifier);
            else
                TrackEvent(trackIdentifier, new Dictionary<string, string> { { key, value } });
        }

        public static Task SendUserFeedback(string comments)
        {
            var feedback = new SentryUserFeedback
            {
                Comments = comments,
                Name = "Anonymous User",
                Email = "anonymous@user.com"
            };

            return RavenClient.SendUserFeedbackAsync(feedback);
        }

        public static Task Report(Exception exception,
                                    [CallerMemberName] string callerMemberName = "",
                                    [CallerLineNumber] int lineNumber = 0,
                                    [CallerFilePath] string filePath = "")
        {
            PrintException(exception, callerMemberName, lineNumber, filePath);

            return RavenClient.CaptureAsync(new SentryEvent(exception));
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
