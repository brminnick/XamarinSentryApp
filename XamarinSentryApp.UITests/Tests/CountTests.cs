using NUnit.Framework;

using Xamarin.UITest;

namespace XamarinSentryApp.UITests
{
    class CountTests : BaseTest
    {
        public CountTests(Platform platform) : base(platform)
        {

        }

        [TestCase(10)]
        [TestCase(5)]
        [TestCase(0)]
        public void IncrementStepper(int incrementCount)
        {
            //Arrange

            //Act
            for (int i = 0; i < incrementCount; i++)
                CountPage.IncrementStepper();

            //Assert
            Assert.AreEqual(incrementCount, double.Parse(CountPage.CountLabelText));
        }

        [TestCase(10, 10)]
        [TestCase(5, 5)]
        [TestCase(0, 5)]
        public void DecrementStepper(int decrementCount, int stepperStartValue)
        {
            //Arrange
            CountPage.SetStepperValue(stepperStartValue);

            //Act
            for (int i = 0; i < decrementCount; i++)
                CountPage.DecrementStepper();

            //Assert
            Assert.AreEqual(stepperStartValue - decrementCount, double.Parse(CountPage.CountLabelText));
        }
    }
}
