using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveApp.Data.Entities.Models
{
    public class SharedItem
    {
        public SharedItem(int itemId, int ownerId, int sharedUserId) 
        {
            ItemId = itemId;
            OwnerId = ownerId;
            SharedUserId = sharedUserId;
        }

        public int Id { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
        public int SharedUserId { get; set; }
    }
}
