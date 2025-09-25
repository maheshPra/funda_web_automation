using Microsoft.Playwright;
using Xunit;

namespace PlaywrightTests;

public class PlaywrightTestBase : IAsyncLifetime
{
    protected IBrowser Browser { get; private set; }
    protected IPlaywright Playwright { get; private set; }
    protected virtual bool Headless => false;

    public async Task InitializeAsync()
    {
        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = Headless
        });
    }

    public async Task DisposeAsync()
    {
        await Browser.CloseAsync();
        Playwright?.Dispose();
    }

    protected async Task<IPage> CreateFundaPageAsync()
{
    var userAgent = Environment.GetEnvironmentVariable("FUNDA_USER_AGENT");
    if (string.IsNullOrEmpty(userAgent))
        throw new InvalidOperationException("FUNDA_USER_AGENT environment variable is not set.");

    var context = await Browser.NewContextAsync(new BrowserNewContextOptions
    {
        UserAgent = userAgent
    });

    return await context.NewPageAsync();
}

}
