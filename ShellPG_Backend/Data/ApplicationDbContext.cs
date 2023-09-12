﻿using Microsoft.EntityFrameworkCore;
using ShellPG_Backend.Data.Model;

namespace ShellPG_Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
    }

}
