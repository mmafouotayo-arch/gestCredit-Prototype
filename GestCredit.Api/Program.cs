using System.Text;
using System.Threading.RateLimiting;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using GestCredit.Api.Data;
using GestCredit.Api.Middleware;
using GestCredit.Api.Models;
using GestCredit.Api.Services;

var builder = WebApplication.CreateBuilder(args);

string jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException(
        "Jwt:Key est introuvable. Configurez-la avec : dotnet user-secrets set \"Jwt:Key\" \"<votre clé>\"");

if (jwtKey.Length < 32)
{
    throw new InvalidOperationException("Jwt:Key doit contenir au moins 32 caractères.");
}

builder.Services.AddDbContext<GestCreditDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GestCreditDb")));

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IDemandeCreditService, DemandeCreditService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// Limite les tentatives de connexion : 5 par minute et par adresse IP
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("login", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "inconnu",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            }));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Collez uniquement le token JWT (sans le mot 'Bearer')."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

var app = builder.Build();

// Création du premier administrateur : le mot de passe vient des secrets, jamais du code
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<GestCreditDbContext>();
    string? motDePasseAdmin = app.Configuration["Admin:Password"];

    bool adminExiste = await context.Utilisateurs
        .AnyAsync(u => u.Roles.Any(r => r.Nom == "Administrateur"));

    if (!adminExiste && !string.IsNullOrWhiteSpace(motDePasseAdmin))
    {
        Role roleAdmin = await context.Roles.FirstAsync(r => r.Nom == "Administrateur");

        var admin = new Utilisateur(
            "admin",
            BCrypt.Net.BCrypt.HashPassword(motDePasseAdmin),
            "admin@gestcredit.local");

        admin.Roles.Add(roleAdmin);
        context.Utilisateurs.Add(admin);
        await context.SaveChangesAsync();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestTimingMiddleware>();

app.UseHttpsRedirection();
app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();