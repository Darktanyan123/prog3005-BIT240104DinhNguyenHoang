using Lesson3.Data;
using Lesson3.Middleware;
using static Lesson3.Middleware.RequestLoggingMiddleware;
using BookManagement.Data;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BookDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("BookConnection")));
builder.Services.AddScoped<BookRepository>();
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BookDbContext>();

    db.Database.EnsureCreated();
}

app.UseMiddleware<RequestLoggingMiddleware>();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.UseMiddleware<CheckDatabaseMiddleware>();

app.UseMiddleware<DatabaseLogMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Book}/{action=Index}/{id?}");

app.Run();
