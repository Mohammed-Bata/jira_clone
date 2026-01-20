using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class CommentReaction
    {
        public int CommentId { get; set; }
        public string UserId { get; set; }
        public ReactionType ReactionType { get; set; }
    }

    public enum ReactionType
    {
        Like = 1,
        Love = 2,
        Laugh = 3,
        Wow = 4,
        Sad = 5,
        Angry = 6
    }
}
