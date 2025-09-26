using Microsoft.Playwright;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;

namespace PlaywrightTests.Pages;

public class SearchResultsPage
{
    // Locators 
    private readonly IPage _page;

    private static readonly Regex CityExactRegex = new Regex("^Nieuw-Amsterdam$");
    private const string PageHeaderTestId = "pageHeader";
    private const string PriceFromInput = "[id$='price_from']";
    private const string PriceToInput = "[id='price_to']";
    private const string MinPriceOptionTestId = "FilterRangepriceMin";
    private const string MaxPriceOptionTestId = "FilterRangepriceMax";
    private const string MinPriceText = "€ 300.000";
    private const string MaxPriceText = "€ 500.000";
    private const string PriceCardContainer = "//div[@class='flex flex-col gap-3 mt-4']";
    private const string PriceCard = "//div[@class='flex flex-col gap-3 mt-4']//div[contains(text(),'€')]";

    // Constructor
    public SearchResultsPage(IPage page)
    {
        _page = page;
    }

    // Methods / Actions
    // Verify that the selected city is displayed correctly in the search textbox
    public async Task EnsureSelectedLocationIsVisible()
    {
        await _page.Locator("div").Filter(new() { HasTextRegex = CityExactRegex }).First.WaitForAsync();

    }

    // Verify that the results match the selected city filter by checking URL and header text
    public async Task VerifyResultsMatchSelectedLocation()
    {
        await _page.WaitForURLAsync(url => url.Contains("nieuw-amsterdam"));
        Assert.Contains("nieuw-amsterdam", _page.Url, StringComparison.OrdinalIgnoreCase);

        var selectedText = await _page.Locator("div")
                                      .Filter(new() { HasTextRegex = CityExactRegex })
                                      .First
                                      .InnerTextAsync();
        var headerText = await _page.GetByTestId(PageHeaderTestId).InnerTextAsync();
        Assert.Contains(selectedText, headerText, StringComparison.OrdinalIgnoreCase);
    }

    // Apply a price filter to the search results
    public async Task SearchWithPriceFilter()
    {
        // Wait for the page and network to finish
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Click "Price from" and select minimum price
        var priceFrom = _page.Locator(PriceFromInput);
        await priceFrom.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
        await priceFrom.ClickAsync();
        await _page.GetByTestId(MinPriceOptionTestId).GetByText(MinPriceText).ClickAsync();

        // Click "Price to" and select maximum price
        var priceTo = _page.Locator(PriceToInput);
        await priceTo.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
        await priceTo.ClickAsync();
        await _page.GetByTestId(MaxPriceOptionTestId).GetByText(MaxPriceText).ClickAsync();

    }
    // Verify that the results match the applied price filter
    public async Task VerifyResultsMatchedPriceFilter()
    {
        // Wait for first price element to ensure results are loaded
        var firstPrice = _page.Locator(PriceCard).First;
        await firstPrice.WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Visible,
            Timeout = 60000 // Wait up to 60s in case of slow network
        });
        await _page.WaitForTimeoutAsync(500);

        // var text = await _page.Locator(PriceCardContainer).InnerTextAsync();
        // Console.WriteLine(text);

        int minPrice = 300_000;
        int maxPrice = 500_000;

        // Get all the price elements
        var priceElements = await _page.Locator(PriceCard).AllAsync();

        foreach (var priceElement in priceElements)
        {
            var priceText = await priceElement.InnerTextAsync();

            // Remove currency symbol, whitespace, dots
            var cleanText = priceText
                .Replace("€", "")
                .Replace("k.k.", "")
                .Replace("v.o.n.", "")
                .Trim()
                .Replace(".", "");

            if (int.TryParse(cleanText, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out int priceValue))
            {
                // Assert the price is within the range
                Assert.InRange(priceValue, minPrice, maxPrice);
            }
            else
            {
                //fail if parsing fails
                Assert.True(false, $"Failed to parse price: '{priceText}'");
            }
        }
    }
}