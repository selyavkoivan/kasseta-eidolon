namespace Shared.Contracts;

public record CassetteRegistered(
    Guid CassetteId,
    Guid MovieId,
    string InventoryNumber,
    DateTimeOffset OccurredAt);

public record CassetteBookingRequested(
    Guid BookingId,    Guid CassetteId,
    DateTimeOffset RequestedAt);

public record CassetteBooked(
    Guid BookingId,
    Guid CassetteId,
    DateTimeOffset BookedAt);

public record CassetteReturned(
    Guid CassetteId,
    Guid MovieId,
    DateTimeOffset ReturnedAt);

public record CassetteBookingConfirmed(
    Guid BookingId,
    Guid CassetteId,
    DateTimeOffset ConfirmedAt);

public record CassetteBookingRejected(
    Guid BookingId,
    Guid CassetteId,
    string Reason);