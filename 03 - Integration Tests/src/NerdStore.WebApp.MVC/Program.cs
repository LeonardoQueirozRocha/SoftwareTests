using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NerdStore.Catalog.Data;
using NerdStore.Catalog.Data.Repositories;
using NerdStore.Catalog.Domain.Interfaces.Repositories;
using NerdStore.Catalog.Domain.Interfaces.Services;
using NerdStore.Catalog.Domain.Services;
using NerdStore.Sales.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CatalogContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<SalesContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

// Catalog
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<CatalogContext>();

// Sales
builder.Services.AddScoped<SalesContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
