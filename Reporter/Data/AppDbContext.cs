using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reporter.Models;
using Microsoft.EntityFrameworkCore;

namespace Reporter.Data
{
    public class AppDbContext:DbContext
    {
        private readonly DbContextOptions _options;


        public AppDbContext(DbContextOptions options) : base(options)
        {
            _options = options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
        }
        
        public DbSet<SalesTable> SalesTable { get; set; }
    }
}
