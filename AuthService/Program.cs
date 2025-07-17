using AuthService.Features.Authentication.ExternalAuth.Google.Controllers;
using AuthService.Features.Authentication.Login.Controllers;
using AuthService.Features.Authentication.RefreshToken.Controllers;
using AuthService.Infrastructure.Extensions;
using AuthService.Features.Authentication.Register.Controllers;
using AuthService.Features.Authentication.Shared.Settings;
using AuthService.Infrastructure.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<ExternalAuthSettings>(builder.Configuration.GetSection(nameof(ExternalAuthSettings)));

//Чтобы авторизация работала в свагере
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        },
    });
});

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddMediator(options=> options.ServiceLifetime = ServiceLifetime.Scoped);
builder.Services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
builder.Services.AddRabbitMq(builder.Configuration);
builder.Services.AddJwt(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetService<AuthDbContext>();
    context?.Database.Migrate();
}

app.UseHsts();
app.UseHttpsRedirection();


app.RegisterUser();
app.LoginUser();
app.RefreshToken();
app.GoogleLoginUser();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();