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

    public void Confirm()
    {
        if (Status == BookingStatus.Pending) Status = BookingStatus.Active;
    }

    public void Reject()
    {
        if (Status == BookingStatus.Pending) Status = BookingStatus.Cancelled;
    }

    public void Return()
    {
        if (Status == BookingStatus.Active)
        {
            Status = BookingStatus.Returned;
            ReturnedAt = DateTimeOffset.UtcNow;
        }
    }
}