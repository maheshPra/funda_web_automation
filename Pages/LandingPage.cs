using Microsoft.Playwright;
using System.Threading.Tasks;
using Xunit;

namespace PlaywrightTests.Pages;

public class LandingPage
{
    private readonly IPage _page;

    // Locators
    private const string allesAccepteren = "Alles accepteren";
    private const string searchBoxTestId = "search-box";
    private const string cityOptionText = "Nieuw-Amsterdam Plaats in";
    private const string meldJeAanButtonText = "Meld je aan";


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
    public async Task acceptCookies()
    {
        var cookieButton = _page.GetByRole(AriaRole.Button, new() { Name = allesAccepteren });

        // Check if the button exists and is visible
        if (await cookieButton.IsVisibleAsync())
        {
            await cookieButton.ClickAsync();
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        }
    }

    // Assert key UI elements are visible
    public async Task assertLandingPageUIElementsVisible()
    {
        Assert.True(await _page.GetByTestId(searchBoxTestId).IsVisibleAsync());
        Assert.True(await _page.GetByRole(AriaRole.Button, new() { Name = meldJeAanButtonText, Exact = true }).IsVisibleAsync());
    }

    // Search for a city and select it from the dropdown
    public async Task searchWithLocationFilter(string city)
    {
        await _page.GetByTestId(searchBoxTestId).FillAsync(city);
        await _page.GetByText(cityOptionText).ClickAsync();
    }
}