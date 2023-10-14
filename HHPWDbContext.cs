using HHPW_SB_BE.Models;
using Microsoft.EntityFrameworkCore;

public class HHPWDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }

    public HHPWDbContext(DbContextOptions<HHPWDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MenuItem>().HasData(new MenuItem[]
        {
            new MenuItem {Id = 1, Description = "Delicious pizza", Name = "Pepperoni Pizza", Category = "Pizza", ImageUrl = "image_url_here", Price = 12.99M},
            new MenuItem {Id = 2, Description = "Crunchy chicken wings", Name = "Spicy Chicken Wings", Category = "Wings", ImageUrl = "image_url_here", Price = 9.99M},
        });

        modelBuilder.Entity<Order>().HasData(new Order[]
        {
            new Order
            {
                Id = 1,
                EmployeeId = 1, 
                OrderName = "First Order",
                DatePlaced = DateTime.Now,
                Tip = 5.00M,
                OrderPrice = 20.00M,
                TotalOrderAmount = 25.00M,
                OrderType = "In-Person",
                PaymentType = "Credit",
                Review = 4, 
                CustomerName = "John Doe",
                CustomerEmail = "john@example.com",
                CustomerPhone = "123-456-7890",
                OrderStatus = "Closed",
                DateClosed = DateTime.Now.AddDays(1),
            },
        });

        modelBuilder.Entity<Employee>().HasData(new Employee[]
        {
            new Employee
            {
                Id = 1,
                Name = "Sean Bryant",
                isEmployee = true
            },
        });

        modelBuilder.Entity<Order>()
            .HasMany(o => o.MenuItems)
            .WithMany(mi => mi.Orders)
            .UsingEntity(j => j.ToTable("OrderMenuItems"));

        base.OnModelCreating(modelBuilder);
    }
}