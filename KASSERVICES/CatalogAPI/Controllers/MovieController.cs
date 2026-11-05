using CatalogAPI.Features.Movies.ArchiveMovie;
using CatalogAPI.Features.Movies.CreateMovie;
using CatalogAPI.Features.Movies.DeleteMovie;
using CatalogAPI.Features.Movies.GetMovieById;
using CatalogAPI.Features.Movies.GetMovies;
using CatalogAPI.Features.Movies.RestoreMovie;
using CatalogAPI.Features.Movies.UpdateMovie;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Movies")]
    public class MovieController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateMovieCommand command, CancellationToken cancellationToken)
        {
            var id = await mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<MovieDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var movies = await mediator.Send(new GetMoviesQuery(), cancellationToken);
            return Ok(movies);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(MovieDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var movie = await mediator.Send(new GetMovieByIdQuery(id), cancellationToken);
            return movie is null ? NotFound() : Ok(movie);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMovieCommand command, CancellationToken cancellationToken)
        {
            await mediator.Send(command with { Id = id }, cancellationToken);
            return NoContent();
        }

        [HttpPatch("{id:guid}/archive")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Archive(Guid id, CancellationToken cancellationToken)
        {
            await mediator.Send(new ArchiveMovieCommand(id), cancellationToken);
            return NoContent();
        }

        [HttpPatch("{id:guid}/restore")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Restore(Guid id, CancellationToken cancellationToken)
        {
            await mediator.Send(new RestoreMovieCommand(id), cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteMovieCommand(id), cancellationToken);
            return NoContent();
        }
    }
}
