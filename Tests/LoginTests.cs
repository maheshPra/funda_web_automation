using Microsoft.Playwright;
using PlaywrightTests.Base;
using PlaywrightTests.Pages;
using Xunit;
using Allure.Xunit.Attributes;
using Allure.Net.Commons;

namespace PlaywrightTests.Tests;

[AllureSuite("Funda SMOKE Tests")]
[AllureFeature("Login and Logout")]
public class LoginTests : PlaywrightTestBase
{
    [AllureStory("Verify Login and logout functionality")]
    [Fact]
    public async Task LoginAndLogout()
    {
        var page = await CreateFundaPageAsync();
        var landingPage = new LandingPage(page);
        var headerPage = new HeaderPage(page);
        var loginPage = new LoginPage(page);

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Navigate to LandingPage" });
        await goToLandingPage(landingPage, headerPage);
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Click Inloggen button" });
        await headerPage.clickInloggenButton();
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify navigation to the Login page and validate its content" });
        await loginPage.verifyLoginPageIsDisplayed();
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Login with valid credentials" });
        await loginPage.login("maheshrathnayake13@gmail.com", "Secret@123");
        await CaptureScreenshotAsync(page, "LoggedIn Screenshot");
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify success login" });
        await headerPage.verifySuccessfulLogin();
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Logout" });
        await headerPage.clickLogoutButton();
        await CaptureScreenshotAsync(page, "Logout Screenshot");
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify successful logout" });
        await headerPage.verifySuccessfulLogout();
        await CaptureScreenshotAsync(page, "LoggedOut Screenshot");
        AllureLifecycle.Instance.StopStep();
    }
}
