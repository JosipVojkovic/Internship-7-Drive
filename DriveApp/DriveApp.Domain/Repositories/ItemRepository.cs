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
    }
}
