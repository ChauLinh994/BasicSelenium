using BasicSelenium.Common.Infrastructures;
using OpenQA.Selenium;

namespace BasicSelenium.Features;

public class SwapLabTests
{
    private const string Url = "https://www.saucedemo.com/";
    
    [Fact]
    public async Task SwapLab_PageShow_ExpectedResult()
    {
        var driver = ChromeDriverCore.CreateDriver(Url);
        
        await Task.Delay(2000);
        
        // Input to Username Textbox
        var byUsername = By.Id("user-name");
        var usernameTextBox = driver.FindElement(byUsername);
        
        Assert.NotNull(usernameTextBox);
        usernameTextBox.SendKeys("standard_user");
        
        // Input to Password Textbox
        var byPassword = By.Id("password");
        var passwordTextBox = driver.FindElement(byPassword);

        Assert.NotNull(passwordTextBox);
        passwordTextBox.SendKeys("secret_sauce");

        // Delay 2s
        await Task.Delay(2000);
        
        // Click on Login Button
        var byLoginButton = By.Id("login-button");
        var loginButton = driver.FindElement(byLoginButton);

        Assert.NotNull(loginButton);
        loginButton.Click();

        driver.Quit();
    }
    
    //[Fact]
}