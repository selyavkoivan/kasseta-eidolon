var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.CatalogAPI>("catalogapi");

builder.AddProject<Projects.BookingAPI>("bookingapi");

builder.AddProject<Projects.UserService>("userservice");

builder.Build().Run();
