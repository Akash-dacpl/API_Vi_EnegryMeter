using DATA.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace DATA
{
    public class ContextClass : DbContext
    {
        public ContextClass(DbContextOptions<ContextClass> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<EM_Details> EM_Details { get; set; }
        public DbSet<EM_Master> eM_Masters { get; set; }

    }
}
