
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
    public class FolderRepository : BaseRepository
    {
        public FolderRepository(DriveAppDbContext dbContext) : base(dbContext)
        {
        }

        public ResponseResultType Add(string name, int parentId, int ownerId)
        {
            if (DbContext.Folders.Any(f => f.ParentId == parentId && f.Name == name && f.OwnerId == ownerId))
                return ResponseResultType.AlreadyExists;

            var newFolder = new Folder(name, parentId, ownerId);

            return SaveChanges();
        }

        public ResponseResultType Delete(string name, int parentId, int ownerId)
        {
            var folder = DbContext.Folders
                .FirstOrDefault(f => f.Name == name && f.ParentId == parentId && f.OwnerId == ownerId); 

            if (folder is null)
                return ResponseResultType.NotFound;

            DbContext.Folders.Remove(folder);

            return SaveChanges();
        }
    }
}
