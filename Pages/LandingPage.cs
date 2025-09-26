using Microsoft.Playwright;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PlaywrightTests.Pages;

public class LandingPage
{
    private readonly IPage _page;

    // Locators
    private const string AcceptCookiesButton = "button:has-text('Alles accepteren')";
    private const string SearchBoxTestId = "search-box";
    private const string FundaLink = "a:has-text('Funda')";
    private const string MijnHuisLink = "a:has-text('Mijn Huis')";
    private const string FavorietenLink = "a:has-text('Favorieten')";
    private const string InloggenButton = "button:has-text('Inloggen')";
    private const string MeldJeAanButton = "button:has-text('Meld je aan')";
    private const string CityOptionText = "Nieuw-Amsterdam Plaats in";
    private const string AllesAccepteren = "Alles accepteren";


    // Constructor
    public LandingPage(IPage page)
    {
        _page = page;
    }

    // Methods / Actions
    public async Task GoTo(string url)
    {
        await _page.GotoAsync(url);
    }

    // Accept cookies on the page
    public async Task AcceptCookies()
{
    var cookieButton = _page.GetByRole(AriaRole.Button, new() { Name = AllesAccepteren });

    // Check if the button exists and is visible
    if (await cookieButton.IsVisibleAsync())
    {
        await cookieButton.ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
    }
}

    //search for a city and select it from the dropdown
    public async Task SearchWithLocationFilter(string city)
    {
        await _page.GetByTestId(SearchBoxTestId).FillAsync(city);
        await _page.GetByText(CityOptionText).ClickAsync();
    }
}