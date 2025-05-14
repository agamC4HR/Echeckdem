using Echeckdem.Models;

using Echeckdem.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Echeckdem.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

//// Forcing development mode
//builder.Host.UseEnvironment("Development");

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession();

// Add session services
builder.Services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // Make the session cookie essential
});


// Dependency Injection for services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<OrganisationSetupService>();
builder.Services.AddScoped<BOCWService>();
builder.Services.AddScoped<ReturnsService>();
builder.Services.AddScoped<ContributionService>();
builder.Services.AddScoped<RegistrationService>();
builder.Services.AddScoped<IBulkUploadService, BulkUploadService>();
builder.Services.AddScoped<ActScopeSetupService>();
builder.Services.AddScoped<IUserManagementService, UserManagementService>();
builder.Services.AddScoped<DetailViewCombinedService>();
builder.Services.AddSingleton<MailService>();
builder.Services.AddScoped<TrackerService>();
builder.Services.AddScoped<ISiteManagementService, SiteManagementService>();
builder.Services.AddScoped<IStateTemplateService, StateTemplateService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ProjectBocwService>();
builder.Services.AddScoped<IAudtrail, AudtrailUpdate>();
builder.Services.AddScoped<IDashboardSummary, DashboardSummary>();
// Configuring the Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionStrings");              //ECHECK
builder.Services.AddDbContext<DbEcheckContext>(options =>
options.UseSqlServer(connectionString));

// Congifuring JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Shows detailed errors
}
else
{
    app.UseDeveloperExceptionPage();

    //app.UseExceptionHandler("/Home/Error");

    //// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();                           
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
 