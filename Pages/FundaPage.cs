using Microsoft.Playwright;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PlaywrightTests.Pages;

public class FundaPage
{
    private readonly IPage _page;

    public FundaPage(IPage page)
    {
        _page = page;
    }

    public async Task GoToAsync()
    {
        await _page.GotoAsync("https://www.funda.nl/");
    }

    public async Task AcceptCookies()
    {
        await _page.GetByRole(AriaRole.Button, new() { Name = "Alles accepteren" }).ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public async Task WaitForHomePage()
    {
        await _page.GetByRole(AriaRole.Link, new() { Name = "Funda", Exact = true }).WaitForAsync();
        await _page.GetByRole(AriaRole.Link, new() { Name = "Mijn Huis", Exact = true }).WaitForAsync();
        await _page.GetByRole(AriaRole.Link, new() { Name = "Favorieten" }).WaitForAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "Inloggen" }).WaitForAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "Meld je aan" }).WaitForAsync();
    }

    public async Task SearchCity(string city)
    {
        await _page.GetByTestId("search-box").FillAsync(city);
        await _page.GetByText("Nieuw-Amsterdam Plaats in").ClickAsync();
        await _page.Locator("div").Filter(new() { HasTextRegex = new Regex("^Nieuw-Amsterdam$") }).First.WaitForAsync();
        await _page.GetByTestId("pageHeader").GetByText("in Nieuw-Amsterdam").WaitForAsync();
    }
}
