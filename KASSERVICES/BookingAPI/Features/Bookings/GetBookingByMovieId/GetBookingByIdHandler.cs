using BookingAPI.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingAPI.Features.Bookings.GetBookingById;

public class GetBookingByMovieIdHandler(BookingDbContext db) : IRequestHandler<GetBookingByMovieIdQuery, List<BookingDetailsDto>>
{
    public async Task<List<BookingDetailsDto>> Handle(GetBookingByMovieIdQuery request, CancellationToken cancellationToken)
    {
        return await db.Bookings
            .AsNoTracking()
            .Where(b => b.MovieId == request.Id)
            .Select(b => new BookingDetailsDto(
                b.Id,
                b.CassetteId,
                b.MovieId,
                b.BookedAt,
                b.DueDate,
                b.Status.ToString()
            ))
            .ToListAsync(cancellationToken);
    }
}