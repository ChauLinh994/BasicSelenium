using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace BasicSelenium.Common.Infrastructures;

public static class ChromeDriverCore
{
    public static ChromeDriver CreateDriver()
    {
        string url = "https://blazedemo.com/";
        
        // Create an instance of the Chrome driver
        ChromeOptions options = new ChromeOptions();
        var serviceChrome = ChromeDriverService.CreateDefaultService();
        var driver = new ChromeDriver(serviceChrome, options);

        // Maximize the window
        driver.Manage().Window.Maximize();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        
        // Navigate to the URL
        driver.Navigate().GoToUrl(url);
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        wait.Until(driver => driver.FindElement(By.CssSelector("body > div.container > form > div > input")));

        return driver;
    }
    
    public static void WaitingForElement(this IWebDriver driver, string text, By by)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        wait.Until(driver => driver.FindElement(by).Text == text);
    }
}