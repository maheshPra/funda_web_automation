using Microsoft.Playwright;
using PlaywrightTests.Data;
using PlaywrightTests.Pages;
using Xunit;

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

    private async Task GoToLandingPageAsync(LandingPage landingPage, HeaderPage headerPage)
    {
        await landingPage.GoTo("https://www.funda.nl/");
        await landingPage.AcceptCookies();
        await headerPage.WaitForLandingPage();
    }

    [Fact]
    public async Task CheckHomePageLoadAndNavigation()
    {
        var page = await CreateFundaPageAsync();
        var (landingPage, _, headerPage) = await InitializePagesAsync(page);

        await GoToLandingPageAsync(landingPage, headerPage);
        await headerPage.AssertLandingPageUIElementsDisplayed();
        await headerPage.VerifyHeaderButtonsAreClickable();
    }

    [Fact]
    public async Task VerifySearchAndFiltering()
    {
        var page = await CreateFundaPageAsync();
        var (landingPage, searchResultsPage, headerPage) = await InitializePagesAsync(page);

        await GoToLandingPageAsync(landingPage, headerPage);
        await landingPage.SearchWithLocationFilter(TestData.City);
        await searchResultsPage.EnsureSelectedLocationIsVisible();
        await searchResultsPage.VerifyResultsMatchSelectedLocation();
        await searchResultsPage.SearchWithPriceFilter();
        await searchResultsPage.VerifyResultsMatchedPriceFilter();
    }

    [Fact]
    public async Task LoginAndLogout()
    {
        var page = await CreateFundaPageAsync();
        var (landingPage, _, headerPage) = await InitializePagesAsync(page);

        await GoToLandingPageAsync(landingPage, headerPage);
        await headerPage.ClickInloggenButton();
        await headerPage.Login("maheshrathnayake13@gmail.com", "Secret@123");
     
    }

    // [Fact]
    // public async Task VerifyCardAndDetailsPageInfoDisplayed()
    // {
    //     var page = await CreateFundaPageAsync();
    //     var (landingPage, searchResultsPage, headerPage) = await InitializePagesAsync(page);
    //     await GoToLandingPageAsync(landingPage, headerPage);
    //     await landingPage.SearchWithLocationFilter(TestData.City);
    //     await page.PauseAsync();
    // }

}