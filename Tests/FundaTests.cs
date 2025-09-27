using Microsoft.Playwright;
using PlaywrightTests.Data;
using PlaywrightTests.Pages;
using Xunit;
using Allure.Xunit.Attributes;
using Allure.Net.Commons;

namespace PlaywrightTests.Tests;

public class FundaTests : PlaywrightTestBase
{
    private async Task<(LandingPage landingPage, SearchResultsPage searchResultsPage, HeaderPage headerPage)> InitializePagesAsync(IPage page)
    {
        var landingPage = new LandingPage(page);
        var searchResultsPage = new SearchResultsPage(page);
        var headerPage = new HeaderPage(page);
        return (landingPage, searchResultsPage, headerPage);
    }

    private async Task GoToLandingPage(LandingPage landingPage, HeaderPage headerPage)
    {
        await landingPage.GoTo("https://www.funda.nl/");
        await landingPage.AcceptCookies();
        await headerPage.WaitForLandingPage();
    }

    [AllureSuite("Funda SMOKE Tests")]
    [AllureFeature("Homepage")]
    [AllureStory("Check LandingPage load and navigation")]
    [Fact]
    public async Task CheckLandingPageLoadAndNavigation()
    {
        var page = await CreateFundaPageAsync();
        var (landingPage, _, headerPage) = await InitializePagesAsync(page);

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Navigate to LandingPage" });
        await GoToLandingPage(landingPage, headerPage);
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify key UI elements on LandingPage-Header" });
        await headerPage.AssertHeaderPageUIElementsDisplayed();
        await CaptureScreenshotAsync(page, "LandingPage-header Screenshot");
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify header buttons are clickable" });
        await headerPage.VerifyHeaderButtonsAreClickable();
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify key UI elements on LandingPage-Body" });
        await landingPage.AssertLandingPageUIElementsVisible();
        await CaptureScreenshotAsync(page, "LandingPage-body Screenshot");
        AllureLifecycle.Instance.StopStep();
    }

    [AllureSuite("Funda SMOKE Tests")]
    [AllureFeature("Homepage")]
    [AllureStory("Verify search and filtering functionality")]
    [Fact]
    public async Task VerifySearchAndFiltering()
    {
        var page = await CreateFundaPageAsync();
        var (landingPage, searchResultsPage, headerPage) = await InitializePagesAsync(page);


        AllureLifecycle.Instance.StartStep(new StepResult { name = "Navigate to LandingPage" });
        await GoToLandingPage(landingPage, headerPage);
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Search with location filter" });
        await landingPage.SearchWithLocationFilter(TestData.City);
        await CaptureScreenshotAsync(page, "EnterLocation Screenshot");
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify key UI elements on SearchResultsPage-Header" });
        await headerPage.AssertHeaderPageUIElementsDisplayed();
        await CaptureScreenshotAsync(page, "SearchResultsPage-Header Screenshot");
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify that the selected location is displayed in the search box" });
        await searchResultsPage.EnsureSelectedLocationIsVisible();
        await CaptureScreenshotAsync(page, "SelectedLocation Screenshot");
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify that search results match the selected location" });
        await searchResultsPage.VerifyResultsMatchSelectedLocation();
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Apply price filter" });
        await searchResultsPage.SearchWithPriceFilter();
        await CaptureScreenshotAsync(page, "PriceFilterApplied Screenshot");
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify search results respect the applied price filter" });
        await searchResultsPage.VerifyResultsMatchedPriceFilter();
        AllureLifecycle.Instance.StopStep();
    }

    [AllureSuite("Funda SMOKE Tests")]
    [AllureFeature("Homepage")]
    [AllureStory("Verify Login and logout functionality")]
    [Fact]
    public async Task LoginAndLogout()
    {
        var page = await CreateFundaPageAsync();
        var (landingPage, _, headerPage) = await InitializePagesAsync(page);

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Navigate to LandingPage" });
        await GoToLandingPage(landingPage, headerPage);
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Click Inloggen button" });
        await headerPage.ClickInloggenButton();
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify navigation to the Login page and validate its content" });
        await headerPage.VerifyLoginPageIsDisplayed();
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Login with valid credentials" });
        await headerPage.Login("maheshrathnayake13@gmail.com", "Secret@123");
        await CaptureScreenshotAsync(page, "LoggedIn Screenshot");
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify success login" });
        await headerPage.VerifySuccessfulLogin();
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Logout" });
        await headerPage.ClickLogoutButton();
        await CaptureScreenshotAsync(page, "Logout Screenshot");
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify successful logout" });
        await headerPage.VerifySuccessfulLogout();
        await CaptureScreenshotAsync(page, "LoggedOut Screenshot");
        AllureLifecycle.Instance.StopStep();
    }
}