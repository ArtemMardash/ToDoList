using AuthService.Features.Authentication.Login.Controllers;
using AuthService.Infrastructure.Extensions;
using AuthService.Features.Authentication.Register.Controllers;

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
 
await app.RegisterUser();
await app.LoginUser();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.Run();