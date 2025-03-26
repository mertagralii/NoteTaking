﻿using Microsoft.EntityFrameworkCore;
using NoteTaking.Models;

namespace NoteTaking.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Category> Categorys { get; set; }

        public DbSet<Note> Notes { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
