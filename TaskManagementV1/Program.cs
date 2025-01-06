using Microsoft.AspNetCore.Authentication.Cookies;
using TaskManagementV1.Controllers;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDistributedMemoryCache();  // Use in-memory cache for storing session data

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // Set session timeout (30 minutes)
    options.Cookie.HttpOnly = true;  // Secure the session cookie
    options.Cookie.IsEssential = true;  // Make sure session is essential for the app's functionality
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // Register IHttpContextAccessor to allow access to HttpContext (for session management)
builder.Services.AddScoped<CommonController>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseSession();
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
    pattern: "{controller=Auth}/{action=Index}/{id?}");

app.Run();
