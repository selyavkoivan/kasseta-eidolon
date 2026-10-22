using CatalogAPI.Features.Movies.CreateMovie;
using CatalogAPI.Features.Movies.GetMovieById;
using CatalogAPI.Features.Movies.GetMovies;
using MediatR;
using Microsoft.AspNetCore.Http;
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
    }
}
