using DriveApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveApp.Data.Entities.Models
{
    public class File : Item
    {
        public File(string name, int? parentId, int ownerId, string content) : base(name, parentId, ownerId)
        {
            Content = content;
        }

        public string Content { get; set; }
    }
}
