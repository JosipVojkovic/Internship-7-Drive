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
    public class UserRepository : BaseRepository
    {
        public UserRepository(DriveAppDbContext dbContext) : base(dbContext)
        {
        }

        public ResponseResultType Add(string email, string password, string firstName, string lastName)
        {
            if(!DbContext.Users.Any(u => u.Email == email))
                return ResponseResultType.AlreadyExists;

            var newUser = new User(email, password, firstName, lastName);

            DbContext.Users.Add(newUser);

            return SaveChanges();
        }

        public ResponseResultType Delete(int id)
        {
            var userToDelete = DbContext.Users.Find(id);
            if (userToDelete is null)
            {
                return ResponseResultType.NotFound;
            }
            DbContext.Users.Remove(userToDelete);

            return SaveChanges();
        }

        public ResponseResultType Update(int id, string newEmail)
        {
            var user = DbContext.Users.Find(id);

            if (user is null)
                return ResponseResultType.NotFound;
            if(user.Email == newEmail)
                return ResponseResultType.AlreadyExists;

            user.Email = newEmail;

            return SaveChanges();
        }

        public ResponseResultType Update(int id, string newPassword, string confirmPassword)
        {
            if(newPassword != confirmPassword)
                return ResponseResultType.ValidationError;

            var user = DbContext.Users.Find(id);

            if (user is null)
                return ResponseResultType.NotFound;

            if (user.Password == newPassword)
                return ResponseResultType.AlreadyExists;

            user.Password = newPassword;

            return SaveChanges();
        }

        public User? GetUser(string email, string password)
        {
            var user = DbContext.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            return user;
        }

        public User? GetUser(string email)
        {
            var user = DbContext.Users.FirstOrDefault(u => u.Email == email);
            return user;
        }

        public ICollection<User> GetAllUsers()
        {
            return DbContext.Users.ToList();
        }
    }
}
