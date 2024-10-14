using BasicSelenium.Models;
using BasicSelenium.Services;

namespace BasicSelenium.Features;

public class Tests
{
    [Fact]
    public void Test()
    {
        ExcelService excelService = new ExcelService();

        List<UserInformation> data = excelService.GetUserInformations("./Resources/UserInformations.xlsx");

    }
}