using Infrastructure;
using Infrastructure.Bills;
using Infrastructure.Categories;
using Infrastructure.Entities;
using Infrastructure.Products;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//TNT91 config entity framework, identity framework
builder.Services.AddEntityFrameworkSqlServer();
builder.Services.AddDbContextPool<TechShopDbContext>
	(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<TechShopDbContext>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IBillServices, BillServices>();

builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddSession();
//    option =>
//{
//    option.Cookie.HttpOnly= true;
//    option.IdleTimeout = TimeSpan.FromHours(1);
//});


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
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
