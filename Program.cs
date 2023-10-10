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
                          policy.WithOrigins("https://localhost:7284",
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
    var employee = db.Employees.SingleOrDefault(x => x.Uid == uid);
    if (employee == null)
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
    return Results.Created($"/api/employee/{employee.Id}", employee);
});

//Get single Employee by id
app.MapGet("/api/employee/{id}", (HHPWDbContext db, int id) =>
{
    var employee = db.Employees.SingleOrDefault(e => e.Id == id);
    if (employee == null)
    {
        return Results.NotFound();
    }
    else
    {
        return Results.Ok(employee);
    }
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

//Update Order
app.MapPut("/api/order/{id}", (HHPWDbContext db, int id, Order updatedOrder) =>
{
    var existingOrder = db.Orders.SingleOrDefault(o => o.Id == id);
    if (existingOrder == null)
    {
        return Results.NotFound();
    }

    // Update the order properties
    existingOrder.OrderName = updatedOrder.OrderName;
    existingOrder.Tip = updatedOrder.Tip;
    // Update other properties as needed...

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

//Associate Menu Item with an Order
app.MapPost("/api/order/{orderId}/menuitem/{menuItemId}", (HHPWDbContext db, int orderId, int menuItemId) =>
{
    var order = db.Orders.SingleOrDefault(o => o.Id == orderId);
    if (order == null)
    {
        return Results.NotFound("Order not found.");
    }

    var menuItem = db.MenuItems.SingleOrDefault(mi => mi.Id == menuItemId);
    if (menuItem == null)
    {
        return Results.NotFound("Menu item not found.");
    }

    order.MenuItems.Add(menuItem);
    db.SaveChanges();

    return Results.Ok("Menu item associated with the order successfully.");
});

//Disassociate Menu Item from an Order
app.MapDelete("/api/order/{orderId}/menuitem/{menuItemId}", (HHPWDbContext db, int orderId, int menuItemId) =>
{
    var order = db.Orders.SingleOrDefault(o => o.Id == orderId);
    if (order == null)
    {
        return Results.NotFound("Order not found.");
    }

    var menuItem = db.MenuItems.SingleOrDefault(mi => mi.Id == menuItemId);
    if (menuItem == null)
    {
        return Results.NotFound("Menu item not found.");
    }

    order.MenuItems.Remove(menuItem);
    db.SaveChanges();

    return Results.Ok("Menu item disassociated from the order successfully.");
});

app.Run();
