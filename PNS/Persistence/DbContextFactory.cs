// File Path: Persistence/DbContextFactory.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Persistence
{
    public class DbContextFactory : IDesignTimeDbContextFactory<PnsDbContext>
    {
        public PnsDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                // እዚህ ጋር ያለውን Path አስተካክለው! የAPI ፕሮጀክት appsettings.json ያለበትን ቦታ ነው መሆን ያለበት።
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../API"))
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<PnsDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new PnsDbContext(optionsBuilder.Options);
        }
    }
}