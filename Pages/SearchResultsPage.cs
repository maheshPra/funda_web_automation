using Microsoft.Playwright;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
    private const string PrijsLabelText = "Prijs";

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

    // Verify that the results match the selected city filter
    public async Task VerifyResultsMatchSelectedLocation()
    {
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


}