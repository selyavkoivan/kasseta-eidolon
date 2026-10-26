using BookingAPI.Features.Bookings.CreateBooking;
using BookingAPI.Features.Bookings.GetBookingById;
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
    public async Task<IActionResult> Create([FromBody] CreateBookingCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return AcceptedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BookingDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var booking = await mediator.Send(new GetBookingByIdQuery(id), ct);
        return booking is null ? NotFound() : Ok(booking);
    }
}