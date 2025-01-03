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
    public class ItemRepository : BaseRepository
    {
        public ItemRepository(DriveAppDbContext dbContext) : base(dbContext) 
        {
        }
        public Item? GetById(int itemId)
        {
            return DbContext.Items
                .FirstOrDefault(i => i.Id == itemId);
        }

        public void GetItems(int userId, int? parentId, out List<Folder> folders, out List<Data.Entities.Models.File> files)
        {
            var items = DbContext.Items
                .Where(i => i.OwnerId == userId && i.ParentId == parentId)
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
