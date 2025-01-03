using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveApp.Data.Entities.Models
{
    public class Comment
    {
        public Comment(int fileId, int userId, string content, DateTime createdAt) 
        {
            FileId = fileId;
            UserId = userId;
            Content = content;
            CreatedAt = createdAt;
        }
        public Comment(int fileId, int userId, string content)
        {
            FileId = fileId;
            UserId = userId;
            Content = content;
            CreatedAt = DateTime.UtcNow;
        }
        public int Id { get; set; }
        public int FileId { get; set; }
        public File? File { get; set; }
        public int UserId { get; set; }
        public User? Owner { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
