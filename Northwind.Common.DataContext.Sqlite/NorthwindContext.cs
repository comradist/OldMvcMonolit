using Microsoft.EntityFrameworkCore;

namespace Packt.Shared;

public partial class NorthwindContext : DbContext
{
    public NorthwindContext()
    {
    }

    public NorthwindContext(DbContextOptions<NorthwindContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeTerritory> EmployeeTerritories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Shipper> Shippers { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Territory> Territories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
    {
        if(!optionsBuilder.IsConfigured)
        {
            string dir = Environment.CurrentDirectory;
            string path = string.Empty;

            if(dir.EndsWith("net7.0"))
            {
                path = Path.Combine("..", "..", "..", "..", "Northwind.db");
            }
            else
            {
                path = Path.Combine("..", "Northwind.db");
            }

            optionsBuilder.UseSqlite($"Filename={path}");
        }
    }
        //=> optionsBuilder.UseSqlite("Filename=../Northwind.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.UnitPrice).HasConversion<double>();
        });

        // modelBuilder.Entity<OrderDetail>(entity =>
        // {
            
        //     entity.HasKey(e => new { e.OrderId, e.ProductId });//may not be here

        //     // entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails).OnDelete(DeleteBehavior.ClientSetNull);

        //     // entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails).OnDelete(DeleteBehavior.ClientSetNull);
        // });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
