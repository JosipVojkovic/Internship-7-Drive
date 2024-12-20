using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveApp.Data.Entities.Models
{
    public class User
    { 
        public User(string email, string password, string firstName, string lastName)
        {
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
        }
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Item> Items { get; set; } = new List<Item>();
        public ICollection<SharedItem> SharedItems { get; set; } = new List<SharedItem>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
