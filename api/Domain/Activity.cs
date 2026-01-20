using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Activity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string Action { get; set; }
        public int ItemId { get; set; }
        public ActivityItemType ItemType { get; set; }
    }

    public enum ActivityItemType
    {
        commentMention = 1,
        commentReply = 2,
        taskAssigned = 3,
        taskStatusChanged = 4
    }
}
