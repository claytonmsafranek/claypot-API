using API.Extensions;
using API.Middleware;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// register controllers
builder.Services.AddControllers();

// add all of our own services
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>(); // add in exception middleware

app.UseStatusCodePagesWithReExecute("/errors/{0}"); // add in middleware

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// tell the api to serve static content too
app.UseStaticFiles();

app.UseHttpsRedirection();

// Map custom controllers
app.MapControllers();

// apply db migrations at startup
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<StoreContext>();
var logger = services.GetRequiredService<ILogger<Program>>();
// try to migrate our db
try
{
    // this will also create the db it it does not already exist
    await context.Database.MigrateAsync();
    // seed data
    await StoreContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occured during databse migration");
}

app.Run();
