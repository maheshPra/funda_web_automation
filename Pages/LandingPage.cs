using Microsoft.Playwright;
using System.Threading.Tasks;
using Xunit;

namespace PlaywrightTests.Pages;

public class LandingPage
{
    private readonly IPage _page;

    // Locators
    private const string AllesAccepteren = "Alles accepteren";
    private const string SearchBoxTestId = "search-box";
    private const string CityOptionText = "Nieuw-Amsterdam Plaats in";
    private const string MeldJeAanButtonText = "Meld je aan";


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

    // Assert key UI elements are visible
    public async Task AssertLandingPageUIElementsVisible()
    {
        Assert.True(await _page.GetByTestId(SearchBoxTestId).IsVisibleAsync());
        Assert.True(await _page.GetByRole(AriaRole.Button, new() { Name = MeldJeAanButtonText, Exact = true }).IsVisibleAsync());
    }

    // Search for a city and select it from the dropdown
    public async Task SearchWithLocationFilter(string city)
    {
        await _page.GetByTestId(SearchBoxTestId).FillAsync(city);
        await _page.GetByText(CityOptionText).ClickAsync();
    }
}