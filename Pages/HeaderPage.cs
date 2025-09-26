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
    private const string LoginHeadingText = "Log in";
    private const string EmailTextboxName = "E-mailadres";
    private const string PasswordTextboxName = "Wachtwoord";
    private const string LoginSubmitButtonText = "Log in";





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

    public async Task ClickInloggenButton()
    {
        var inloggenButton = _page.Locator($"button:has-text('{InloggenButtonText}')").First;

        // Wait until button is visible and enabled
        await inloggenButton.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 15000 });

        // Hover (optional, for menus)
        await inloggenButton.HoverAsync();

        // Click to trigger modal
        await inloggenButton.ClickAsync();

        // Wait for modal to appear
        await _page.GetByRole(AriaRole.Heading, new() { Name = LoginHeadingText })
                   .WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 15000 });
    }
    
    public async Task Login(string email, string password)
    {
        await _page.GetByRole(AriaRole.Textbox, new() { Name = EmailTextboxName }).FillAsync(email);
        await _page.GetByRole(AriaRole.Textbox, new() { Name = PasswordTextboxName }).FillAsync(password);
        await _page.GetByRole(AriaRole.Button, new() { Name = LoginSubmitButtonText }).ClickAsync();
    }



    


}