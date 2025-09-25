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

        await fundaPage.GoToAsync();
        await fundaPage.AcceptCookies();
        await fundaPage.WaitForHomePage();
        await fundaPage.SearchCity(TestData.City);
        await page.PauseAsync();
    }
}
