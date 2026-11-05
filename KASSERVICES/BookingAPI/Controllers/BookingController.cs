using BookingAPI.Domain.Entities;
using BookingAPI.Features.Bookings;
using BookingAPI.Features.Bookings.CreateBooking;
using BookingAPI.Features.Bookings.ExtendBooking;
using BookingAPI.Features.Bookings.GetBookingById;
using BookingAPI.Features.Bookings.ReturnBooking;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookingAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Tags("Bookings")]
public class BookingController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateBookingCommand command, CancellationToken cancellationToken)
    {
        var id = await mediator.Send(command, cancellationToken);
        return AcceptedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BookingDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var booking = await mediator.Send(new GetBookingByIdQuery(id), cancellationToken);
        return booking is null ? NotFound() : Ok(booking);
    }

    [HttpGet("cassette/{id:guid}")]
    [ProducesResponseType(typeof(BookingDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByCassetteId(Guid id, CancellationToken cancellationToken)
    {
        var booking = await mediator.Send(new GetBookingByCassetteIdQuery(id), cancellationToken);
        return booking is null ? NotFound() : Ok(booking);
    }

    [HttpGet("movie/{id:guid}")]
    [ProducesResponseType(typeof(BookingDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByMovieId(Guid id, CancellationToken cancellationToken)
    {
        var booking = await mediator.Send(new GetBookingByMovieIdQuery(id), cancellationToken);
        return booking is null ? NotFound() : Ok(booking);
    }

    [HttpPatch("{id:guid}/return")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Return(Guid id, CancellationToken ct)
    {
        var booking = await mediator.Send(new ReturnBookingCommand(id), ct);
        return booking is null ? NotFound() : Ok(booking);
    }

    [HttpPatch("{id:guid}/extend")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Extend(ExtendBookingCommand command, CancellationToken ct) 
    {
        var booking = await mediator.Send(command, ct);
        return booking is null ? NotFound() : Ok(booking);
    }
}