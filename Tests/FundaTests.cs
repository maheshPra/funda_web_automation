using Microsoft.Playwright;
using PlaywrightTests.Data;
using PlaywrightTests.Pages;
using Xunit;

namespace PlaywrightTests.Tests;

public class FundaTests : PlaywrightTestBase
{
    [Fact]
    public async Task SearchACity()
    {
        var page = await CreateFundaPageAsync();
        var fundaPage = new FundaPage(page);

        await fundaPage.GoToAsync("https://www.funda.nl/");
        await fundaPage.AcceptCookies();
        await fundaPage.WaitForHomePage();
        await fundaPage.AssertHomePageElementsVisible();
        await fundaPage.SearchCity(TestData.City);
        await fundaPage.VerifyCitySelectionAndResults();
        await page.PauseAsync();
    }
}