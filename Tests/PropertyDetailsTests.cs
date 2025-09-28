using Microsoft.Playwright;
using PlaywrightTests.Base;
using PlaywrightTests.Data;
using PlaywrightTests.Pages;
using Xunit;
using Allure.Xunit.Attributes;
using Allure.Net.Commons;

namespace PlaywrightTests.Tests;

[AllureSuite("Funda SMOKE Tests")]
[AllureFeature("Property Details")]
public class PropertyDetailsTests : PlaywrightTestBase
{
    [AllureStory("Verify property card and details page information")]
    [Fact]
    public async Task VerifyCardAndDetailsPageInfoDisplayed()
    {
        var page = await CreateFundaPageAsync();
        var landingPage = new LandingPage(page);
        var searchResultsPage = new SearchResultsPage(page);
        var headerPage = new HeaderPage(page);
        var propertyDetailsPage = new PropertyDetailsPage(page);

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Navigate to LandingPage" });
        await goToLandingPage(landingPage, headerPage);
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

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify that the street name and house number match" });
        await propertyDetailsPage.verifyPropertyDetailsMatchCardStreetAndHouseNumber(streetAndHouseNo);
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify that the postal code and city match" });
        await propertyDetailsPage.verifyPropertyDetailsMatchCardpostalCodeAndCity(postalCodeAndCity);
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify that the property price matches" });
        await propertyDetailsPage.verifyPropertyDetailsMatchCardPropertyPrice(propertyPrice);
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Check that the property image is displayed" });
        await propertyDetailsPage.checkPropertyImageOnPropertyDetails();
        AllureLifecycle.Instance.StopStep();
    }
}
