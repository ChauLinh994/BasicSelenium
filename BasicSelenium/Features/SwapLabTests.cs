using System.Collections.ObjectModel;
using BasicSelenium.Common.Infrastructures;
using BasicSelenium.Models;
using BasicSelenium.Services;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V85.DOMSnapshot;
using OpenQA.Selenium.Support.UI;

namespace BasicSelenium.Features;

public class SwapLabTests
{
    private const string Url = "https://www.saucedemo.com/";

    //Login page
    private async Task Login(ChromeDriver driver)
    {
        await InputToTextBox(driver, "user-name", "standard_user");
         
        await InputToTextBox(driver, "password", "secret_sauce");

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

    private readonly List<string> DropdownListvalues =
    [
        "az",
        "za",
        "lohi",
        "hilo",
    ];


    private async Task FilteringOptionsDropdown(ChromeDriver driver, int numberOfFiltering, bool isRandom)
    {
        List<int> filteredIndexList = new List<int>();// Create the list to contain the selected Filtered option

        Random rd = new Random();

        for (int i = 0; i < numberOfFiltering; i++)
        {
            int inventoryfilteredIndex = i;

            if (isRandom)
            {
                do
                {
                    inventoryfilteredIndex = rd.Next(0, DropdownListvalues.Count);
                }
                while (filteredIndexList.Contains(inventoryfilteredIndex));

               filteredIndexList.Add(inventoryfilteredIndex);
            }

            var inventoryFilterValue = By.CssSelector(DropdownListvalues[inventoryfilteredIndex]);
            var inventoryFilterDropdownList = driver.FindElement(inventoryFilterValue);
            await Task.Delay(500);

            Assert.NotNull(inventoryFilterDropdownList);

            inventoryFilterDropdownList.Click();
            await Task.Delay(500);
        }
    }

    private async Task SelectSortDropdownItems(ChromeDriver driver)
    {
        var bySelect = By.ClassName("product_sort_container");
        var filterDropdownList = driver.FindElement(bySelect);

        var byItems = By.TagName("option");
        var options = filterDropdownList.FindElements(byItems);

        var loopCount = options.Count();

        Random rd = new Random();
        List<int> selectedOptions = new List<int>();

        for (int i = 0; i < loopCount; i++)
        {
            filterDropdownList = driver.FindElement(bySelect);
            options = filterDropdownList.FindElements(byItems);

            filterDropdownList.Click();

            await Task.Delay(2000);

            int index = i;

            do
            {
                index = rd.Next(0, loopCount);
            }
            while (selectedOptions.Contains(index));

            selectedOptions.Add(index);

            options[index].Click();

            await Task.Delay(2000);
        }
    }

    private void FilteringDropdownBtn(ChromeDriver driver)
    {
        var filterDropdownId = By.ClassName("product_sort_container");
        var filterDropdownList = driver.FindElement(filterDropdownId);

        filterDropdownList.Click();

    }
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

    // ReadOnlyCollection ~ List ===> Collection
    // IWebElement ~ int, string, class
    private List<IWebElement> FindRemoveButtons(ChromeDriver driver, int numberOfRemoveProduct, bool isRandom)
    {
        // 10 Button Remove
        var removeButtonClassName = By.ClassName("btn_secondary");
        ReadOnlyCollection<IWebElement> buttons = driver.FindElements(removeButtonClassName);

        List<IWebElement> selectedButtons = new List<IWebElement>();
        List<int> inventorySelectedIndexes = new List<int>();

        Random rd = new Random();

        // numberOfRemoveProduct = 5
        // 0 -> 4
        // Loop 1: i = 0; 
        for (int i = 0; i < numberOfRemoveProduct; i++)
        {
            int index = i;
            // isRandom = true
            if (isRandom)
            {               // Logic for random
                do
                {
                    index = rd.Next(0, buttons.Count);
                }
                while (inventorySelectedIndexes.Contains(index));//true => loop

                inventorySelectedIndexes.Add(index);
            }
            selectedButtons.Add(buttons[index]);
        }


        // all button remove ---> buttons
        // Take 5 button --> selectedButtons
        // return selectedButtons

        return selectedButtons;
    }

    private async Task RemoveMultipleProducts(ChromeDriver driver, int numberOfProduct, bool isRandom)
    {

        List<IWebElement> removeButtons = FindRemoveButtons(driver, numberOfProduct, isRandom);

        foreach (IWebElement item in removeButtons)
        {
            await Task.Delay(500);
            item.Click();
            await Task.Delay(500);
        }
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

    private static List<IWebElement> FindRemoveBtninYourCart(ChromeDriver driver, int numberOfRemoveProductYourCart, bool isRandom)
    {
        By removeProductinYourCartId = By.Id("btn btn_secondary btn_small cart_button");
        ReadOnlyCollection<IWebElement> removeButtons = driver.FindElements(removeProductinYourCartId);

        List<IWebElement> selectedRemoveButtons = new List<IWebElement>(); // Create List to contain the Remove button that has been selected

        List<int> SelectedIndex = new List<int>(); //Create List to contain the remove button index for looping

        Random rd = new Random();

        for (int i = 0; i < numberOfRemoveProductYourCart; i++) // i < NumberOfRemoveButton in YourCart 
        {
            int index = i;
            if (isRandom)
            {
                do
                {
                    index = rd.Next(0, removeButtons.Count);
                }
                while (SelectedIndex.Contains(index));  //Do the index existed in the list ? ==> isTrue => loop do while
                SelectedIndex.Add(index); // isFalse ==> Add index to the list
            }
            selectedRemoveButtons.Add(removeButtons[index]); //add removebuton ID + index to the List ==>  loop for 
        }
        return selectedRemoveButtons;
    }
    private async Task YourCart_RemoveProduct_Action(ChromeDriver driver, int numberOfRemoveProductYourCart, bool isRandom)
    {
        List<IWebElement> removeBtn = FindRemoveBtninYourCart(driver, numberOfRemoveProductYourCart, isRandom);

        foreach (IWebElement item in removeBtn)
        {
            await Task.Delay(500);
            item.Click();
            await Task.Delay(500);
        }
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

    //CheckOutYourInformation page
    
    private async Task CheckOutYourInformation(ChromeDriver driver)
    {
        
        await InputToTextBox(driver, "first-name", "Jon");
        await InputToTextBox(driver, "last-name", "Doe");
        await InputToTextBox(driver, "postal-code", "94043");
    }
    private async Task InputToTextBox(ChromeDriver driver, string id, string value)
    {
        var byId = By.Id(id);
        var textBox = driver.FindElement(byId);

        Assert.NotNull(textBox);
        textBox.SendKeys(value);
        await Task.Delay(2000);

    }
    private void YourInformationContinueBtnaction(ChromeDriver driver)
    {
        var continueId = By.Id("continue");
        var continueBtn = driver.FindElement(continueId);
        continueBtn.Click();
    }

    private void Overview_FinishBtnAction(ChromeDriver driver)
    {
        var finishId = By.Id("finish");
        var finishBtn = driver.FindElement(finishId);
        finishBtn.Click();
    }
    private void BackToHomeBtnAction(ChromeDriver driver)
    {
        var BackToHomeId = By.Id("back-to-products");
        var BackToHomeBtn = driver.FindElement(BackToHomeId);
        BackToHomeBtn.Click();
    }


    private async Task Delay(int v)
    {
        throw new NotImplementedException();
    }

    [Fact]
    public async Task SwapLab_SelectedMultipleProduct_YourCart_CheckOut_OrderSuccessful_Result()
    {
        var driver = ChromeDriverCore.CreateDriver(Url);
        await Task.Delay(2000);
        await Login(driver);
        await SelectedMultipleProduct(driver, InventoryIds.Count, true);
        await ShoppingCartAction(driver);
        await CheckOutAction(driver);
        await CheckOutYourInformation(driver);
        YourInformationContinueBtnaction(driver);
        Overview_FinishBtnAction(driver);
        driver.Quit();
    }



    [Fact]

    public async Task SwapLab_RemoveMultipleProduct_YourCart_CheckOut_OrderSuccessful_Result()
    {
        var driver = ChromeDriverCore.CreateDriver(Url);
        await Task.Delay(2000);
        await Login(driver);
        await SelectedMultipleProduct(driver, 3, true);
        await RemoveMultipleProducts(driver, 2, true);
        await ShoppingCartAction(driver);
        await CheckOutAction(driver);
        await CheckOutYourInformation(driver);
        YourInformationContinueBtnaction(driver);
        Overview_FinishBtnAction(driver);
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
        await CheckOutYourInformation(driver);
        YourInformationContinueBtnaction(driver);
        Overview_FinishBtnAction(driver);
        driver.Quit();
    }

    [Fact]
    public async Task SwapLab_RemoveSingleProduct_YourCart_Null_CheckOut_Result()
    {
        var driver = ChromeDriverCore.CreateDriver(Url);
        await Task.Delay(2000);
        await Login(driver);
        await SelectedMultipleProduct(driver, 1, true);
        await RemoveMultipleProducts(driver, 1, false);
        await ShoppingCartAction(driver);
        await CheckOutAction(driver);
        YourInformationContinueBtnaction(driver);
        driver.Quit();
    }

    [Fact]
    public async Task SwapLab_SelectedSingleProduct_YourCart_ContinueShoppingSelect_CheckOut_Result()
    {
        var driver = ChromeDriverCore.CreateDriver(Url);
        await Task.Delay(2000);
        await Login(driver);
        await SelectedMultipleProduct(driver, 1, true);
        await ShoppingCartAction(driver);
        await ContinueShoppingAction(driver);
        await SelectedMultipleProduct(driver, 1, true);
        await ShoppingCartAction(driver);
        await CheckOutAction(driver);
        YourInformationContinueBtnaction(driver);
        Overview_FinishBtnAction(driver);
        await CheckOutYourInformation(driver);
        driver.Quit();
    }

    [Fact]
    public async Task SwapLab_SelectedSingleProduct_YourCart_ContinueShoppingRemove_CheckOut_Result()
    {
        var driver = ChromeDriverCore.CreateDriver(Url);
        await Login(driver);
        await SelectedMultipleProduct(driver, 3, true);
        await ShoppingCartAction(driver);
        await ContinueShoppingAction(driver);
        await RemoveMultipleProducts(driver, 1, true);
        await ShoppingCartAction(driver);
        await CheckOutAction(driver);
        YourInformationContinueBtnaction(driver);
        Overview_FinishBtnAction(driver);
        await CheckOutYourInformation(driver);
        driver.Quit();
    }


    [Fact]
    public async Task SwapLab_YourCart_RemoveProduct_CheckOut_Result()
    {
        var driver = ChromeDriverCore.CreateDriver(Url);
        await Login(driver);
        await SelectedMultipleProduct(driver, 3, true);
        await ShoppingCartAction(driver);
        await YourCart_RemoveProduct_Action(driver, 1, true);
        await CheckOutAction(driver);
        YourInformationContinueBtnaction(driver);
        Overview_FinishBtnAction(driver);
        await CheckOutYourInformation(driver);
        driver.Quit();
    }

    [Fact]
    public async Task SwapLab_FilteringOptions()
    {
        var driver = ChromeDriverCore.CreateDriver(Url);
        await Login(driver);
        await SelectSortDropdownItems(driver);
        driver.Quit();
    }


    [Fact]
    public async Task SwapLab_MultipleOrderSuccessful_Result()
    {
        var driver = ChromeDriverCore.CreateDriver(Url);
        ExcelService excelService = new ExcelService();
        List<UserInformation> data = excelService.GetUserInformations("./Resources/UserInformations.xlsx");
        await Login(driver);
        for (int i = 0; i < data.Count; i++)
        {
            
            await SelectedMultipleProduct(driver, 5, true);
            await ShoppingCartAction(driver);
            await CheckOutAction(driver);
            await CheckOutYourInformation1(driver, data[i]);
            YourInformationContinueBtnaction(driver);
            await Task.Delay(2000);
            Overview_FinishBtnAction(driver);
            BackToHomeBtnAction (driver);
        }       
        driver.Quit();
    }
    
    private async Task CheckOutYourInformation1(ChromeDriver driver, UserInformation userInfo)
    {
        await InputToTextBox(driver, "first-name", userInfo.FirstName);
        await InputToTextBox(driver, "last-name", userInfo.LastName);
        await InputToTextBox(driver, "postal-code", userInfo.CountryCode);
    }

    [Fact]
    public void Test()
    {
        // if isRandom = true, ==> Random ra 5 so ko trung nhau
        // if isRandom = false ==> 0,1,2,3,4
        bool isRandom = true;

        List<int> buttons = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        List<int> selectdButtons = new List<int>();


        List<int> checkIndexes = new List<int>();

        Random rd = new Random();

        for (int i = 0; i < 5; i++)
        {
            int tempIndex = i;

            if (isRandom)
            {
                do
                {
                    tempIndex = rd.Next(0, 5);
                }
                while (checkIndexes.Contains(tempIndex)); // Dieu kien de? loop, While = true


                checkIndexes.Add(tempIndex);
            }

            selectdButtons.Add(buttons[tempIndex]);
        }
    }
}