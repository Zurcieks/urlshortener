using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using UrlShortener.Api.Data;
using UrlShortener.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!));

builder.Services.AddSingleton<Base62Service>();
builder.Services.AddSingleton<UrlValidator>();
builder.Services.AddScoped<UrlShortenerService>();

builder.Services.AddRateLimiter(opt =>
{
    opt.AddFixedWindowLimiter("shorten", l =>
    {
        l.Window = TimeSpan.FromMinutes(1);
        l.PermitLimit = 10;
        l.QueueLimit = 0;
    });

    opt.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsJsonAsync(new { error = "Rate limit exceeded. Try again later." }, cancellationToken);
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();

app.UseRateLimiter();
app.MapControllers();


app.UseHttpsRedirection();



app.Run();


