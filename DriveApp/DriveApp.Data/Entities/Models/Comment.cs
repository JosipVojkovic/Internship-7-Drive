using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveApp.Data.Entities.Models
{
    public class Comment
    {
        public Comment(int sharedItemId, int userId, string content, DateTime createdAt) 
        {
            SharedItemId = sharedItemId;
            UserId = userId;
            Content = content;
            CreatedAt = createdAt;
        }
        public int Id { get; set; }
        public int SharedItemId { get; set; }
        public SharedItem? SharedItem { get; set; }
        public int UserId { get; set; }
        public User? Owner { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
