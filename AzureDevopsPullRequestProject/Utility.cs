using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDevopsPullRequestProject
{
    public class Utility
    {
        public static string ConvertToIST(DateTime dateTime)
        {
            var istTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            var istDateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, istTimeZone);
            string formattedDate = istDateTime.ToString("MMMM d, yyyy, 'at' h:mm:ss tt 'IST'");
            return formattedDate;
        }

        public static void AddHyperlink(ExcelWorksheet worksheet, int row, int col, string text, string url)
        {
            var cell = worksheet.Cells[row, col];
            cell.Value = text;
            cell.Hyperlink = new Uri(url);
            cell.Style.Font.Color.SetColor(System.Drawing.Color.Blue);
            cell.Style.Font.UnderLine = true;
            cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
        }

        public static void InitializeWorksheetHeader(ExcelWorksheet worksheet)
        {
            worksheet.Cells[1, 1].Value = "Pull Request ID";
            worksheet.Cells[1, 2].Value = "Linked Tickets ID";
            worksheet.Cells[1, 3].Value = "PR Description";
            worksheet.Cells[1, 4].Value = "Creation Date";
            worksheet.Cells[1, 5].Value = "Approval Date";
            worksheet.Cells[1, 6].Value = "PR Close Date";
            worksheet.Cells[1, 7].Value = "Approved By";
            worksheet.Cells[1, 8].Value = "No of Comments";
            worksheet.Cells[1, 9].Value = "Comments Provided by Users";
            worksheet.Cells[1, 10].Value = "Development Branch";
            worksheet.Cells[1, 11].Value = "Main Branch";
        }
    }
}
