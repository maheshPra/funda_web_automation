using Microsoft.Playwright;
using Xunit;
using Allure.Net.Commons;
using System.IO;

namespace PlaywrightTests;

public class PlaywrightTestBase : IAsyncLifetime
{
    protected IBrowser Browser { get; private set; }
    protected IPlaywright Playwright { get; private set; }
    protected virtual bool Headless => false;

    public async Task InitializeAsync()
    {
        Environment.SetEnvironmentVariable("ALLURE_CONFIG", "allureConfig.json");
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
            UserAgent = userAgent,
            ViewportSize = new ViewportSize
            {
                Width = 1920,
                Height = 1080
            }
        });

        var page = await context.NewPageAsync();

        // Optional: maximize window in non-headless mode
        if (!Headless)
            await page.SetViewportSizeAsync(1920, 1080);

        return page;
    }

    protected async Task CaptureScreenshotAsync(IPage page, string name = "screenshot")
    {
        var screenshotBytes = await page.ScreenshotAsync();
        Console.WriteLine("Capturing screenshot and adding to Allure...");
        AllureApi.AddAttachment(name, "image/png", screenshotBytes);
    }
}