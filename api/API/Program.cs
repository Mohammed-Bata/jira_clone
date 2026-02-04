using Application;
using Azure.Core;
using Infrastructure;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddSignalR();


builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

var key = builder.Configuration["ApiSettings:Secret"];
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = true;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidIssuer = "API",
        ValidateAudience = false,
        ValidAudience = "Client",
        ClockSkew = TimeSpan.Zero,
    };

    // IMPORTANT: Configure JWT for SignalR

    x.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;

            if (!string.IsNullOrEmpty(accessToken) &&
                path.StartsWithSegments("/notifications"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };


}).AddCookie("External")
.AddGoogle("Google",options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    options.SignInScheme = "External"; // REQUIRED
    options.Scope.Add("profile");  // ensures profile claims including picture
    options.Scope.Add("email");

    options.ClaimActions.MapJsonKey("picture", "picture");
}).AddMicrosoftAccount("Microsoft", options =>
{
    options.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"];

    options.Scope.Add("email");
    options.Scope.Add("profile");
}).AddGitHub("GitHub",options =>
{
    options.ClientId = builder.Configuration["Authentication:GitHub:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:GitHub:ClientSecret"];
    options.SignInScheme = "External"; // REQUIRED

    options.Scope.Add("user:email");
    
    options.ClaimActions.MapJsonKey("picture", "avatar_url");
});



// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()); // Required because you use withCredentials: true
});


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors("AllowAngular");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/notifications");

app.Run();
