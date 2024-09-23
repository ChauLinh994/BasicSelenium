// using OpenQA.Selenium;
//
// namespace BasicSelenium.Common.Infrastructures;
//
// public class TestCommon : IDisposable
// {
//     public IWebDriver Driver { get; set; } = ChromeDriverCore.CreateDriver();
//
//     public void Dispose()
//     {
//         Driver.Quit();
//         GC.SuppressFinalize(this);
//     }
// }