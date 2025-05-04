using System;
using Microsoft.EntityFrameworkCore;
using FinalProject.BackEnd.Models.DomainModels.PersonAggregates;
using FinalProject.BackEnd.Models.DomainModels.ProductAggregates;
using FinalProject.BackEnd.Models.DomainModels.OrderAggregates.OrderHeaderAggregates;
using FinalProject.BackEnd.Models.DomainModels.OrderAggregates.OrderDetailsAggregates;

namespace FinalProject.BackEnd.Models
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>().HasKey(od => new { od.OrderHeaderId, od.ProductId });
            modelBuilder.Entity<OrderHeader>().HasKey(oh=>oh.Id);
            modelBuilder.Entity<Product>().HasKey(p=>p.Id);
            modelBuilder.Entity<Person>().HasKey(p => p.Id);
            modelBuilder.Entity<OrderHeader>(builder =>
            {
                builder.HasOne(e=>e.Seller)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
                builder.HasOne(e=>e.Buyer)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
                builder.HasKey(p => p.Id);

            }
            );

        }

        public DbSet<Person> Person { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

      
    }
}
