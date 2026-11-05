using CatalogAPI.Common.Interfaces;
using CatalogAPI.Consumers;
using CatalogAPI.Data;
using CatalogAPI.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// ORM
builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Catalog Service API",
        Version = "v1"
    });
});

//mediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

//Genre resolver
builder.Services.AddScoped<IGenreResolver, GenreResolver>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CassetteBookingRequestedConsumer>();
    x.AddConsumer<CassetteReturnRequestedConsumer>();
    x.AddConsumer<BookingExtendRequestConsumer>();

    x.AddEntityFrameworkOutbox<CatalogDbContext>(o =>
    {
        o.UseSqlServer();
        o.UseBusOutbox();
    });

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("host.docker.internal", "/", h =>
        {
            h.Username("appuser");
            h.Password("apppassword");
        });
        cfg.ConfigureEndpoints(context);

    });
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API v1");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
