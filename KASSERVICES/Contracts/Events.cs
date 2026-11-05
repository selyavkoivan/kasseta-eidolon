namespace Shared.Contracts;

// booking

public record CassetteBookingRequested(
    Guid BookingId,    
    Guid CassetteId,
    DateTimeOffset RequestedAt);

public record CassetteBookingConfirmed(
    Guid BookingId,
    Guid CassetteId,
    DateTimeOffset ConfirmedAt);

public record CassetteBookingRejected(
    Guid BookingId,
    Guid CassetteId,
    string Reason);

// returning
public record CassetteReturnRequested(
    Guid BookingId, 
    Guid CassetteId,
    DateTimeOffset RequestedAt);


public record CassetteReturnConfirmed(
    Guid BookingId,
    Guid CassetteId,
    DateTimeOffset ReturnedAt);

public record CassetteReturnRejected(
    Guid BookingId,
    Guid CassetteId,
    string Reason);

// extend
public record BookingExtendRequested(
    Guid BookingId,
    Guid CassetteId,
    DateTimeOffset RequestedAt);


public record BookingExtendConfirmed(
    Guid BookingId,
    Guid CassetteId,
    DateTimeOffset ReturnedAt);

public record BookingExtendRejected(
    Guid BookingId,
    Guid CassetteId,
    string Reason);