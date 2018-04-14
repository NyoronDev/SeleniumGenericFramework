﻿using AC.Contracts;
using AC.Contracts.Pages;
using CL.Containers;
using DF.Entities;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Unity;

namespace US.AcceptanceTests.Steps.Login
{
    /// <summary>
    /// The login steps.
    /// </summary>
    /// <seealso cref="US.AcceptanceTests.Steps.StepBase" />
    [Binding]
    public class LoginSteps : StepBase
    {
        private readonly ILoginPage loginPage;
        private readonly ISetUp setUp;

        public LoginSteps()
        {
            loginPage = AppContainer.Container.Resolve<ILoginPage>();
            setUp = AppContainer.Container.Resolve<ISetUp>();
        }

        /// <summary>
        /// Thes the user goes to the page.
        /// </summary>
        /// <param name="page">The page.</param>
        [Given(@"The user goes to the '(.*)' page")]
        public void TheUserGoesToThePage(string page)
        {
            ScenarioContext.Current.Add("url", page);
            setUp.GoToUrl(page);
        }

        /// <summary>
        /// Thes the user tries to login with the following user.
        /// </summary>
        /// <param name="table">The table.</param>
        [When(@"The user tries to login with the following user")]
        public void TheUserTriesToLoginWithTheFollowingUser(Table table)
        {
            var user = table.CreateInstance<UserLogin>();

            loginPage.LoginUser(user);
        }

        /// <summary>
        /// Thes the user cannot login and the alert appears.
        /// </summary>
        /// <param name="alertMessage">The alert message.</param>
        [Then(@"The user cannot login and the '(.*)' alert appears")]
        public void TheUserCannotLoginAndTheAlertAppears(string alertMessage)
        {
            var realMessage = loginPage.GetAlertBoxMessage();

            realMessage.Should().Be(alertMessage);
        }

        /// <summary>
        /// The after scenario.
        /// </summary>
        [AfterScenario]
        public void AfterScenario()
        {
            try
            {
                if (!setUp.IsDriverNull())
                {
                    if (ScenarioContext.Current.TestError != null)
                    {
                        // Take a screenshot.
                        var screenshotPathFile = setUp.MakeScreenshot(ScenarioContext.Current.ScenarioInfo.Title);
                        InitializeTestContext.Context.AddResultFile(screenshotPathFile);
                    }
                }
            }
            catch
            {
                setUp.CloseDriver();
            }
        }

        /// <summary>
        /// The clean test run.
        /// </summary>
        [AfterTestRun]
        public static void CleanTestRun()
        {
            AppContainer.Container.Resolve<ISetUp>().CloseDriver();
        }
    }
}