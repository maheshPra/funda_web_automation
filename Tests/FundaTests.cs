using Microsoft.Playwright;
using PlaywrightTests.Data;
using PlaywrightTests.Pages;
using Xunit;
using Allure.Xunit.Attributes;
using Allure.Net.Commons;

namespace PlaywrightTests.Tests;

public class FundaTests : PlaywrightTestBase
{
    private async Task<(LandingPage landingPage, SearchResultsPage searchResultsPage, HeaderPage headerPage, LoginPage loginPage, PropertyDetailsPage propertyDetailsPage)> InitializePagesAsync(IPage page)
    {
        var landingPage = new LandingPage(page);
        var searchResultsPage = new SearchResultsPage(page);
        var headerPage = new HeaderPage(page);
        var loginPage = new LoginPage(page);
        var propertyDetailsPage = new PropertyDetailsPage(page);
        return (landingPage, searchResultsPage, headerPage, loginPage, propertyDetailsPage);
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
        var (landingPage, _, headerPage, _, _) = await InitializePagesAsync(page);

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
        var (landingPage, searchResultsPage, headerPage, _, _) = await InitializePagesAsync(page);


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
        var (landingPage, _, headerPage, loginPage, _) = await InitializePagesAsync(page);

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Navigate to LandingPage" });
        await GoToLandingPage(landingPage, headerPage);
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

    [AllureSuite("Funda SMOKE Tests")]
    [AllureFeature("Homepage")]
    [AllureStory("Verify property card and details page information")]
    [Fact]
    public async Task VerifyCardAndDetailsPageInfoDisplayed()
    {
        var page = await CreateFundaPageAsync();
        var (landingPage, searchResultsPage, headerPage, _, propertyDetailsPage) = await InitializePagesAsync(page);

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
        var streetAndHouseNo = await searchResultsPage.verifyStreetNameAndHouseNumber();
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Validate postal code and city format on the card" });
        var postalCodeAndCity = await searchResultsPage.verifyPostalCodeAndCity();
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Validate property price on the card" });
        var propertyPrice = await searchResultsPage.verifyPropertyPrice();
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Click the first property card to navigate to the details page" });
        await searchResultsPage.clickFirstPropertyCard();
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify navigation to the property details page" });
        await propertyDetailsPage.verifyNavigationToPropertyDetailsPage(streetAndHouseNo);
        await CaptureScreenshotAsync(page, "PropertyDetailsPage Screenshot");
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify that the street name and house number on the property details page match the values from the card." });
        await propertyDetailsPage.verifyPropertyDetailsMatchCardStreetAndHouseNumber(streetAndHouseNo);
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify that the postal code and city on the property details page match the values from the card." });
        await propertyDetailsPage.verifyPropertyDetailsMatchCardpostalCodeAndCity(postalCodeAndCity);
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify that the property price on the property details page matches the value from the card." });
        await propertyDetailsPage.verifyPropertyDetailsMatchCardPropertyPrice(propertyPrice);
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Check that the property image is displayed on the property details page" });
        await propertyDetailsPage.checkPropertyImageOnPropertyDetails();
        AllureLifecycle.Instance.StopStep();

        




      
    }
}