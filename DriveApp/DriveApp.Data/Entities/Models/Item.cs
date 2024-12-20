using DriveApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveApp.Data.Entities.Models
{
    public class Item
    {
        public Item(string name, int? parentId, int ownerId)
        {
            Name = name;
            ParentId = parentId;
            LastChanged = DateTime.Now;
            OwnerId = ownerId;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public DateTime LastChanged { get; set; }
        public int OwnerId { get; set; }
        public User? Owner { get; set; }
        public ICollection<SharedItem> SharedItems { get; set; } = new List<SharedItem>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
