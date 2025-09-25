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
    private const string FundaLinkText = "Funda";
    private const string MijnHuisLinkText = "Mijn Huis";
    private const string FavorietenLinkText = "Favorieten";
    private const string InloggenButtonText = "Inloggen";
    private const string MeldJeAanButtonText = "Meld je aan";



    // Constructor
    public FundaPage(IPage page)
    {
        _page = page;
    }

    // Methods / Actions
    public async Task GoToAsync(string url)
    {
        await _page.GotoAsync(url);
    }
    // Accept cookies on the page
    public async Task AcceptCookies()
    {
        await _page.GetByRole(AriaRole.Button, new() { Name = "Alles accepteren" }).ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    // Wait for the home page to load by checking for a key element (Funda link)
    public async Task WaitForHomePage()
    {
        await _page.GetByRole(AriaRole.Link, new() { Name = FundaLinkText, Exact = true }).WaitForAsync();
    }
    // Assert that key elements are visible on the home page
    public async Task AssertHomePageElementsVisible()
    {
        Assert.True(await _page.GetByRole(AriaRole.Link, new() { Name = FundaLinkText, Exact = true }).IsVisibleAsync());
        Assert.True(await _page.GetByRole(AriaRole.Link, new() { Name = MijnHuisLinkText, Exact = true }).IsVisibleAsync());
        Assert.True(await _page.GetByRole(AriaRole.Link, new() { Name = FavorietenLinkText, Exact = true }).IsVisibleAsync());
        Assert.True(await _page.GetByRole(AriaRole.Button, new() { Name = InloggenButtonText, Exact = true }).IsVisibleAsync());
        Assert.True(await _page.GetByRole(AriaRole.Button, new() { Name = MeldJeAanButtonText, Exact = true }).IsVisibleAsync());
    }
    //search for a city and select it from the dropdown
    public async Task SearchCity(string city)
    {
        await _page.GetByTestId(SearchBoxTestId).FillAsync(city);
        await _page.GetByText(CityOptionText).ClickAsync();
    }

    // Verify that the selected city is displayed correctly in the header and results
    public async Task VerifyCitySelectionAndResults()
    {
        await _page.Locator("div").Filter(new() { HasTextRegex = CityExactRegex }).First.WaitForAsync();
        await _page.GetByTestId(PageHeaderTestId).GetByText("in Nieuw-Amsterdam").WaitForAsync();
    }
}