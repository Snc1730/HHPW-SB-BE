using HHPW_SB_BE;
using HHPW_SB_BE.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:7027",
                                              "http://localhost:3000")
                                               .AllowAnyHeader()
                                               .AllowAnyMethod();
                      });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// allows passing datetimes without time zone data 
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// allows our api endpoints to access the database through Entity Framework Core
builder.Services.AddNpgsql<HHPWDbContext>(builder.Configuration["HHPWDbConnectionString"]);

// Set the JSON serializer options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// Employee Endpoints

// Check if employee exists
app.MapGet("/checkemployee/{uid}", (HHPWDbContext db, string uid) =>
{
    var employee = db.Employees.Where(x => x.Uid == uid).ToList();
    if (uid == null)
    {
        return Results.NotFound();
    }
    else
    {
        return Results.Ok(employee);
    }
});

//Create Employee
app.MapPost("/api/employee", (HHPWDbContext db, Employee employee) =>
{
    db.Employees.Add(employee);
    db.SaveChanges();
    return Results.Created($"/api/user/{employee.Id}", employee);
});

//Get single Employee by id
app.MapGet("/api/employee/{id}", (HHPWDbContext db, int id) =>
{
    var user = db.Employees.Single(u => u.Id == id);
    return user;
});

//MenuItem Endpoints

//Create MenuItem
app.MapPost("/api/menuitem", (HHPWDbContext db, MenuItem menuItem) =>
{
    db.MenuItems.Add(menuItem);
    db.SaveChanges();
    return Results.Created($"/api/menuitem/{menuItem.Id}", menuItem);
});

//Get All MenuItems
app.MapGet("/api/menuitems", (HHPWDbContext db) =>
{
    var menuItems = db.MenuItems.ToList();
    return Results.Ok(menuItems);
});

//Get single MenuItem by id
app.MapGet("/api/menuitem/{id}", (HHPWDbContext db, int id) =>
{
    var menuItem = db.MenuItems.SingleOrDefault(mi => mi.Id == id);
    if (menuItem == null)
    {
        return Results.NotFound();
    }
    else
    {
        return Results.Ok(menuItem);
    }
});

//Update MenuItem
app.MapPut("/api/menuitem/{id}", (HHPWDbContext db, int id, MenuItem updatedMenuItem) =>
{
    var existingMenuItem = db.MenuItems.SingleOrDefault(mi => mi.Id == id);
    if (existingMenuItem == null)
    {
        return Results.NotFound();
    }

    // Update the menu item properties
    existingMenuItem.Name = updatedMenuItem.Name;
    existingMenuItem.Description = updatedMenuItem.Description;
    // Update other properties as needed...

    db.SaveChanges();
    return Results.Ok();
});

//Delete MenuItem
app.MapDelete("/api/menuitem/{id}", (HHPWDbContext db, int id) =>
{
    var menuItem = db.MenuItems.SingleOrDefault(mi => mi.Id == id);
    if (menuItem == null)
    {
        return Results.NotFound();
    }

    db.MenuItems.Remove(menuItem);
    db.SaveChanges();
    return Results.NoContent();
});

//Order Endpoints

//Create Order
app.MapPost("/api/order", (HHPWDbContext db, Order order) =>
{
    db.Orders.Add(order);
    db.SaveChanges();
    return Results.Created($"/api/order/{order.Id}", order);
});

//Get all orders
app.MapGet("/api/orders", (HHPWDbContext db) =>
{
    var orders = db.Orders.ToList();
    return Results.Ok(orders);
});


//Get single order by id
app.MapGet("/api/order/{id}", (HHPWDbContext db, int id) =>
{
    var order = db.Orders.SingleOrDefault(o => o.Id == id);
    if (order == null)
    {
        return Results.NotFound();
    }
    else
    {
        return Results.Ok(order);
    }
});


// Function to get the price of a menu item
decimal GetMenuItemPrice(HHPWDbContext db, int menuItemId)
{
    var menuItem = db.MenuItems.SingleOrDefault(mi => mi.Id == menuItemId);
    if (menuItem == null)
    {
        // Handle the case where the menu item doesn't exist (e.g., return a default price)
        return 0; // Replace with an appropriate default price
    }

    return menuItem.Price; // Assuming that the menu item has a "Price" property
}

// Update an order
app.MapPut("api/order/{id}", (int id, HHPWDbContext db, Order updatedOrder) =>
{
    var existingOrder = db.Orders.SingleOrDefault(o => o.Id == id);
    if (existingOrder == null)
    {
        return Results.NotFound();
    }

    // Update the order properties based on the data in the request
    existingOrder.OrderName = updatedOrder.OrderName;
    existingOrder.OrderType = updatedOrder.OrderType;
    existingOrder.PaymentType = updatedOrder.PaymentType;
    existingOrder.CustomerName = updatedOrder.CustomerName;
    existingOrder.CustomerEmail = updatedOrder.CustomerEmail;
    existingOrder.CustomerPhone = updatedOrder.CustomerPhone;
    existingOrder.Tip = updatedOrder.Tip; 
    existingOrder.Review = updatedOrder.Review; 
    existingOrder.OrderStatus = updatedOrder.OrderStatus;
    existingOrder.DateClosed = updatedOrder.DateClosed;
    existingOrder.TotalOrderAmount = updatedOrder.TotalOrderAmount;

    // Recalculate OrderPrice based on updated MenuItemQuantities
    if (updatedOrder.MenuItemQuantities != null)
    {
        decimal totalOrderPrice = 0;

        foreach (var menuItemQuantity in updatedOrder.MenuItemQuantities)
        {
            decimal menuItemPrice = GetMenuItemPrice(db, menuItemQuantity.Key);

            // Calculate the total price for this menu item and quantity
            totalOrderPrice += menuItemPrice * menuItemQuantity.Value;
        }

        existingOrder.OrderPrice = totalOrderPrice;
    }

    // You can update other properties here...

    db.SaveChanges();
    return Results.Ok();
});

//Delete Order
app.MapDelete("/api/order/{id}", (HHPWDbContext db, int id) =>
{
    var order = db.Orders.SingleOrDefault(o => o.Id == id);
    if (order == null)
    {
        return Results.NotFound();
    }

    db.Orders.Remove(order);
    db.SaveChanges();
    return Results.NoContent();
});

//OrderMenuItem Endpoints

//Get Order MenuItems for a specific order
app.MapGet("/api/orders/{orderId}/orderitems", (HHPWDbContext db, int orderId) =>
{
    var order = db.Orders.Include(o => o.MenuItems).SingleOrDefault(o => o.Id == orderId);
    if (order == null)
    {
        return Results.NotFound("Order not found.");
    }

    var orderItems = order.MenuItems;
    return Results.Ok(orderItems);
});

// Associate Menu Item with an Order
app.MapPost("/api/order/{orderId}/menuitem/{menuItemId}", (HHPWDbContext db, int orderId, int menuItemId) =>
{
    try
    {
        // Retrieve the order from the database
        Order order = db.Orders.FirstOrDefault(o => o.Id == orderId);
        if (order == null)
            return Results.NotFound("Order not found.");

        // Retrieve the menu item from the database
        MenuItem menuItem = db.MenuItems.FirstOrDefault(mi => mi.Id == menuItemId);
        if (menuItem == null)
            return Results.NotFound("Menu item not found.");

        // Ensure the order's MenuItems collection is initialized
        if (order.MenuItems == null)
            order.MenuItems = new List<MenuItem>();

        // Add the menu item to the order
        order.MenuItems.Add(menuItem);

        // Save changes to the database
        db.SaveChanges();

        return Results.Ok("Menu item associated with the order successfully.");
    }
    catch (Exception ex)
    {
        return Results.Problem("An error occurred while associating menu item with order.", ex.Message);
    }
});


// delete a product from an order 
app.MapDelete("/api/MenuItemsOrder", (int orderId, int itemId, HHPWDbContext db) =>
{
    var item = db.MenuItems.Include(m => m.Orders).FirstOrDefault(m => m.Id == itemId);

    if (item == null)
    {
        return Results.NotFound();
    }

    var orderToRemove = item.Orders.FirstOrDefault(o => o.Id == orderId);

    if (orderToRemove == null)
    {
        return Results.NotFound();
    }

    item.Orders.Remove(orderToRemove);
    db.SaveChanges();

    return Results.Ok("Item removed from order successfully");
});

app.Run();
