using NUnit.Framework;

using Xamarin.UITest;

namespace XamarinSentryApp.UITests
{
	[TestFixture(Platform.Android)]
	[TestFixture(Platform.iOS)]
	abstract class BaseTest
	{
		#region Constant Fields
		readonly Platform _platform;
		#endregion

		#region Constructors
		protected BaseTest(Platform platform) => _platform = platform;
		#endregion

		#region Properties
		protected IApp App { get; private set; }
        protected CountPage CountPage { get; private set; }
		#endregion

		#region Methods
		[SetUp]
        public virtual void BeforeEachTest()
		{
			App = AppInitializer.StartApp(_platform);
			App.Screenshot("App Initialized");

            CountPage = new CountPage(App);

            CountPage.WaitForPageToLoad();
		}
		#endregion
	}
}

