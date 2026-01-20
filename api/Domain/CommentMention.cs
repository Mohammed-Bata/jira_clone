using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class CommentMention
    {
        public int CommentId { get; set; }
        public string MentionedUserId { get; set; }
    }
}
