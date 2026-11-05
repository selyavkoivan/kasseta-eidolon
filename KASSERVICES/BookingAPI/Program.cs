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
        Title = "Catalog Service API",
        Version = "v1"
    });
});

// mediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

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
        cfg.Host("host.docker.internal", "/", h =>
        {
            h.Username("appuser");
            h.Password("apppassword");
        });
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
