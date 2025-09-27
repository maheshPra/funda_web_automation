using Microsoft.Playwright;
using System.Threading.Tasks;
using Xunit;

namespace PlaywrightTests.Pages;

public class HeaderPage
{
    // Locators
    private readonly IPage _page;

    // Header elements
    private const string FundaLinkText = "Funda";
    private const string KopenButtonId = "#headlessui-menu-button-v-0-0-0-21";
    private const string HurenButtonId = "#headlessui-menu-button-v-0-0-0-27";
    private const string VerkopenButtonId = "#headlessui-menu-button-v-0-0-0-30";
    private const string MijnHuisLinkText = "Mijn Huis";
    private const string FavorietenLinkText = "Favorieten";
    private const string InloggenButtonText = "Inloggen";
    private const string LoggenButtonText = "Account";
    private const string AccountButtonId = "#headlessui-menu-button-v-0-1-0-3";
    private const string LoginHeadingText = "Log in";
    private const string LogoutMenuId = "#headlessui-menu-items-v-0-1-0-4";
    private const string LogoutButtonText = "Uitloggen";

    // Login page elements (should go to new Page file)
    private const string EmailTextboxName = "E-mailadres";
    private const string PasswordTextboxName = "Wachtwoord";
    private const string LoginSubmitButtonText = "Log in";
    private const string EmailLabelText = "E-mailadres";
    private const string PasswordLabelText = "Wachtwoord";

    // Constructor
    public HeaderPage(IPage page)
    {
        _page = page;
    }

    // Wait for the home page to load by checking for a key element (Funda link)
    public async Task WaitForLandingPage()
    {
        await _page.GetByRole(AriaRole.Link, new() { Name = FundaLinkText, Exact = true }).WaitForAsync();
    }

    // Assert that key elements are visible on the home page
    public async Task AssertHeaderPageUIElementsDisplayed()
    {
        Assert.True(await _page.GetByRole(AriaRole.Link, new() { Name = FundaLinkText, Exact = true }).IsVisibleAsync());
        Assert.True(await _page.Locator(KopenButtonId).IsVisibleAsync());
        Assert.True(await _page.Locator(HurenButtonId).IsVisibleAsync());
        Assert.True(await _page.Locator(VerkopenButtonId).IsVisibleAsync());
        Assert.True(await _page.GetByRole(AriaRole.Link, new() { Name = MijnHuisLinkText, Exact = true }).IsVisibleAsync());
        Assert.True(await _page.GetByRole(AriaRole.Link, new() { Name = FavorietenLinkText, Exact = true }).IsVisibleAsync());
        Assert.True(await _page.GetByRole(AriaRole.Button, new() { Name = InloggenButtonText, Exact = true }).IsVisibleAsync());
    }

    // Verify that header buttons are clickable
    public async Task VerifyHeaderButtonsAreClickable()
    {
        Assert.True(await _page.Locator(KopenButtonId).IsEnabledAsync(), "Kopen button should be enabled before clicking.");
        Assert.True(await _page.Locator(HurenButtonId).IsEnabledAsync(), "Huren button should be enabled before clicking.");
        Assert.True(await _page.Locator(VerkopenButtonId).IsEnabledAsync(), "Verkopen button should be enabled before clicking.");
    }

    public async Task ClickInloggenButton()
    {
        var inloggenButton = _page.Locator($"button:has-text('{InloggenButtonText}')").First;
        await inloggenButton.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 15000 });
        await inloggenButton.HoverAsync();
        await inloggenButton.ClickAsync();
        await _page.GetByRole(AriaRole.Heading, new() { Name = LoginHeadingText })
                   .WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 15000 });
    }

    // Verify the button label after logging in with a valid user.
    public async Task VerifySuccessfulLogin()
    {
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        var accountButton = _page.Locator(AccountButtonId);
        await accountButton.WaitForAsync(new() { State = WaitForSelectorState.Visible });
        Assert.Equal(LoggenButtonText, await accountButton.InnerTextAsync());
    }

    // Click the logout button from the account dropdown menu
    public async Task ClickLogoutButton()
    {
        var accountButton = _page.Locator(AccountButtonId);
        await accountButton.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 15000 });
        await accountButton.HoverAsync();
        await accountButton.ClickAsync();
        var logoutMenu = _page.Locator(LogoutMenuId);
        await logoutMenu.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 15000 });
        await logoutMenu.Locator($"text={LogoutButtonText}").ClickAsync();
    }

    // Verify successful logout
    public async Task VerifySuccessfulLogout()
    {
        Assert.True(await _page.GetByRole(AriaRole.Button, new() { Name = InloggenButtonText, Exact = true }).IsVisibleAsync());
    }

    ////////// Should go to new Page file //////////

    // Verify Login Page details
    public async Task VerifyLoginPageIsDisplayed()
    {
        await _page.GetByRole(AriaRole.Heading, new() { Name = LoginHeadingText }).IsVisibleAsync();
        await _page.GetByText(EmailLabelText).IsVisibleAsync();
        await _page.GetByText(PasswordLabelText, new() { Exact = true }).IsVisibleAsync();
    }

    // Enter username, password and login
    public async Task Login(string email, string password)
    {
        await _page.GetByRole(AriaRole.Textbox, new() { Name = EmailTextboxName }).FillAsync(email);
        await _page.GetByRole(AriaRole.Textbox, new() { Name = PasswordTextboxName }).FillAsync(password);
        await _page.GetByRole(AriaRole.Button, new() { Name = LoginSubmitButtonText }).ClickAsync();
    }
}