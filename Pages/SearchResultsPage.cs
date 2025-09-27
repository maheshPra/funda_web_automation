using Microsoft.Playwright;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;
using Xunit;

namespace PlaywrightTests.Pages;

public class SearchResultsPage
{
    // Locators 
    private readonly IPage _page;
    private static readonly Regex CityExactRegex = new Regex("^Nieuw-Amsterdam$");
    private const string pageHeaderTestId = "pageHeader";
    private const string priceFromInput = "[id$='price_from']";
    private const string priceToInput = "[id='price_to']";
    private const string minPriceOptionTestId = "FilterRangepriceMin";
    private const string maxPriceOptionTestId = "FilterRangepriceMax";
    private const string minPriceText = "€ 300.000";
    private const string maxPriceText = "€ 500.000";
    private static readonly string PropertyImageContainerLocator = "[class='relative overflow-hidden rounded-md @lg:flex @lg:shrink-0']";
    private const string priceCard = "//div[@class='flex flex-col gap-3 mt-4']//div[contains(text(),'€')]";
    private static readonly string StreetAndHouseNumberLocator = "[class$='flex font-semibold']";
    private static readonly string PostalCodeAndCityLocator = "[class$='truncate text-neutral-80']";
    private static readonly string PropertyPriceLocator = "[class$='flex gap-2']";
    private static readonly Regex StreetAndHouseNumberRegex = new Regex(@"^[A-Za-z\s]+ \d+[A-Za-z]?\s*$");
    private static readonly Regex PostalCodeAndCityRegex = new Regex(@"^\d{4}\s[A-Z]{2}\s.+$");
    private static readonly Regex PropertyPriceRegex = new Regex(@"^€\s\d{1,3}(\.\d{3})*\s(k\.k\.|v\.o\.n\.)$");


    // Constructor
    public SearchResultsPage(IPage page)
    {
        _page = page;
    }

    // Methods / Actions
    // Verify that the selected city is displayed correctly in the search textbox
    public async Task ensureSelectedLocationIsVisible()
    {
        await _page.Locator("div").Filter(new() { HasTextRegex = CityExactRegex }).First.WaitForAsync();

    }

    // Verify that the results match the selected city filter by checking URL and header text
    public async Task verifyResultsMatchSelectedLocation()
    {
        await _page.WaitForURLAsync(url => url.Contains("nieuw-amsterdam"));
        Assert.Contains("nieuw-amsterdam", _page.Url, StringComparison.OrdinalIgnoreCase);

        var selectedText = await _page.Locator("div")
                                      .Filter(new() { HasTextRegex = CityExactRegex })
                                      .First
                                      .InnerTextAsync();
        var headerText = await _page.GetByTestId(pageHeaderTestId).InnerTextAsync();
        Assert.Contains(selectedText, headerText, StringComparison.OrdinalIgnoreCase);
    }

    // Apply a price filter to the search results
    public async Task searchWithPriceFilter()
    {
        // Wait for the page and network to finish
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Click "Price from" and select minimum price
        var priceFrom = _page.Locator(priceFromInput);
        await priceFrom.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
        await priceFrom.ClickAsync();
        await _page.GetByTestId(minPriceOptionTestId).GetByText(minPriceText).ClickAsync();

        // Click "Price to" and select maximum price
        var priceTo = _page.Locator(priceToInput);
        await priceTo.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
        await priceTo.ClickAsync();
        await _page.GetByTestId(maxPriceOptionTestId).GetByText(maxPriceText).ClickAsync();

    }
    // Verify that the results match the applied price filter
    public async Task verifyResultsMatchedPriceFilter()
    {
        // Wait for first price element to ensure results are loaded
        var firstPrice = _page.Locator(priceCard).First;
        await firstPrice.WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Visible,
            Timeout = 60000 // Wait up to 60s in case of slow network
        });
        await _page.WaitForTimeoutAsync(500);

        int minPrice = 300_000;
        int maxPrice = 500_000;

        // Get all the price elements
        var priceElements = await _page.Locator(priceCard).AllAsync();

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
                Assert.Fail($"Failed to parse price: '{priceText}'");
            }
        }

    }

   //Verify that the street name and house no is correct
    public async Task<string> verifyStreetNameAndHouseNumber()
    {
        var streetAndHouseNo = await _page.Locator(StreetAndHouseNumberLocator).First.InnerTextAsync();
        Assert.Matches(StreetAndHouseNumberRegex, streetAndHouseNo);
        return streetAndHouseNo;
    }

    // Verify that the postal code and city format is correct
    public async Task<string> verifyPostalCodeAndCity()
    {
        var postalCodeAndCity = await _page.Locator(PostalCodeAndCityLocator).First.InnerTextAsync();
        Assert.Matches(PostalCodeAndCityRegex, postalCodeAndCity);
        return postalCodeAndCity;
    }

    public async Task<string> verifyPropertyPrice()
    {
        var propertyPrice = await _page.Locator(PropertyPriceLocator).First.InnerTextAsync();
        Assert.Matches(PropertyPriceRegex, propertyPrice);
        return propertyPrice;
    }

// Verify that the property image container exists on the search results page
    public async Task verifyPropertyImageExists()
    {
        var imageLocator = _page.Locator(PropertyImageContainerLocator).Locator("img");
        bool hasImage = await imageLocator.CountAsync() > 0;

        Console.WriteLine(hasImage
            ? "Property image exists."
            : "No property image found.");

        Assert.True(hasImage, "Expected a property image inside the container.");
    }

    // Click the first property card to navigate to the property details page
    public async Task clickFirstPropertyCard()
    {
        var firstPropertyCard = _page.Locator(PropertyImageContainerLocator).First;
        await firstPropertyCard.ClickAsync();
    }
}
