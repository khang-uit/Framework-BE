using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Memoriesx.Data
{
    public class PostDbContextFactory : IDesignTimeDbContextFactory<MemoriesxDbContext>
    {
        public MemoriesxDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json")
             .Build();
            var connectionString = configurationRoot.GetConnectionString("MemoriesxDatabase");

            var optionBuilder = new DbContextOptionsBuilder<MemoriesxDbContext>();
            optionBuilder.UseSqlServer(connectionString);

            return new MemoriesxDbContext(optionBuilder.Options);
        }
    }
}
