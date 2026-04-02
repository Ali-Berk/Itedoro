using Itedoro.Api;
using System.Text;
using Itedoro.Data;
using Itedoro.Business;
using Itedoro.Data.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Itedoro.Data.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Itedoro.Business.Services.AuthServices.LoginService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApiServices();

builder.Services.AddDbContext<ItedoroDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))); 
builder.Services.AddRepositories();

builder.Services.AddBusinessServices();

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
        ValidIssuer = builder.Configuration["AppSettings:Issuer"],
        ValidAudience = builder.Configuration["AppSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration["AppSettings:Token"] ?? throw new InvalidOperationException("Token key not found in settings.")
        ))
    };
});

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

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