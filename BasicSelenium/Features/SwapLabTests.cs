using System.Collections.ObjectModel;
using BasicSelenium.Common.Infrastructures;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BasicSelenium.Features;

public class SwapLabTests
{
    private const string Url = "https://www.saucedemo.com/";

    // Todo: Use InputToTextBox instead
    //Login page
    private async Task Login(ChromeDriver driver)
    {
        InputToTextBox(driver, "user-name", "standard_user");

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
    private readonly List<string> InventoryIds = 
    [ 
        "add-to-cart-sauce-labs-backpack", 
        "add-to-cart-sauce-labs-bike-light", 
        "add-to-cart-sauce-labs-bolt-t-shirt", 
        "add-to-cart-sauce-labs-fleece-jacket", 
        "add-to-cart-sauce-labs-onesie", 
        "add-to-cart-test.allthethings()-t-shirt-(red)" 
    ];

    private readonly List<string> RemoveInventoryIds = 
    [ 
        "remove-sauce-labs-backpack", 
        "remove-sauce-labs-bike-light", 
        "remove-sauce-labs-bolt-t-shirt", 
        "remove-sauce-labs-fleece-jacket", 
        "remove-sauce-labs-onesie", 
        "remove-test.allthethings()-t-shirt-(red)" 
    ];

    // Thay vì 2 viết 2 hàm là SelectSingleProduct, SelectMultipleProduct
    // Thêm param numberOfProducts để xác định số lượng sản phẩm cần chọn
    // Ví dụ muốn 1 sản pham thi numberOfProducts = 1
    private async Task SelectedMultipleProduct(ChromeDriver driver, int numberOfProducts, bool isRandom)
    {
        // Add 5 products
        // adding 4th ==> Trùng? 
        List<int> inventorySelectedIndexes = [];
        
        // isRandom, true ==> random, false ==> not random
        Random rd = new Random();
        
        for (int i = 0; i < numberOfProducts; i++)
        {
            int inventoryIndex = i;

            if (isRandom)
            {
                inventoryIndex = rd.Next(0, InventoryIds.Count);
                
                // Check trùng?
                // isContained = true ---> inventoryIndex có tồn tại trong List inventorySelectedIndex
                while (inventorySelectedIndexes.Contains(inventoryIndex))
                {
                    inventoryIndex = rd.Next(0, InventoryIds.Count);
                }

                // Add vaào danh sách de check
                inventorySelectedIndexes.Add(inventoryIndex);
            }
            
            // Click()
            var inventoryId = By.Id(InventoryIds[inventoryIndex]);
            var addToCartBtn = driver.FindElement(inventoryId);

            await Task.Delay(500);
            Assert.NotNull(addToCartBtn);
            addToCartBtn.Click();
            await Task.Delay(500);
        }
    }

    private ReadOnlyCollection<IWebElement> FindRemoveButtons(ChromeDriver driver)
    {
        var removeButtonClassName = By.ClassName("btn_secondary");
        ReadOnlyCollection<IWebElement>? buttons = driver.FindElements(removeButtonClassName);
        
        return buttons;
    }

    private async Task RemoveMultipleProducts(ChromeDriver driver)
    {
        var removeButtons = FindRemoveButtons(driver);
        
        foreach (var item in removeButtons)
        {
            await Task.Delay(500);
            item.Click();
            await Task.Delay(500);
        }

        // for (int i = 0; i < removeButtons.Count; i++)
        // {
        //     await Task.Delay(500);
        //     removeButtons[i].Click();
        //     await Task.Delay(500);
        // }
    }

    //Your Cart Drawer
    private async Task ShoppingCartAction(ChromeDriver driver)
    {
        var shoppingCardId = By.Id("shopping_cart_container");
        var shoppingCartBtn = driver.FindElement(shoppingCardId);
        await Task.Delay(500);
        shoppingCartBtn.Click();
        await Task.Delay(500);
    }

    private async Task ContinueShoppingAction(ChromeDriver driver)
    {
        var continueShoppingId = By.Id("continue-shopping");
        var continueShoppingBtn = driver.FindElement(continueShoppingId);
        await Task.Delay(500);
        continueShoppingBtn.Click();
        await Task.Delay(500);
    }

    //CheckOut
    private async Task CheckOutAction(ChromeDriver driver)
    {
        var checkOutId = By.Id("checkout");
        var checkOutBtn = driver.FindElement(checkOutId);
        await Task.Delay(500);
        checkOutBtn.Click();
        await Task.Delay(500);
    }

    // Todo: Use InputToTextBox instead
    private void CheckOutYourInformation(ChromeDriver driver)
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

    private void InputToTextBox(ChromeDriver driver, string id, string value)
    {
        var byId = By.Id(id);
        var textBox = driver.FindElement(byId);

        Assert.NotNull(textBox);
        textBox.SendKeys(value);
    }

    [Fact]
    public async Task SwapLab_SelectedMultipleProduct_YourCart_CheckOut_Result()
    {
        var driver = ChromeDriverCore.CreateDriver(Url);

        await Task.Delay(2000);

        await Login(driver);

        await SelectedMultipleProduct(driver, InventoryIds.Count, false);

        await ShoppingCartAction(driver);

        await CheckOutAction(driver);

        CheckOutYourInformation(driver);

        driver.Quit();
    }

    [Fact]
    public async Task SwapLab_SelectedSingleProduct_YourCart_CheckOut_Result()
    {
        var driver = ChromeDriverCore.CreateDriver(Url);

        await Task.Delay(2000);

        await Login(driver);

        await SelectedMultipleProduct(driver, 1, true);

        await ShoppingCartAction(driver);

        await CheckOutAction(driver);

        CheckOutYourInformation(driver);

        driver.Quit();
    }

    [Fact]
    public async Task SwapLab_SelectedMultipleProduct_YourCart_ContinueShopping_Result()
    {
        var driver = ChromeDriverCore.CreateDriver(Url);

        await Task.Delay(2000);

        await Login(driver);

        await SelectedMultipleProduct(driver, 1, false);

        await ShoppingCartAction(driver);

        await ContinueShoppingAction(driver);

        await RemoveMultipleProducts(driver);
        await ShoppingCartAction(driver);
        await CheckOutAction(driver);
        CheckOutYourInformation(driver);
        driver.Quit();
    }
}