using AzureDevopsPullRequestProject.Models;
using AzureDevopsPullRequestProject.ViewModels;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace AzureDevopsPullRequestProject.Views
{
    public class PullRequestView
    {
        private readonly PullRequestViewModel _pullRequestViewModel;

        public PullRequestView(PullRequestViewModel pullRequestViewModel)
        {
            _pullRequestViewModel = pullRequestViewModel;
        }

        public async Task ShowPullRequestData()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();

            // Completed Pull Requests
            var completedPullRequestResponse = await _pullRequestViewModel.GetAllCompletedPullRequest();
            await DisplayPullRequests(completedPullRequestResponse, package, "Completed Pull Requests");

            // Abandoned Pull Requests
            var abandonedPullRequestResponse = await _pullRequestViewModel.GetAllAbandonedPullRequest();
            await DisplayPullRequests(abandonedPullRequestResponse, package, "Abandoned Pull Requests");

            // Active Pull Reuest
            var activePullRequestResponse = await _pullRequestViewModel.GetAllActivePullRequest();
            await DisplayPullRequests(activePullRequestResponse, package, "Active Pull Requests");

            // Save the Excel file
            var fileInfo = new FileInfo("PullRequestsReport11.xlsx");
            package.SaveAs(fileInfo);
        }

        private async Task DisplayPullRequests(PullRequestResponse pullRequestResponse, ExcelPackage package, string sheetName)
        {

            try
            {
                var worksheet = package.Workbook.Worksheets.Add(sheetName);
                worksheet.Cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                Utility.InitializeWorksheetHeader(worksheet);

                int row = 2;
                foreach (var pullRequest in pullRequestResponse.PullRequests)
                {
                    var pullRequestUiUrl = $"{System.Configuration.ConfigurationManager.AppSettings["PullRequestUiUrl"]}/{pullRequest.Id}";
                    Utility.AddHyperlink(worksheet, row, 1, pullRequest.Id.ToString(), pullRequestUiUrl);

                    var workItemResponse = await _pullRequestViewModel.GetWorkItemsById(pullRequest.Id);
                    SetWorkItems(workItemResponse, row, worksheet);

                    worksheet.Cells[row, 3].Value = pullRequest.Description;
                    worksheet.Cells[row, 4].Value = Utility.ConvertToIST(pullRequest.CreationDate);

                    var dataComments = await _pullRequestViewModel.GetThreadsByPullRequestId(pullRequest.Id);
                    var (approvalDates, approvalUsers, commentCount, commentsByUser) = _pullRequestViewModel.ProcessComments(dataComments);
                    if (sheetName == "Active Pull Requests") 
                    {
                        if (approvalDates != null)
                        {
                            worksheet.Cells[row, 5].Value = approvalDates;
                            worksheet.Cells[row, 7].Value = approvalUsers;
                        }
                        else
                        {
                            worksheet.Cells[row, 5].Value = "N/A";
                            worksheet.Cells[row, 7].Value = "N/A";
                        }
                    }
                    else if (sheetName == "Abandoned Pull Requests")
                    {
                        worksheet.Cells[row, 5].Value = "N/A";
                        worksheet.Cells[row, 7].Value = "N/A";
                    }
                    else
                    {
                        worksheet.Cells[row, 5].Value = approvalDates;
                        worksheet.Cells[row, 7].Value = approvalUsers;
                    }
                    //worksheet.Cells[row, 5].Value = sheetName == "Abandoned Pull Requests" ? "N/A" : string.Join(", ", approvalDates);
                    worksheet.Cells[row, 6].Value = sheetName == "Active Pull Requests" ? "N/A" : Utility.ConvertToIST((DateTime)pullRequest.ClosedDate);
                    //worksheet.Cells[row, 7].Value = sheetName == "Abandoned Pull Requests" ? "N/A" : string.Join(", ", approvalUsers);
                    worksheet.Cells[row, 8].Value = commentCount;
                    worksheet.Cells[row, 9].Value = string.Join(", ", commentsByUser);

                    worksheet.Cells[row, 10].Value = pullRequest.SourceBranch.Replace("refs/heads/", "");
                    worksheet.Cells[row, 11].Value = pullRequest.TargetBranch.Replace("refs/heads/", "");

                    row++;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        public void SetWorkItems(WorkItemResponse workItemResponse, int row, ExcelWorksheet worksheet)
        {
            
            for (int i = 0; i < workItemResponse.Count; i++)
            {
                var id = workItemResponse.WorkItems[i].Id;
                var WorkItemUiUrl = System.Configuration.ConfigurationManager.AppSettings["WorkItemUiUrl"] + "/" + id;
                var cell = worksheet.Cells[row, 2];

                // Check if the cell already contains some value
                if (string.IsNullOrEmpty(cell.Text))
                {
                    cell.Value = id.ToString();
                }
                else
                {
                    cell.Value += ", " + id;
                }

                // Set hyperlink for the current ID
                cell.Hyperlink = new Uri(WorkItemUiUrl);

                // Apply formatting for the cell to appear as a hyperlink
                cell.Style.Font.Color.SetColor(System.Drawing.Color.Blue);
                cell.Style.Font.UnderLine = true;
            }
        }
    }
}
