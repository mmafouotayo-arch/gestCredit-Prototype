using Microsoft.EntityFrameworkCore;
using GestCredit.Api.Services;
using GestCredit.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<GestCredit.Api.Data.GestCreditDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GestCreditDb")));

builder.Services.AddScoped<IClientService, ClientService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestTimingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();