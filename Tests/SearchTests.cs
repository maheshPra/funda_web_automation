using Microsoft.Playwright;
using PlaywrightTests.Base;
using PlaywrightTests.Data;
using PlaywrightTests.Pages;
using Xunit;
using Allure.Xunit.Attributes;
using Allure.Net.Commons;

namespace PlaywrightTests.Tests;

[AllureSuite("Funda SMOKE Tests")]
[AllureFeature("Search")]
public class SearchTests : PlaywrightTestBase
{
    [AllureStory("Verify search and filtering functionality")]
    [Fact]
    public async Task VerifySearchAndFiltering()
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
}
