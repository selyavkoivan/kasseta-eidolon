using BookingAPI.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingAPI.Features.Bookings.GetBookingByMovieId;

public class GetBookingByMovieIdHandler(BookingDbContext db) : IRequestHandler<GetBookingByMovieIdQuery, List<BookingDetailsDto>>
{
    public async Task<List<BookingDetailsDto>> Handle(GetBookingByMovieIdQuery request, CancellationToken cancellationToken)
    {
        return await db.Bookings
            .AsNoTracking()
            .Where(b => b.MovieId == request.Id)
            .ToDetailsDto()
            .ToListAsync(cancellationToken);
    }
}