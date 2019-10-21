using System;
using Microsoft.EntityFrameworkCore;

namespace Blobzor.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        
        }   

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Configuration.BusinessObjectConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}