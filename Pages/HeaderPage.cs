using Microsoft.Playwright;
using System.Threading.Tasks;
using Xunit;

namespace PlaywrightTests.Pages;

public class HeaderPage
{
    
    private readonly IPage _page;

   // Locators
    private const string fundaLinkText = "Funda";
    private const string kopenButtonId = "#headlessui-menu-button-v-0-0-0-21";
    private const string hurenButtonId = "#headlessui-menu-button-v-0-0-0-27";
    private const string verkopenButtonId = "#headlessui-menu-button-v-0-0-0-30";
    private const string mijnHuisLinkText = "Mijn Huis";
    private const string favorietenLinkText = "Favorieten";
    private const string inloggenButtonText = "Inloggen";
    private const string loggenButtonText = "Account";
    private const string accountButtonId = "#headlessui-menu-button-v-0-1-0-3";
    public const string loginHeadingText = "Log in";
    private const string logoutMenuId = "#headlessui-menu-items-v-0-1-0-4";
    private const string logoutButtonText = "Uitloggen";

    // Constructor
    public HeaderPage(IPage page)
    {
        _page = page;
    }

    // Wait for the home page to load by checking for a key element (Funda link)
    public async Task waitForLandingPage()
    {
        await _page.GetByRole(AriaRole.Link, new() { Name = fundaLinkText, Exact = true }).WaitForAsync();
    }

    // Assert that key elements are visible on the home page
    public async Task assertHeaderPageUIElementsDisplayed()
    {
        Assert.True(await _page.GetByRole(AriaRole.Link, new() { Name = fundaLinkText, Exact = true }).IsVisibleAsync());
        Assert.True(await _page.Locator(kopenButtonId).IsVisibleAsync());
        Assert.True(await _page.Locator(hurenButtonId).IsVisibleAsync());
        Assert.True(await _page.Locator(verkopenButtonId).IsVisibleAsync());
        Assert.True(await _page.GetByRole(AriaRole.Link, new() { Name = mijnHuisLinkText, Exact = true }).IsVisibleAsync());
        Assert.True(await _page.GetByRole(AriaRole.Link, new() { Name = favorietenLinkText, Exact = true }).IsVisibleAsync());
        Assert.True(await _page.GetByRole(AriaRole.Button, new() { Name = inloggenButtonText, Exact = true }).IsVisibleAsync());
    }

    // Verify that header buttons are clickable
    public async Task verifyHeaderButtonsAreClickable()
    {
        Assert.True(await _page.Locator(kopenButtonId).IsEnabledAsync(), "Kopen button should be enabled before clicking.");
        Assert.True(await _page.Locator(hurenButtonId).IsEnabledAsync(), "Huren button should be enabled before clicking.");
        Assert.True(await _page.Locator(verkopenButtonId).IsEnabledAsync(), "Verkopen button should be enabled before clicking.");
    }

    // Click the Inloggen button to navigate to the login page
    public async Task clickInloggenButton()
    {
        var inloggenButton = _page.Locator($"button:has-text('{inloggenButtonText}')").First;
        await inloggenButton.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 15000 });
        await inloggenButton.HoverAsync();
        await inloggenButton.ClickAsync();
        await _page.GetByRole(AriaRole.Heading, new() { Name = loginHeadingText })
                   .WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 15000 });
    }

    // Verify the button label after logging in with a valid user.
    public async Task verifySuccessfulLogin()
    {
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        var accountButton = _page.Locator(accountButtonId);
        await accountButton.WaitForAsync(new() { State = WaitForSelectorState.Visible });
        Assert.Equal(loggenButtonText, await accountButton.InnerTextAsync());
    }

    // Click the logout button from the account dropdown menu
    public async Task clickLogoutButton()
    {
        var accountButton = _page.Locator(accountButtonId);
        await accountButton.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 15000 });
        await accountButton.HoverAsync();
        await accountButton.ClickAsync();
        var logoutMenu = _page.Locator(logoutMenuId);
        await logoutMenu.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 15000 });
        await logoutMenu.Locator($"text={logoutButtonText}").ClickAsync();
    }

    // Verify successful logout
    public async Task verifySuccessfulLogout()
    {
        Assert.True(await _page.GetByRole(AriaRole.Button, new() { Name = inloggenButtonText, Exact = true }).IsVisibleAsync());
    }
}