using OfficeOpenXml;

namespace BasicSelenium.Common.Helpers;

public static class ExcelHelpers
{
    public static string GetStringValue(this ExcelRange cell)
    {
        // cell.Value ==> null, .ToString ==> exception
        if (cell.Value is null)
        {
            return string.Empty; // ==> ""
        }
        
        return cell.Value.ToString()!;
    }
}