using DriveApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DriveApp.Data.Entities.Models
{
    public class Folder : Item
    {
        public Folder(string name, int? parentId, int ownerId) : base(name, parentId, ownerId)
        {
        }
        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
