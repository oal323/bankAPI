using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.SqlServer;
using Pomelo.EntityFrameworkCore.MySql;



namespace bankAPI.Models
{
    
    public class BankContext : DbContext
    {
        
    
    public DbSet<User> User { get; set; }
    public DbSet<Account> Account { get; set; }
    public DbSet<Transaction> Transaction {get; set; }
    public BankContext(DbContextOptions<BankContext> options)
            : base(options)
        {
        }
    public BankContext(){}
    protected void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      modelBuilder.Entity<User>(entity =>
      {
        entity.HasKey(e => e.id);
        entity.Property(e => e.last).IsRequired();
        entity.Property(e => e.first).IsRequired();
        entity.Property(e=> e.AcctsDB);
        entity.HasMany(e => e.AcctsDB).WithOne();
      });
      modelBuilder.Entity<Account>(entity =>
      {
        entity.HasKey(i=>i.id);
        entity.Property(e => e.userid).IsRequired();
        entity.Property(e => e.accountType);
        entity.HasMany(e => e.transactions);
      });

      
      modelBuilder.Entity<Transaction>(entity =>
      {
        entity.HasKey(e => e.id);
        entity.Property(e => e.sourceID).IsRequired();
        entity.Property(e => e.destID).IsRequired();
        entity.Property(e => e.amount).IsRequired();
      });
    }

  }
  
} 
