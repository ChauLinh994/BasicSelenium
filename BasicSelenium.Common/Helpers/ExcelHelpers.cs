using OfficeOpenXml;

namespace BasicSelenium.Common.Helpers;

public static class ExcelHelpers
{
    public static string GetStringValue(this ExcelRange cell)
    {
        if (cell.Value is null)
        {
            return string.Empty;
        }
        
        return cell.Value.ToString()!;
    }
}