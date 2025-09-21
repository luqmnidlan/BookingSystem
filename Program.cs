using BookingSystem.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Db Context - Support both SQL Server and PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("BookingSystem");
if (connectionString.Contains("Host="))
{
    // PostgreSQL connection
    builder.Services.AddDbContext<ApplicationDbContext>(options => options
        .UseNpgsql(connectionString));
}
else
{
    // SQL Server connection
    builder.Services.AddDbContext<ApplicationDbContext>(options => options
        .UseSqlServer(connectionString));
}

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
