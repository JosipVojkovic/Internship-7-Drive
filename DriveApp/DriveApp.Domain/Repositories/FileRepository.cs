﻿using DriveApp.Data.Entities;
using DriveApp.Data.Entities.Models;
using DriveApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveApp.Domain.Repositories
{
    public class FileRepository : BaseRepository
    {
        public FileRepository(DriveAppDbContext dbContext) : base(dbContext) 
        {
        }

        public ResponseResultType Add(string name, int? parentId, int ownerId, string content)
        {
            if (DbContext.Files.Any(f => f.ParentId == parentId && f.Name == name && f.OwnerId == ownerId))
                return ResponseResultType.AlreadyExists;

            var newFile = new Data.Entities.Models.File(name, parentId, ownerId, content);
            DbContext.Files.Add(newFile);

            return SaveChanges();
        }

        public ResponseResultType Delete(string name, int? parentId, int ownerId)
        {
            var file = DbContext.Files
                .FirstOrDefault(f => f.Name == name && f.ParentId == parentId && f.OwnerId == ownerId);

            if (file is null)
                return ResponseResultType.NotFound;

            DbContext.Files.Remove(file);

            return SaveChanges();
        }

        public ResponseResultType Update(Data.Entities.Models.File file, string newName)
        {
            file.Name = newName;
            file.LastChanged = DateTime.UtcNow;

            return SaveChanges();
        }

        public ResponseResultType UpdateContent(Data.Entities.Models.File file, string content)
        {
            file.Content = content;
            file.LastChanged = DateTime.UtcNow;

            return SaveChanges();
        }

        public Data.Entities.Models.File? GetFile(string name, int? parentId, int ownerId)
        {
            return DbContext.Files
                .FirstOrDefault(f => f.Name == name && f.ParentId == parentId && f.OwnerId == ownerId);
        }

        public Data.Entities.Models.File? GetFile(string name, int? parentId)
        {
            return DbContext.Files
                .FirstOrDefault(f => f.Name == name && f.ParentId == parentId);
        }

        public Data.Entities.Models.File? GetById(int fileId)
        {
            return DbContext.Files
                .FirstOrDefault(f => f.Id == fileId);
        }

        public ICollection<Data.Entities.Models.File> GetFiles(int userId, int? parentId)
        {
            return DbContext.Files
                .Where(f => f.OwnerId == userId && f.ParentId == parentId)
                .OrderBy(f => f.LastChanged)
                .ToList();
        }

        public ICollection<Data.Entities.Models.File> GetFiles(int? parentId)
        {
            return DbContext.Files
                .Where(f => f.ParentId == parentId)
                .OrderBy(f => f.LastChanged)
                .ToList();
        }

        public bool isFile(int id)
        {
            var file = DbContext.Files.Find(id);

            return file is null? false: true;
        }
    }
}
