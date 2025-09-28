using Microsoft.Playwright;
using PlaywrightTests.Base;
using PlaywrightTests.Data;
using PlaywrightTests.Pages;
using Xunit;
using Allure.Xunit.Attributes;
using Allure.Net.Commons;

namespace PlaywrightTests.Tests;

[AllureSuite("Funda SMOKE Tests")]
[AllureFeature("Sorting")]
public class SortingTests : PlaywrightTestBase
{
    [AllureStory("Verify sorting functionality in search results page")]
    [Fact]
    public async Task VerifySortingFunctionality()
    {
        var page = await CreateFundaPageAsync();
        var landingPage = new LandingPage(page);
        var searchResultsPage = new SearchResultsPage(page);
        var headerPage = new HeaderPage(page);

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Navigate to LandingPage" });
        await goToLandingPage(landingPage, headerPage);
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Search with location filter" });
        await landingPage.searchWithLocationFilter(TestData.City);
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify that the selected location is displayed in the search box" });
        await searchResultsPage.ensureSelectedLocationIsVisible();
        await CaptureScreenshotAsync(page, "SelectedLocation Screenshot");
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify that search results match the selected location" });
        await searchResultsPage.verifyResultsMatchSelectedLocation();
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify default sorting option" });
        await searchResultsPage.verifySortingDropdownHasDefaultOption();
        await CaptureScreenshotAsync(page, "DefaultSortingOption Screenshot");
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Select sorting option: Price - low to high" });
        await searchResultsPage.selectPriceSorting("Prijs - laag naar hoog");
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify sorting by price low to high" });
        await searchResultsPage.verifySortingByPriceLowToHigh();
        await CaptureScreenshotAsync(page, "PriceLowToHigh Screenshot");
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Select sorting option: Price - high to low" });
        await searchResultsPage.selectPriceSorting("Prijs - hoog naar laag");
        AllureLifecycle.Instance.StopStep();

        AllureLifecycle.Instance.StartStep(new StepResult { name = "Verify sorting by price high to low" });
        await searchResultsPage.verifySortingByPriceHighToLow();
        await CaptureScreenshotAsync(page, "PriceHighToLow Screenshot");
        AllureLifecycle.Instance.StopStep();
    }
}
