using API.Data;
using API.Middleware;
using API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StoreContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCors();
builder.Services.AddIdentityCore<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<StoreContext>();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(opt =>
{
    opt.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:3000");
});

app.UseAuthorization();

app.MapGet("/api/buggy/not-found", () =>
{
    return Results.NotFound();
});
app.MapGet("/api/buggy/bad-request", () =>
{
    return Results.BadRequest(new ProblemDetails { Title = "this is a bad request." });
});
app.MapGet("/api/buggy/unauthorized", () =>
{
    return Results.Unauthorized();
});
app.MapGet("/api/buggy/validation-error", () =>
{
    var errors = new Dictionary<string, string[]>
    {
        {"Problem1", new[]{ "This is the first error"}},
        {"Problem2", new[]{ "This is the second error"}}
    };
    return Results.ValidationProblem(errors);
});
app.MapGet("/api/buggy/server-error", () =>
{
    throw new Exception("This is a server error");
});

app.MapControllers();

var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<StoreContext>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
try
{
    context.Database.Migrate();
    await DbInitializer.Initialize(context, userManager);
}
catch (System.Exception ex)
{
    logger.LogError(ex, "A problem occurred during migrations");
}
app.Run();
