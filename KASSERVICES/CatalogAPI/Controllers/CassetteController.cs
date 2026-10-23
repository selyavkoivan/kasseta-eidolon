using CatalogAPI.Features.Cassettes.ChangeCassetteStatus;
using CatalogAPI.Features.Cassettes.CreateCassette;
using CatalogAPI.Features.Cassettes.DeleteCassette;
using CatalogAPI.Features.Cassettes.GetCassettesByMovie;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Cassettes")]
    public class CassetteController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] CreateCassetteCommand command, CancellationToken cancellationToken)
        {
            var id = await mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetByMovie), new { movieId = command.MovieId }, id);
        }

        [HttpGet("by-movie/{movieId:guid}")]
        [ProducesResponseType(typeof(List<CassetteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByMovie(Guid movieId, CancellationToken cancellationToken)
        {
            var cassettes = await mediator.Send(new GetCassettesByMovieQuery(movieId), cancellationToken);
            return Ok(cassettes);
        }

        /// <summary>
        /// Изменить физический статус кассеты (Например: Сломалась, Потеряна)
        /// </summary>
        [HttpPatch("{id:guid}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ChangeCassetteStatusCommand command, CancellationToken cancellationToken)
        {
            // Принудительно подставляем ID из URL в команду
            await mediator.Send(command with { CassetteId = id }, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Списать кассету (полное удаление из оборота)
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteCassetteCommand(id), cancellationToken);
            return NoContent();
        }
    }
}