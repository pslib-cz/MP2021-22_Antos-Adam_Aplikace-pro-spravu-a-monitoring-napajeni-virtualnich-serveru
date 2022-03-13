using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MMNVS.Data;
using MMNVS.Model;
using MMNVS.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<MyUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages(o => o.Conventions.AuthorizeFolder("/"));

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IVMService, VMService>();
builder.Services.AddTransient<IUPSService, UPSService>();
builder.Services.AddTransient<IDbService, DbService>();
builder.Services.AddTransient<IServerService, ServerService>();
builder.Services.AddTransient<IScenarioService, ScenarioService>();
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddHostedService<TimedHostedService>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
