using AuthService.Features.Authentication.Login.Controllers;
using AuthService.Features.Authentication.RefreshToken.Controllers;
using AuthService.Infrastructure.Extensions;
using AuthService.Features.Authentication.Register.Controllers;
using AuthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddInfrastructure();
builder.Services.AddJwt(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetService<AuthDbContext>();
    context?.Database.Migrate();
}

app.RegisterUser();
app.LoginUser();
app.RefreshToken();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();