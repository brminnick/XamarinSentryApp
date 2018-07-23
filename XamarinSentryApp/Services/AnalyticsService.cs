using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using SharpRaven;

namespace XamarinSentryApp
{
    public static class AnalyticsService
    {
        #region Constant Fields
        readonly static Lazy<RavenClient> _ravenClientHolder = new Lazy<RavenClient>(() => new RavenClient(AnalyticsConstants.RavenDsn));
        #endregion

        #region Properties
        static RavenClient RavenClient => _ravenClientHolder.Value;
        #endregion

        #region Methods
        [Conditional("DEBUG")]
        public static void CrashApp() => throw new Exception("Auto-Generated Exception");

        public static void TrackEvent(string trackIdentifier, IDictionary<string, string> table = null) =>
            RavenClient.AddTrail(new SharpRaven.Data.Breadcrumb(trackIdentifier) { Data = table });

        public static void TrackEvent(string trackIdentifier, string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key) && string.IsNullOrWhiteSpace(value))
                TrackEvent(trackIdentifier);
            else
                TrackEvent(trackIdentifier, new Dictionary<string, string> { { key, value } });
        }

        public static void Report(Exception exception,
                                  [CallerMemberName] string callerMemberName = "",
                                  [CallerLineNumber] int lineNumber = 0,
                                  [CallerFilePath] string filePath = "")
        {
            PrintException(exception, callerMemberName, lineNumber, filePath);

            RavenClient.Capture(new SharpRaven.Data.SentryEvent(exception));
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
        #endregion
    }
}
