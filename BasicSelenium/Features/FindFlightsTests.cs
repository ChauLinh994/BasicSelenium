using BasicSelenium.Common.Infrastructures;
using OpenQA.Selenium;

namespace BasicSelenium.Features;

public class ABC
{
    
}

public class FindFlightsTests
{
    private const string Url = "https://blazedemo.com/";
    // Step 1: Create a new instance of the Chrome driver
    // var driver = ChromeDriverCore.CreateDriver();
    
    // Step 2: Find the element by CSS Selector/Id/ClassName/Tag
    
    
    
    
    [Fact]
    public async Task FindFlights_PageShow_ExpectedResult()
    {
        // Create a new instance of the Chrome driver
        var driver = ChromeDriverCore.CreateDriver(Url);

        // Check if the title contains "BlazeDemo"
        Assert.Contains("BlazeDemo", driver.Title);
        
        // Check if findFlightsButton is showed
        var findFlightsButton = driver.FindElement(By.CssSelector("body > div.container > form > div > input"));
        Assert.NotNull(findFlightsButton);
        
        // Check if Title Page is displayed
        var titlePage = driver.FindElement(By.CssSelector("body > div.jumbotron > div > h1"));
        Assert.Contains("Welcome to the Simple Travel Agency!", titlePage.Text);
        
        // Check if Departure Dropdown List is displayed
        var departureCity = driver.FindElement(By.CssSelector("body > div.container > form > select:nth-child(1)"));
        Assert.NotNull(departureCity);

        // Check if Dropdown List is displayed
        var destinationCity = driver.FindElement(By.CssSelector("body > div.container > form > select:nth-child(4)"));
        Assert.NotNull(destinationCity);

        // Cleanup
        driver.Quit();
    }

    [Fact]
    public async Task FindFlights_FlightResults_ExpectedResult()
    {
        string titlePage = "Flights from Paris to Buenos Aires:";
        By titlePageSelector = By.CssSelector("body > div.container > h3");
        
        var driver = ChromeDriverCore.CreateDriver(Url);
        
        await Task.Delay(2000);

        var findFlightsButton = driver.FindElement(By.CssSelector("body > div.container > form > div > input"));
        Assert.NotNull(findFlightsButton);
        findFlightsButton.Click();

        driver.WaitingForElement(titlePage, titlePageSelector);
        
        Assert.Contains(titlePage, driver.FindElement(titlePageSelector).Text);

        // Cleanup
        driver.Quit();
    }
}