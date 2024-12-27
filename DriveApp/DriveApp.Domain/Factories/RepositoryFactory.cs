using DriveApp.Domain.Repositories;

namespace DriveApp.Domain.Factories
{
    public class RepositoryFactory
    {
        public static TRepository Create<TRepository>()
        where TRepository : BaseRepository
        {
            var dbContext = DbContextFactory.GetDriveAppDbContext();
            var repositoryInstance = Activator.CreateInstance(typeof(TRepository), dbContext) as TRepository;

            return repositoryInstance!;
        }
    }
}
