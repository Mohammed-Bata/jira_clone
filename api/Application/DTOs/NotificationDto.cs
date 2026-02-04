using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Messsage { get; set; }
        public string ActorName { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public bool IsRead { get; set; }
    }
}
