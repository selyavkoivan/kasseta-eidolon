using BookingAPI.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingAPI.Features.Bookings.GetBookingByCassetteId;

public class GetBookingByCassetteIdHandler(BookingDbContext db) : IRequestHandler<GetBookingByCassetteIdQuery, List<BookingDetailsDto>>
{
    public async Task<List<BookingDetailsDto>> Handle(GetBookingByCassetteIdQuery request, CancellationToken cancellationTokent)
    {
        return await db.Bookings
            .AsNoTracking()
            .Where(b => b.CassetteId == request.Id)
            .ToDetailsDto()
            .ToListAsync(cancellationTokent);
    }
}