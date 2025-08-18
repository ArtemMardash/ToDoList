
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using SyncService;
using SyncService.BackgroundJobs;
using SyncService.Features.Shared.Interfaces;
using SyncService.Features.Shared.Repositories;
using SyncService.Infrastructure.Extensions;
using SyncService.Infrastructure.Persistence;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddPersistence(builder.Configuration);
builder.Services
    .AddQueue(builder.Configuration)
    .RegisterBackgroundJob();
builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    })
    .AddCookie(opt =>
    {
        opt.Cookie.SameSite = SameSiteMode.Lax;
        opt.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    })
    .AddGoogle(opt =>
    {
        opt.ClientId = builder.Configuration.GetSection("GoogleOAuth")["ClientId"];
        opt.ClientSecret = builder.Configuration.GetSection("GoogleOAuth")["ClientSecret"];
        opt.CallbackPath = "/signin-google";
        opt.SaveTokens = true;
    });

builder.Services.AddHostedService<TelegramService>();
builder.Services.AddHttpClient("telegram_bot_client")
    .AddTypedClient<ITelegramBotClient>((client, sp) =>
    {
        var config = sp.GetRequiredService<IConfiguration>();
        return new TelegramBotClient(config["Telegram:Token"], client);
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;

    var context = service.GetService<SyncDbContext>();
    context?.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();

