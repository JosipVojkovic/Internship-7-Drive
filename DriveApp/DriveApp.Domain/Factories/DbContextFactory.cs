using DriveApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace DriveApp.Domain.Factories
{
    public static class DbContextFactory
    {
        public static DriveAppDbContext GetDriveAppDbContext()
        {
            var options = new DbContextOptionsBuilder()
                .UseNpgsql(ConfigurationManager.ConnectionStrings["DriveApp"].ConnectionString)
                .Options;

            return new DriveAppDbContext(options);
        }
    }
}
