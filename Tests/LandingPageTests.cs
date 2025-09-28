using Microsoft.Playwright;
using PlaywrightTests.Base;
using PlaywrightTests.Pages;
using Xunit;
using Allure.Xunit.Attributes;
using Allure.Net.Commons;

namespace PlaywrightTests.Tests;

[AllureSuite("Funda SMOKE Tests")]
[AllureFeature("LandingPage")]
public class LandingPageTests : PlaywrightTestBase
{
    [AllureStory("Check LandingPage load and navigation")]
    [Fact]
    public async Task CheckLandingPageLoadAndNavigation()
    {
        var page = await CreateFundaPageAsync();
        var landingPage = new LandingPage(page);
        var headerPage = new HeaderPage(page);

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Navigate to LandingPage" });
        await goToLandingPage(landingPage, headerPage);
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify key UI elements on LandingPage-Header" });
        await headerPage.assertHeaderPageUIElementsDisplayed();
        await CaptureScreenshotAsync(page, "LandingPage-header Screenshot");
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify header buttons are clickable" });
        await headerPage.verifyHeaderButtonsAreClickable();
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify key UI elements on LandingPage-Body" });
        await landingPage.assertLandingPageUIElementsVisible();
        await CaptureScreenshotAsync(page, "LandingPage-body Screenshot");
        AllureLifecycle.Instance.StopStep();
    }
}
