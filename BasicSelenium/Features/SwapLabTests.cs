using BasicSelenium.Common.Infrastructures;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BasicSelenium.Features;

public class SwapLabTests
{
    private const string Url = "https://www.saucedemo.com/";
    //Login page
    private async Task Login(ChromeDriver driver)
    {
        var byUsername = By.Id("user-name");
        var usernameTextBox = driver.FindElement(byUsername);

        Assert.NotNull(usernameTextBox);
        usernameTextBox.SendKeys("standard_user");

        var byPassword = By.Id("password");
        var passwordTextBox = driver.FindElement(byPassword);

        Assert.NotNull(passwordTextBox);
        passwordTextBox.SendKeys("secret_sauce");

        await Task.Delay(2000);

        var byLoginButton = By.Id("login-button");
        var loginButton = driver.FindElement(byLoginButton);

        Assert.NotNull(loginButton);
        loginButton.Click();
    }
    //Inventory page
    private List<string> InventoryIds =
        ["add-to-cart-sauce-labs-backpack",
        "add-to-cart-sauce-labs-bike-light",
        "add-to-cart-sauce-labs-bolt-t-shirt",
        "add-to-cart-sauce-labs-fleece-jacket",
        "add-to-cart-sauce-labs-onesie",
        "add-to-cart-test.allthethings()-t-shirt-(red)"
        ];

    private List<string> RemoveInventoryIds =
        [
        "remove-sauce-labs-backpack",
        "remove-sauce-labs-bike-light",
        "remove-sauce-labs-bolt-t-shirt",
        "remove-sauce-labs-fleece-jacket",
        "remove-sauce-labs-onesie",
        "remove-test.allthethings()-t-shirt-(red)"
        ];

    private async Task RamdomSelectedSingleProduct(ChromeDriver driver)
    {
        Random randomivt = new Random();
        int randomindex = randomivt.Next(0, InventoryIds.Count);

        var invetoryId = By.Id(InventoryIds[randomindex]);
        var addtoCartBtn = driver.FindElement(invetoryId);

        await Task.Delay(500);
        Assert.NotNull(addtoCartBtn);
        addtoCartBtn.Click();
        await Task.Delay(500);
    }

    private async Task SelectedMultipleProduct(ChromeDriver driver, int numberOfProducts)
    {
        for (int i = 0; i < numberOfProducts; i++)
        {
            var invetoryId = By.Id(InventoryIds[i]);
            var addtoCartBtn = driver.FindElement(invetoryId);

            await Task.Delay(500);
            Assert.NotNull(addtoCartBtn);
            addtoCartBtn.Click();
            await Task.Delay(500);
        }
    }

    private async Task RemoveMultipleProduct(ChromeDriver driver)
    {
        for (int i = 0;i < RemoveInventoryIds.Count;i++)
        {
            var removeInventoryId = By.Id(RemoveInventoryIds[i]);
            var removeInventoryBtn = driver.FindElement(removeInventoryId);
            await Task.Delay(500);
            Assert.NotNull(removeInventoryBtn);
            removeInventoryBtn.Click();
            await Task.Delay(500);
        }
    }
    //Your Cart Drawer
    private async Task ShoppingCartAction (ChromeDriver driver)
    {
        var shoppingCardId = By.Id("shopping_cart_container");
        var shoppingCartBtn = driver.FindElement(shoppingCardId);
        await Task.Delay(500);
        shoppingCartBtn.Click();
        await Task.Delay(500);
    }

    private async Task ContinueShoppingAction (ChromeDriver driver)
    {
        var continueShoppingId = By.Id("continue-shopping");
        var continueShoppingBtn = driver.FindElement(continueShoppingId);
        await Task.Delay(500);
        continueShoppingBtn.Click();
        await Task.Delay(500);
    }
    //CheckOut
    private async Task CheckOutAction (ChromeDriver driver)
    {
        var checkOutId = By.Id("checkout");
        var checkOutBtn = driver.FindElement(checkOutId);
        await Task.Delay(500);
        checkOutBtn.Click();
        await Task.Delay(500);
    }

    private async Task CheckOutYourInformation (ChromeDriver driver)
    {
        var firstNameId = By.Id("first-name");
        var firstNameTextBox = driver.FindElement(firstNameId);
        Assert.NotNull(firstNameTextBox);
        firstNameTextBox.SendKeys("Jon");

        var lastNameId = By.Id("last-name");
        var lastNameTextBox = driver.FindElement(lastNameId);
        Assert.NotNull(lastNameTextBox);
        lastNameTextBox.SendKeys("Doe");

        var postalCodeId = By.Id("postal-code");
        var postalCodeTextBox = driver.FindElement(postalCodeId);
        Assert.NotNull(postalCodeTextBox);
        postalCodeTextBox.SendKeys("94043");
    }





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

    [Fact]
    public async Task SwapLab_SelectedMultipleProduct_YourCart_CheckOut_Result()
    {

        var driver = ChromeDriverCore.CreateDriver(Url);

        await Task.Delay(2000);

        await Login(driver);

        await SelectedMultipleProduct(driver, InventoryIds.Count);
        await SelectedMultipleProduct(driver, 5);

        await ShoppingCartAction(driver);

        await CheckOutAction(driver);

        await CheckOutYourInformation(driver);

        driver.Quit();
    }

    [Fact]
    public async Task SwapLab_SelectedSingleProduct_YourCart_CheckOut_Result()
    {

        var driver = ChromeDriverCore.CreateDriver(Url);

        await Task.Delay(2000);

        await Login(driver);

        await RamdomSelectedSingleProduct(driver);

        await ShoppingCartAction(driver);

        await CheckOutAction(driver);

        await CheckOutYourInformation(driver);

        driver.Quit();
    }

    [Fact] 
    public async Task SwapLab_SelectedMultipleProduct_YourCart_ContinueShopping_Result()
    {
        var driver = ChromeDriverCore.CreateDriver(Url);
        
        await Task.Delay(2000);

        await Login(driver);

        await SelectedMultipleProduct(driver,5);

        await ShoppingCartAction(driver);

        await ContinueShoppingAction(driver);

        await RemoveMultipleProduct(driver);
        await ShoppingCartAction (driver);
        await CheckOutAction(driver);
        await CheckOutYourInformation(driver);
        driver.Quit();

    }
}







