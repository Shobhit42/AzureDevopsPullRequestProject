using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDevopsPullRequestProject.Models
{
    public class PullRequestResponse
    {
        [JsonProperty("value")]
        public List<PullRequest> PullRequests { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }
    }

    public class PullRequest
    {
        [JsonProperty("pullRequestId")]
        public int Id { get; set; }

        [JsonProperty("creationDate")]
        public DateTime CreationDate { get; set; }

        [JsonProperty("closedDate")]
        public DateTime? ClosedDate { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("sourceRefName")]
        public string SourceBranch { get; set; }

        [JsonProperty("targetRefName")]
        public string TargetBranch { get; set; }

        [JsonProperty("reviewers")]
        public List<Reviewer> Reviewers { get; set; }
    }

    public class WorkItemLink
    {
        [JsonProperty("href")]
        public string Href { get; set; }
    }

    public class WorkItemResponse
    {
        [JsonProperty("value")]
        public List<WorkItem> WorkItems { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }
    }

    public class WorkItem
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class ReviewerListResponse
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("value")]
        public List<Reviewer> Reviewers { get; set; }
    }

    public class Reviewer
    {
        [JsonProperty("vote")]
        public int Vote { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
    }

    public class CommentThreadResponse
    {
        [JsonProperty("value")]
        public List<CommentThread> CommentThreads { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }
    }

    public class CommentThread
    {
        [JsonProperty("comments")]
        public List<Comment> Comments { get; set; }
    }

    public class Comment
    {
        [JsonProperty("author")]
        public Author Author { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("commentType")]
        public string CommentType { get; set; }

        [JsonProperty("publishedDate")]
        public DateTime PublishedDate { get; set; }
    }

    public class Author
    {
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
    }
}
