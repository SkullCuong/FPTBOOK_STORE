using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FPTBOOK_STORE.Models;

public class MvcContext : DbContext
{
    public MvcContext(DbContextOptions<MvcContext> options)
        : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FPTBOOK_STORE.Models.Book>()
        .Property(p => p.Price).HasColumnType("decimal(18,4)");
        modelBuilder.Entity<FPTBOOK_STORE.Models.Category>()
        .Property(p =>p.Status).HasDefaultValue(false);

    }

    public DbSet<FPTBOOK_STORE.Models.Category> Category { get; set; } = default!;

    public DbSet<FPTBOOK_STORE.Models.Publisher>? Publisher { get; set; }

    public DbSet<FPTBOOK_STORE.Models.Author>? Author { get; set; }

    public DbSet<FPTBOOK_STORE.Models.Book>? Book { get; set; }

    public DbSet<FPTBOOK_STORE.Models.User>? User { get; set; }

    public DbSet<FPTBOOK_STORE.Models.Order>? Order { get; set; }

    public DbSet<FPTBOOK_STORE.Models.OrderDetail>? OrderDetail { get; set; }
}
