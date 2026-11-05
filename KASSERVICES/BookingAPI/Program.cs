using BookingAPI.Consumers.Booking;
using BookingAPI.Consumers.Extend;
using BookingAPI.Consumers.ReturnBooking;
using BookingAPI.Data;
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
builder.Services.AddDbContext<BookingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Booking Service API",
        Version = "v1"
    });
});

// mediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// authorization
builder.Services.AddAuthorization();

// RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<BookingConfirmedConsumer>();
    x.AddConsumer<BookingRejectedConsumer>();

    x.AddConsumer<ReturnBookingConfirmedConsumer>();
    x.AddConsumer<ReturnBookingRejectedConsumer>();
    
    x.AddConsumer<BookingExtendConfirmedConsumer>();
    x.AddConsumer<BookingExtendRejectedConsumer>();

    x.AddEntityFrameworkOutbox<BookingDbContext>(o =>
    {
        o.UseSqlServer();
        o.UseBusOutbox();
    });

    x.UsingRabbitMq((context, cfg) =>
    {
        var host = builder.Configuration["RabbitMq:Host"] ?? "host.docker.internal";
        var username = builder.Configuration["RabbitMq:Username"]!;
        var password = builder.Configuration["RabbitMq:Password"]!;

        cfg.Host(host, "/", h =>
        {
            h.Username(username);
            h.Password(password);
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Booking API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
