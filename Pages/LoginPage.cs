using Microsoft.Playwright;
using System.Threading.Tasks;
using Xunit;

namespace PlaywrightTests.Pages;

public class LoginPage
{
    private readonly IPage _page;


    // Locators
    private const string emailTextboxName = "E-mailadres";
    private const string passwordTextboxName = "Wachtwoord";
    private const string loginSubmitButtonText = "Log in";
    private const string emailLabelText = "E-mailadres";
    private const string passwordLabelText = "Wachtwoord";


    // Constructor
    public LoginPage(IPage page)
    {
        _page = page;
    }


    // Methods / Actions

    // Verify Login Page details
    public async Task verifyLoginPageIsDisplayed()
    {
        await _page.GetByRole(AriaRole.Heading, new() { Name = HeaderPage.loginHeadingText }).IsVisibleAsync();
        await _page.GetByText(emailLabelText).IsVisibleAsync();
        await _page.GetByText(passwordLabelText, new() { Exact = true }).IsVisibleAsync();
    }

    // Enter username, password and login
    public async Task login(string email, string password)
    {
        await _page.GetByRole(AriaRole.Textbox, new() { Name = emailTextboxName }).FillAsync(email);
        await _page.GetByRole(AriaRole.Textbox, new() { Name = passwordTextboxName }).FillAsync(password);
        await _page.GetByRole(AriaRole.Button, new() { Name = loginSubmitButtonText }).ClickAsync();
    }

    
     

}
