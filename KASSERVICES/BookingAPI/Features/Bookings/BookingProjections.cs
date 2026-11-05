using BookingAPI.Domain.Entities;

namespace BookingAPI.Features.Bookings;

public record BookingDetailsDto(
    Guid Id,
    Guid CassetteId,
    Guid MovieId,
    DateTimeOffset BookedAt,
    DateTimeOffset DueDate,
    string Status);

public static class BookingProjections
{
    public static IQueryable<BookingDetailsDto> ToDetailsDto(this IQueryable<Booking> query)
        => query.Select(b => new BookingDetailsDto(
            b.Id,
            b.CassetteId,
            b.MovieId,
            b.BookedAt,
            b.DueDate,
            b.Status.ToString()));

    public static BookingDetailsDto ToDetailsDto(this Booking booking)
        => new(
            booking.Id,
            booking.CassetteId,
            booking.MovieId,
            booking.BookedAt,
            booking.DueDate,
            booking.Status.ToString());
}