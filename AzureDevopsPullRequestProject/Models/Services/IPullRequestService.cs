using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDevopsPullRequestProject.Models.Services
{
    public interface IPullRequestService
    {
        Task<PullRequestResponse?> GetAllActivePullRequest();
        Task<PullRequestResponse?> GetAllCompletedPullRequest();
        Task<PullRequestResponse?> GetAllAbandonedPullRequest();
        Task<WorkItemResponse?> GetWorkItemsById(int id);
        Task<CommentThreadResponse?> GetThreadsByPullRequestId(int id);
        (string approvalDate, string approvalUser, int commentCount, List<string> commentsByUser)
            ProcessComments(CommentThreadResponse dataComments);
    }
}
