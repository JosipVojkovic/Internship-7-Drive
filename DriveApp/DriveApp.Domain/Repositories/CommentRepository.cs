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
    public class CommentRepository : BaseRepository
    {
        public CommentRepository(DriveAppDbContext dbContext) : base(dbContext)
        {
        }

        public ResponseResultType Add(int fileId, int userId, string content)
        {
            var newComment = new Comment(fileId, userId, content);
            DbContext.Comments.Add(newComment);

            return SaveChanges();
        }

        public ResponseResultType Delete(int commentId)
        {
            var comment = DbContext.Comments.FirstOrDefault(c => c.Id == commentId);

            if (comment is null)
                return ResponseResultType.NotFound;

            DbContext.Comments.Remove(comment);

            return SaveChanges();
        }

        public ResponseResultType Delete(int fileId, int commentId)
        {
            var comment = DbContext.Comments.FirstOrDefault(c => c.Id == commentId && c.FileId == fileId);

            if (comment is null)
                return ResponseResultType.NotFound;

            DbContext.Comments.Remove(comment);

            return SaveChanges();
        }

        public ResponseResultType Update(int commentId, string content)
        {
            var comment = DbContext.Comments.FirstOrDefault(c => c.Id == commentId);

            if (comment is null)
                return ResponseResultType.NotFound;

            comment.Content = content;

            return SaveChanges();
        }

        public Comment? GetComment(int commentId)
        {
            return DbContext.Comments.FirstOrDefault(c => c.Id == commentId);    
        }

        public Comment? GetComment(int fileId, int commentId)
        {
            return DbContext.Comments.FirstOrDefault(c => c.Id == commentId && c.FileId == fileId);
        }

        public ICollection<Comment> GetComments(int fileId)
        {
            return DbContext.Comments.Where(c => c.FileId == fileId).ToList();
        }
    }
}
