
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

        public ResponseResultType Add(string name, int? parentId, int ownerId)
        {
            if (DbContext.Folders.Any(f => f.ParentId == parentId && f.Name == name && f.OwnerId == ownerId))
                return ResponseResultType.AlreadyExists;

            var newFolder = new Folder(name, parentId, ownerId);
            DbContext.Folders.Add(newFolder);

            return SaveChanges();
        }

        public ResponseResultType Delete(string name, int? parentId, int ownerId)
        {
            var folder = DbContext.Folders
                .FirstOrDefault(f => f.Name == name && f.ParentId == parentId && f.OwnerId == ownerId); 

            if (folder is null)
                return ResponseResultType.NotFound;

            DbContext.Folders.Remove(folder);

            return SaveChanges();
        }

        public ResponseResultType Update(Folder folder, string newName)
        {
            folder.Name = newName;
            folder.LastChanged = DateTime.UtcNow;

            return SaveChanges();
        }

        public Folder? GetFolder(string name, int? parentId, int ownerId)
        {
            return DbContext.Folders
                .FirstOrDefault(f => f.Name == name && f.ParentId == parentId && f.OwnerId == ownerId);
        }

        public Folder? GetById(int? id)
        {
            return DbContext.Folders
                .FirstOrDefault(f => f.Id == id);
        }

        public ICollection<Folder> GetFolders(int userId, int? parentId)
        {
            return DbContext.Folders
                .Where(f => f.OwnerId == userId && f.ParentId == parentId)
                .OrderBy(f => f.Name)
                .ToList();
        }

        public ICollection<Folder> GetFolders(int? parentId)
        {
            return DbContext.Folders
                .Where(f => f.ParentId == parentId)
                .OrderBy(f => f.Name)
                .ToList();
        }
    }
}
