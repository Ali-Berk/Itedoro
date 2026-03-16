using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Itedoro.Data;
using Itedoro.Business;
using Itedoro.Data.Entities.Users;
using Itedoro.Business.Services.LoginService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ItedoroDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))); 

builder.Services.AddBusinessServices();



//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// FluentValidation
builder.Services.AddBusinessValidators();
// AutoMapper
builder.Services.AddAutoMapper(cfg =>cfg.AddMaps(typeof(LoginManager).Assembly));

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

var app = builder.Build();

//Docker
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

//Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); 

app.UseAuthorization();
app.MapControllers();

app.Run();