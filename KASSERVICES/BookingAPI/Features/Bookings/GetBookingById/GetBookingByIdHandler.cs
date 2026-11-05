using BookingAPI.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingAPI.Features.Bookings.GetBookingById;

public class GetBookingByIdHandler(BookingDbContext db) : IRequestHandler<GetBookingByIdQuery, BookingDetailsDto?>
{
    public async Task<BookingDetailsDto?> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        return await db.Bookings
            .AsNoTracking()
            .Where(b => b.Id == request.Id)
            .ToDetailsDto()
            .FirstOrDefaultAsync(cancellationToken);
    }
}