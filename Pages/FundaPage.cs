using Microsoft.Playwright;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PlaywrightTests.Pages;

public class FundaPage
{
    private readonly IPage _page;

    // Locators / Selectors
    private const string AcceptCookiesButton = "button:has-text('Alles accepteren')";
    private const string SearchBoxTestId = "search-box";
    private const string PageHeaderTestId = "pageHeader";

    private const string FundaLink = "a:has-text('Funda')";
    private const string MijnHuisLink = "a:has-text('Mijn Huis')";
    private const string FavorietenLink = "a:has-text('Favorieten')";
    private const string InloggenButton = "button:has-text('Inloggen')";
    private const string MeldJeAanButton = "button:has-text('Meld je aan')";
    private const string CityOptionText = "Nieuw-Amsterdam Plaats in";
    private static readonly Regex CityExactRegex = new Regex("^Nieuw-Amsterdam$");

    
    // Constructor
    public FundaPage(IPage page)
    {
        _page = page;
    }

   
    // Methods / Actions
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
        await _page.GetByRole(AriaRole.Link, new() { Name = "Favorieten", Exact = true }).WaitForAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "Inloggen", Exact = true }).WaitForAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "Meld je aan", Exact = true }).WaitForAsync();
    }

    public async Task SearchCity(string city)
    {
        await _page.GetByTestId(SearchBoxTestId).FillAsync(city);
        await _page.GetByText(CityOptionText).ClickAsync();
        await _page.Locator("div").Filter(new() { HasTextRegex = CityExactRegex }).First.WaitForAsync();
        await _page.GetByTestId(PageHeaderTestId).GetByText("in Nieuw-Amsterdam").WaitForAsync();
    }
}
