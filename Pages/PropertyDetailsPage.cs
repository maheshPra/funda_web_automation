using Microsoft.Playwright;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace PlaywrightTests.Pages;

public class PropertyDetailsPage
{
    private readonly IPage _page;


    // Locators
    private const string StreetAndHouseNumberLocator = "[class='block text-2xl font-bold md:text-3xl lg:text-4xl']";
    private const string PostalCodeAndCityLocator = "[class='text-neutral-40']";
    private const string PropertyPriceLocator = "[class='flex gap-2 font-bold']";
    private const string PropertyImagesContainerLocator = "[class='md:grid gap-1 grid-cols-3 md:grid-cols-4 grid-rows-2 rounded-lg md:max-h-112 md:overflow-hidden']";
    private static readonly Regex PropertyPriceRegex = new Regex(@"^€\s\d{1,3}(\.\d{3})*\s(k\.k\.|v\.o\.n\.)$");



    // Constructor
    public PropertyDetailsPage(IPage page)
    {
        _page = page;
    }


    //Methods / Actions
    // Verify navigation to property details page by checking URL contains expected slug
    public async Task verifyNavigationToPropertyDetailsPage(string expectedStreetAndHouseNo)
    {
        await _page.WaitForURLAsync("**/detail/**");

        var currentUrl = _page.Url;

        // Trim, remove newlines, normalize spaces, lowercase
        var expectedSlug = expectedStreetAndHouseNo
            .Trim()                        // remove leading/trailing spaces
            .Replace("\n", "")             // remove newline characters
            .Replace("\r", "")             // remove carriage returns (if any)
            .Replace("  ", " ")            // replace double spaces with single space
            .ToLower()
            .Replace(" ", "-");            // convert spaces to hyphens

        Console.WriteLine($"Expected slug: {expectedSlug}");
        Console.WriteLine($"Current URL: {currentUrl}");

        Assert.Contains(expectedSlug, currentUrl);
    }

    // Verify that property details (street, house number) match those from the card
    public async Task verifyPropertyDetailsMatchCardStreetAndHouseNumber(String expectedStreetAndHouseNo)
    {
        var streetAndHouseNoPropertyDetails = _page.Locator(StreetAndHouseNumberLocator);
        await streetAndHouseNoPropertyDetails.WaitForAsync(new() { State = WaitForSelectorState.Visible });

        // Capture text from the element
        var actualText = await streetAndHouseNoPropertyDetails.InnerTextAsync();

        // Normalize both strings (trim spaces, remove newlines) before comparing
        var normalizedExpected = expectedStreetAndHouseNo.Trim().Replace("\n", "").Replace("\r", "");
        var normalizedActual = actualText.Trim().Replace("\n", "").Replace("\r", "");

        Console.WriteLine($"Expected: {normalizedExpected}");
        Console.WriteLine($"Actual  : {normalizedActual}");

        // Assert equality
        Assert.Equal(normalizedExpected, normalizedActual);
    }

    // Verify that property details (postal code, city) match those from the card
    public async Task verifyPropertyDetailsMatchCardpostalCodeAndCity(String postalCodeAndCity)
    {
        // Capture text from the element
        var postalCodeAndCityPropertyDetails = await _page.Locator(PostalCodeAndCityLocator).InnerTextAsync();

        Console.WriteLine($"Expected: {postalCodeAndCity}");
        Console.WriteLine($"Actual  : {postalCodeAndCityPropertyDetails}");

        // Assert equality
        Assert.Equal(postalCodeAndCity, postalCodeAndCityPropertyDetails);
    }

    // Verify that property details (property price) match those from the card
    public async Task verifyPropertyDetailsMatchCardPropertyPrice(string propertyPrice)
    {
        var priceOnPropertyDetails = await _page.Locator(PropertyPriceLocator).InnerTextAsync();
        Console.WriteLine($"Property Price on Details Page: {priceOnPropertyDetails}");

        // Validate format
        Assert.Matches(PropertyPriceRegex, priceOnPropertyDetails);

        // Assert that it matches the expected price
        Assert.Equal(propertyPrice.Trim(), priceOnPropertyDetails.Trim());
    }

    // Check that the property image is displayed on the property details page
    public async Task checkPropertyImageOnPropertyDetails()
    {
        // Wait for the container to appear
        var containerLocator = _page.Locator(PropertyImagesContainerLocator);
        await containerLocator.WaitForAsync();

        // Find all images inside the container
        var imageLocator = containerLocator.Locator("img");
        var imageCount = await imageLocator.CountAsync();

        Console.WriteLine(imageCount > 0
            ? $"Property has {imageCount} image(s) on the details page."
            : "No property images found on the details page.");

        // Assert at least one image exists
        Assert.True(imageCount > 0, "Expected at least one property image on the details page.");
    }
}

