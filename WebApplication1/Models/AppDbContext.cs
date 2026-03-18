using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User - Order (1-n)
        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId);

        // Order - OrderDetail (1-n)
        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Order)
            .WithMany(o => o.OrderDetails)
            .HasForeignKey(od => od.OrderId);

        // Item - OrderDetail (1-n)
        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Item)
            .WithMany(i => i.OrderDetails)
            .HasForeignKey(od => od.ItemId);
    }
}