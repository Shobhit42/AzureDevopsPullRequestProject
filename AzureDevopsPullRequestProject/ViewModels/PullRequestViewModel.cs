using AzureDevopsPullRequestProject.Models;
using AzureDevopsPullRequestProject.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDevopsPullRequestProject.ViewModels
{
    public class PullRequestViewModel
    {
        private readonly PullRequestService _pullRequestService;

        public async Task<PullRequestResponse?> GetAllActivePullRequest()
        {
            return await _pullRequestService.GetAllActivePullRequest();
        }

        public PullRequestViewModel(PullRequestService pullRequestService)
        {
            _pullRequestService = pullRequestService;
        }

        public async Task<PullRequestResponse?> GetAllCompletedPullRequest()
        {
            return await _pullRequestService.GetAllCompletedPullRequest();
        }

        public async Task<PullRequestResponse?> GetAllAbandonedPullRequest()
        {
            return await _pullRequestService.GetAllAbandonedPullRequest();
        }

        public async Task<WorkItemResponse?> GetWorkItemsById(int id)
        {
            return await _pullRequestService.GetWorkItemsById(id);
        }

        public async Task<CommentThreadResponse?> GetThreadsByPullRequestId(int id)
        {
            return await _pullRequestService.GetThreadsByPullRequestId(id);
        }

        public (string approvalDates, string approvalUsers, int commentCount, List<string> commentsByUser) 
            ProcessComments(CommentThreadResponse dataComments)
        {
            return _pullRequestService.ProcessComments(dataComments);
        }
    }
}
