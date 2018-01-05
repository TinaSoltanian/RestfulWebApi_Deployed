
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Entities
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MyDBContext>
    {
        public MyDBContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<MyDBContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);
            return new MyDBContext(builder.Options);
        }
    }

    public class MyDBContext : DbContext 
    {
        public MyDBContext()
        {

        }
        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
    }
}
