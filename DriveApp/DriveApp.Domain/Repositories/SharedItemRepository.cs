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

            var item = DbContext.Items.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
            {
                return ResponseResultType.NotFound;
            }

            AddSharedItemRecursive(item, ownerId, sharedUserId);
            
            return SaveChanges();
        }

        private void AddSharedItemRecursive(Item item, int ownerId, int sharedUserId)
        {
            if (!DbContext.SharedItems.Any(s => s.ItemId == item.Id && s.OwnerId == ownerId && s.SharedUserId == sharedUserId))
            {
                var sharedItem = new SharedItem(item.Id, ownerId, sharedUserId);
                DbContext.SharedItems.Add(sharedItem);
            }

            if (item is Folder)
            {
                var children = DbContext.Items.Where(i => i.ParentId == item.Id).ToList();
                foreach (var child in children)
                {
                    AddSharedItemRecursive(child, ownerId, sharedUserId);
                }
            }
        }

        public ResponseResultType Delete(int itemId, int ownerId, int sharedUserId) 
        {
            var sharedItem = DbContext.SharedItems
                .FirstOrDefault(s => s.ItemId == itemId && s.OwnerId == ownerId && s.SharedUserId == sharedUserId);

            if (sharedItem is null)
                return ResponseResultType.NotFound;

            var item = DbContext.Items.FirstOrDefault(i => i.Id == itemId);
            if (item is null)
                return ResponseResultType.NotFound;

            DeleteSharedItemRecursive(item, ownerId, sharedUserId);

            DbContext.SharedItems.Remove(sharedItem);

            return SaveChanges();
        }

        private void DeleteSharedItemRecursive(Item item, int ownerId, int sharedUserId)
        {
            var sharedItem = DbContext.SharedItems
                .FirstOrDefault(s => s.ItemId == item.Id && s.OwnerId == ownerId && s.SharedUserId == sharedUserId);

            if (sharedItem != null)
            {
                DbContext.SharedItems.Remove(sharedItem);
            }

            if (item is Folder)
            {
                var children = DbContext.Items.Where(i => i.ParentId == item.Id).ToList();
                foreach (var child in children)
                {
                    DeleteSharedItemRecursive(child, ownerId, sharedUserId);
                }
            }
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

        public ICollection<Folder> GetSharedFolders(int sharedUserId)
        {
            var sharedItems = DbContext.SharedItems
                .Where(si => si.SharedUserId == sharedUserId);
            var sharedFolders = DbContext.Folders
                .Where(f => sharedItems.Any(si => si.ItemId == f.Id))
                .OrderBy(sf => sf.Name)
                .ToList();

            return sharedFolders;
        }

        public ICollection<Data.Entities.Models.File> GetSharedFiles(int sharedUserId)
        {
            var sharedItems = DbContext.SharedItems
                .Where(si => si.SharedUserId == sharedUserId);
            var sharedFiles = DbContext.Files
                .Where(f => sharedItems.Any(si => si.ItemId == f.Id))
                .OrderBy(sf => sf.Name)
                .ToList();

            return sharedFiles;
        }

        public Item? GetSharedItem(int sharedUserId, int itemId)
        {
            var sharedItem = DbContext.SharedItems
                .FirstOrDefault(si => si.SharedUserId == sharedUserId && si.ItemId == itemId);

            if (sharedItem == null)
                return null;

            var item = DbContext.Items
                .FirstOrDefault(i => i.Id == sharedItem.ItemId);

            return item;
        }

        public SharedItem? GetItem(int itemId, int userId)
        {
            var sharedItem = DbContext.SharedItems.FirstOrDefault(si => si.ItemId == itemId && si.OwnerId == userId);

            return sharedItem;
        }

        public void GetRootSharedItems(int userId, out List<Folder> folders, out List<Data.Entities.Models.File> files)
        {
            var userSharedItems = DbContext.SharedItems
                .Where(s => s.SharedUserId == userId)
                .ToList();

            var sharedItemIds = userSharedItems.Select(s => s.ItemId).ToHashSet();

            var rootSharedItems = userSharedItems
                .Where(sharedItem =>
                {
                    var currentItem = DbContext.Items.FirstOrDefault(i => i.Id == sharedItem.ItemId);
                    while (currentItem?.ParentId != null)
                    {
                        if (sharedItemIds.Contains(currentItem.ParentId.Value))
                        {
                            return false;
                        }
                        currentItem = DbContext.Items.FirstOrDefault(i => i.Id == currentItem.ParentId.Value);
                    }
                    return true;
                })
                .ToList();

            var itemIds = rootSharedItems.Select(s => s.ItemId).ToList();
            var items = DbContext.Items
                .Where(i => itemIds.Contains(i.Id))
                .ToList();

            folders = items
                .OfType<Folder>()
                .OrderBy(f => f.Name)
                .ToList();

            files = items
                .OfType<Data.Entities.Models.File>()
                .OrderBy(f => f.LastChanged)
                .ToList();
        }
    }

}
