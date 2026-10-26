using BookingAPI.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BookingAPI.Domain.Entities;

public class Booking
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid CassetteId { get; set; }
    public Guid MovieId { get; set; }

    public DateTimeOffset BookedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset DueDate { get; set; }
    public DateTimeOffset? ReturnedAt { get; set; }

    public BookingStatus Status { get; set; } = BookingStatus.Pending;

    public void Return()
    {
        if (Status != BookingStatus.Active && Status != BookingStatus.Overdue)
            throw new InvalidOperationException("Only active or overdue bookings can be returned.");

        Status = BookingStatus.Returned;
        ReturnedAt = DateTimeOffset.UtcNow;
    }

    public void Cancel()
    {
        if (Status != BookingStatus.Active)
            throw new InvalidOperationException("Only active bookings can be cancelled.");

        Status = BookingStatus.Cancelled;
    }
}