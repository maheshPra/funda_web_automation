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
        await landingPage.acceptCookies();
        await headerPage.waitForLandingPage();
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
        await landingPage.searchWithLocationFilter(TestData.City);
        await CaptureScreenshotAsync(page, "EnterLocation Screenshot");
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify key UI elements on SearchResultsPage-Header" });
        await headerPage.assertHeaderPageUIElementsDisplayed();
        await CaptureScreenshotAsync(page, "SearchResultsPage-Header Screenshot");
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify that the selected location is displayed in the search box" });
        await searchResultsPage.ensureSelectedLocationIsVisible();
        await CaptureScreenshotAsync(page, "SelectedLocation Screenshot");
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify that search results match the selected location" });
        await searchResultsPage.verifyResultsMatchSelectedLocation();
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Apply price filter" });
        await searchResultsPage.searchWithPriceFilter();
        await CaptureScreenshotAsync(page, "PriceFilterApplied Screenshot");
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify search results respect the applied price filter" });
        await searchResultsPage.verifyResultsMatchedPriceFilter();
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
        await headerPage.clickInloggenButton();
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify navigation to the Login page and validate its content" });
        await headerPage.verifyLoginPageIsDisplayed();
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Login with valid credentials" });
        await headerPage.login("maheshrathnayake13@gmail.com", "Secret@123");
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

    [AllureSuite("Funda SMOKE Tests")]
    [AllureFeature("Homepage")]
    [AllureStory("Verify Login and logout functionality")]
    [Fact]
    public async Task VerifyCardAndDetailsPageInfoDisplayed()
    {
        var page = await CreateFundaPageAsync();
         var (landingPage, searchResultsPage, headerPage) = await InitializePagesAsync(page);

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Navigate to LandingPage" });
        await GoToLandingPage(landingPage, headerPage);
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Search with location filter" });
        await landingPage.searchWithLocationFilter(TestData.City);
        await CaptureScreenshotAsync(page, "EnterLocation Screenshot");
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify that property image exists on the card" });
        await searchResultsPage.verifyPropertyImageExists();
        await CaptureScreenshotAsync(page, "PropertyImageExists Screenshot");
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Ensure the address includes both street name and house number on the Card" });
        await searchResultsPage.verifyStreetNameAndHouseNumber();
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Validate postal code and city format on the card" });
        await searchResultsPage.verifyPostalCodeAndCity();
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Validate property price on the card" });
        await searchResultsPage.verifyPropertyPrice(); 
        AllureLifecycle.Instance.StopStep();


        await page.PauseAsync();    
    }
}