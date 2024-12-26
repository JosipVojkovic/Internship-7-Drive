using DriveApp.Data.Entities;
using DriveApp.Data.Entities.Models;
using DriveApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveApp.Domain.Repositories
{
    public class SharedItemRepository : BaseRepository
    {
        public SharedItemRepository(DriveAppDbContext dbContext) : base(dbContext)
        {
        }

        public ResponseResultType Add(int itemId, int ownerId, int sharedUserId)
        {
            if(DbContext.SharedItems
                .Any(s => s.ItemId == itemId && s.OwnerId == ownerId && s.SharedUserId == sharedUserId))
            {
                return ResponseResultType.AlreadyExists;
            }

            var newSharedItem = new SharedItem(itemId, ownerId, sharedUserId);
            DbContext.SharedItems.Add(newSharedItem);
            
            return SaveChanges();
        }

        public ResponseResultType Delete(int itemId, int ownerId, int sharedUserId) 
        {
            var sharedItem = DbContext.SharedItems
                .FirstOrDefault(s => s.ItemId == itemId && s.OwnerId == ownerId && s.SharedUserId == sharedUserId);

            if (sharedItem is null)
                return ResponseResultType.NotFound;

            DbContext.SharedItems.Remove(sharedItem);

            return SaveChanges();
        }

        public ResponseResultType Delete(int itemId, int sharedUserId)
        {
            var sharedItem = DbContext.SharedItems
                .FirstOrDefault(s => s.ItemId == itemId && s.SharedUserId == sharedUserId);

            if (sharedItem is null)
                return ResponseResultType.NotFound;

            DbContext.SharedItems.Remove(sharedItem);

            return SaveChanges();
        }

    }

}
