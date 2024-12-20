using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveApp.Data.Entities.Models
{
    public class Comment
    {
        public Comment(int itemId, int userId, string content, DateTime createdAt) 
        {
            ItemId = itemId;
            UserId = userId;
            Content = content;
            CreatedAt = createdAt;
        }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public int UserId { get; set; }
        public User Owner { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
