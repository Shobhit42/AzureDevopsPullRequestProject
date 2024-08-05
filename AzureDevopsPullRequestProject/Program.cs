using AzureDevopsPullRequestProject.Models.Services;
using AzureDevopsPullRequestProject.ViewModels;
using AzureDevopsPullRequestProject.Views;

namespace AzureDevopsPullRequestProject
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            PullRequestService service = new PullRequestService();
            PullRequestViewModel pullRequestViewModel = new PullRequestViewModel(service);
            PullRequestView pullRequestView = new PullRequestView(pullRequestViewModel);

            await pullRequestView.ShowPullRequestData();

            Console.WriteLine("Report Created Successfully");
            Console.ReadLine();
        }
    }
}
