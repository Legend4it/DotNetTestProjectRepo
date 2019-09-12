
using System.IO;
using System.Windows.Automation;
using NUnit.Framework;
using TestStack.White.Factory;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using Xunit;
using Application = TestStack.White.Application;

namespace UnitTestWinFormWhite
{
    public class UnitTest1
    {

        private static string applicationDirectory;
        private static string applicationPath;
        [Fact]
        public void TestMethod1()
        {
            var window = SetUpWindow();

            Button btn = window.Get<Button>("button1");
            btn.Click();


        }

        private static Window SetUpWindow()
        {
            applicationDirectory = TestContext.CurrentContext.TestDirectory;
            applicationPath = Path.Combine(applicationDirectory, "WinFormApp.exe");
            Application application = Application.Launch(applicationPath);
            Window window = application.GetWindow("Form1", InitializeOption.NoCache);
            return window;
        }

        [Fact]
        public void TestMethod2()
        {
            var window = SetUpWindow();

            SearchCriteria searchCriteria =
                SearchCriteria.ByAutomationId("button1")
                    .AndControlType(ControlType.Button)
                    .AndIndex(0);

            Button btn = (Button)window.Get(searchCriteria);
            btn.Click();

        }
    }
}
