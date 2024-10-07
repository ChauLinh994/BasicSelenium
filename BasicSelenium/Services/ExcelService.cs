using BasicSelenium.Common.Helpers;
using BasicSelenium.Models;
using OfficeOpenXml;

namespace BasicSelenium.Services;

public class ExcelService
{
    public List<UserInformation> GetUserInformations(string path)
    {
        List<UserInformation> symbolMappings = [];
        
        // Load file excel và các setting ban đầu
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using ExcelPackage package = new ExcelPackage(new FileInfo(path));
        
        // Get First Worksheet
        var workSheet = package.Workbook.Worksheets.FirstOrDefault();

        if (workSheet is null)
        {
            return [];
        }

        for (int i = 2; i < workSheet.Dimension.End.Row + 1; i++)
        {
            symbolMappings.Add(new()
            {
                LastName = ExcelHelpers.GetStringValue(workSheet.Cells[i, 1]),
                FirstName = ExcelHelpers.GetStringValue(workSheet.Cells[i, 2]),
                CountryCode = ExcelHelpers.GetStringValue(workSheet.Cells[i, 3]),
            });
        }

        return symbolMappings;
    }
}