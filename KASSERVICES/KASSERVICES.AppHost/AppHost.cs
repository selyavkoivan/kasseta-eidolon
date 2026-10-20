var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.CatalogAPI>("catalogapi");

builder.AddProject<Projects.BookingAPI>("bookingapi");

builder.Build().Run();
