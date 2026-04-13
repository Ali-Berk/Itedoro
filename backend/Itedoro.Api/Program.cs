using Itedoro.Persistence;
using Itedoro.Application;
using Itedoro.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Itedoro.Api.DependencyInjection;
using Itedoro.Persistence.DependencyInjection;
using Itedoro.Application.Services.AuthServices.LoginService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices(builder.Configuration);

builder.Services.AddDbContext<ItedoroDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))); 
builder.Services.AddRepositories();

builder.Services.AddApplicationServices();
builder.Services.AddApplicationValidators();
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(LoginManager).Assembly));
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

var app = builder.Build();

// Docker & Migration
using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ItedoroDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Database migration failed.");
    }
}

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Itedoro.Api v1");
        options.RoutePrefix = string.Empty;
        options.EnablePersistAuthorization();
        options.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model);
    });
}
else
{
    app.UseExceptionHandler();
}

// app.UseHttpsRedirection(); 
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
