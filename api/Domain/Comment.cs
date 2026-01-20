using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string AuthorUserId { get; set; }
        public int WorkItemId { get; set; }
        public int? ParentCommentId { get; set; }
    }
}
