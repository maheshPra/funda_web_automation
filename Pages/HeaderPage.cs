using Microsoft.Playwright;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PlaywrightTests.Pages;

public class HeaderPage
{

    // Locators 
    private readonly IPage _page;
    private const string FundaLinkText = "Funda";
    private const string KopenButtonId = "#headlessui-menu-button-v-0-0-0-21";
    private const string HurenButtonId = "#headlessui-menu-button-v-0-0-0-27";
    private const string VerkopenButtonId = "#headlessui-menu-button-v-0-0-0-30";
    private const string MijnHuisLinkText = "Mijn Huis";
    private const string FavorietenLinkText = "Favorieten";
    private const string InloggenButtonText = "Inloggen";
    private const string SearchBoxTestId = "search-box";
    private const string MeldJeAanButtonText = "Meld je aan";



    // Constructor
    public HeaderPage(IPage page)
    {
        _page = page;
    }

    // Methods / Actions
    // Wait for the home page to load by checking for a key element (Funda link)
    public async Task WaitForLandingPage()
    {
        await _page.GetByRole(AriaRole.Link, new() { Name = FundaLinkText, Exact = true }).WaitForAsync();
    }

    // Assert that key elements are visible on the home page
    public async Task AssertLandingPageUIElementsDisplayed()
    {
        Assert.True(await _page.GetByRole(AriaRole.Link, new() { Name = FundaLinkText, Exact = true }).IsVisibleAsync());
        Assert.True(await _page.Locator(KopenButtonId).IsVisibleAsync());
        Assert.True(await _page.Locator(HurenButtonId).IsVisibleAsync());
        Assert.True(await _page.Locator(VerkopenButtonId).IsVisibleAsync());
        Assert.True(await _page.GetByRole(AriaRole.Link, new() { Name = MijnHuisLinkText, Exact = true }).IsVisibleAsync());
        Assert.True(await _page.GetByRole(AriaRole.Link, new() { Name = FavorietenLinkText, Exact = true }).IsVisibleAsync());
        Assert.True(await _page.GetByRole(AriaRole.Button, new() { Name = InloggenButtonText, Exact = true }).IsVisibleAsync());
        Assert.True(await _page.GetByTestId(SearchBoxTestId).IsVisibleAsync());
        Assert.True(await _page.GetByRole(AriaRole.Button, new() { Name = MeldJeAanButtonText, Exact = true }).IsVisibleAsync());
    }
    public async Task VerifyHeaderButtonsAreClickable()
    {
        // Optionally assert that it's enabled
        Assert.True(await _page.Locator(KopenButtonId).IsEnabledAsync(), "Kopen button should be enabled before clicking.");
        Assert.True(await _page.Locator(HurenButtonId).IsEnabledAsync(), "Huren button should be enabled before clicking.");
        Assert.True(await _page.Locator(VerkopenButtonId).IsEnabledAsync(), "Verkopen button should be enabled before clicking.");

    }


}