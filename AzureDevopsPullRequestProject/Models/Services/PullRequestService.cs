using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;
using AzureDevopsPullRequestProject.Views;

namespace AzureDevopsPullRequestProject.Models.Services
{
    public class PullRequestService : IPullRequestService
    {
        public readonly HttpClient _httpClient;
        public PullRequestService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<PullRequestResponse?> GetAllActivePullRequest()
        {
            return await GetPullRequest("active");
        }

        public async Task<PullRequestResponse?> GetAllCompletedPullRequest()
        {
            return await GetPullRequest("completed");
        }

        public async Task<PullRequestResponse?> GetAllAbandonedPullRequest()
        {
            return await GetPullRequest("abandoned");
        }

        public async Task<PullRequestResponse?> GetPullRequest(string status)
        {

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(Encoding.ASCII.GetBytes($":{System.Configuration.ConfigurationManager.AppSettings["PatToken"]}")));

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(System.Configuration.ConfigurationManager.AppSettings["BasePullRequestUrl"] + $"?status={status}&" +
                    System.Configuration.ConfigurationManager.AppSettings["ApiVersion"]);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PullRequestResponse>(responseBody);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<WorkItemResponse?> GetWorkItemsById(int id)
        {
            try
            {
                string responseBodyWorkItem = await _httpClient.GetStringAsync(System.Configuration.ConfigurationManager.AppSettings["BasePullRequestUrl"] + "/" + id + "/workitems");
                return JsonConvert.DeserializeObject<WorkItemResponse>(responseBodyWorkItem);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<CommentThreadResponse?> GetThreadsByPullRequestId(int id)
        {
            try
            {
                string responseBodyComment = await _httpClient.GetStringAsync(System.Configuration.ConfigurationManager.AppSettings["BasePullRequestUrl"] + "/" + id + "/threads?" +
                    System.Configuration.ConfigurationManager.AppSettings["ApiVersion"]);
                return JsonConvert.DeserializeObject<CommentThreadResponse>(responseBodyComment);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public (string approvalDate, string approvalUser, int commentCount, List<string> commentsByUser)
            ProcessComments(CommentThreadResponse dataComments)
        {
            var approvalDates = new List<string>();
            var approvalUsers = new List<string>();
            var commentsByUser = new List<string>();
            int commentCount = 0;

            foreach (var commentsThread in dataComments.CommentThreads)
            {
                if (commentsThread.Comments == null) continue;

                foreach (var comment in commentsThread.Comments)
                {
                    if (comment.Content != null && comment.Content.Contains("voted 10"))
                    {
                        approvalDates.Add(Utility.ConvertToIST(comment.PublishedDate));
                        approvalUsers.Add(comment.Author.DisplayName);
                    }

                    if (comment.CommentType == "text" && !string.IsNullOrWhiteSpace(comment.Content))
                    {
                        commentsByUser.Add($"{comment.Author.DisplayName} - {comment.Content}");
                        commentCount++;
                    }
                }
            }
            int size = approvalUsers.Count;
            var approvalUser = "";
            var approvalDate = "";
            if (size == 0)
            {
                approvalUser = null;
                approvalDate = null;
            }
            else
            {
                approvalDate = approvalDates[size-1];
                approvalUser = approvalUsers[size-1];
            }
            return (approvalDate, approvalUser, commentCount, commentsByUser);
        }

    }
}
