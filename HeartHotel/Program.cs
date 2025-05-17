using HeartHotel.Models;
using Microsoft.EntityFrameworkCore;
using HeartHotel.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register SignalRHub
builder.Services.AddSingleton<SignalRHub>();

var connectionString = builder.Configuration.GetConnectionString("EventisContext");
builder.Services.AddDbContext<EventisContext>(options =>
    options.UseSqlServer(connectionString));

// builder.Services.AddSingleton(connectionString ?? "");

builder.Services.AddSignalR();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(3); // Set session timeout to 3 days
});

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

app.UseSession(); // Enable session middleware

app.MapHub<HeartHotel.Hubs.SignalRHub>("/signalrHub");

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
