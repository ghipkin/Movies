using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.ExternalFacing;

namespace Movies.Controllers
{
    [Route("api/movie")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="moveService"></param>
        public MovieController(IMovieService moveService)
        { 
            _movieService = moveService;
        }

        /// <summary>
        /// Controller method to search for Movies by the name or title of the film
        /// </summary>
        /// <param name="title"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet, Produces("application/json")]
        [ProducesResponseType(typeof(MovieResult),200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("GetMoviesByTitle")]
        public IActionResult GetMoviesByTitle(string title, int limit, int page)
        {
            var result = new MovieResult();
            try
            {
                var movies = _movieService.GetMoviesByTitle(title, limit, page);

                if(movies == null || movies.Any() == false)
                {
                    return NotFound();
                }

                result.Movies = movies;

            }
            catch(Exception ex) 
            { 
                result.ErrorMessage = ex.Message;

                return StatusCode(StatusCodes.Status500InternalServerError,result);
            }

            return Ok(result);

        }


        /// <summary>
        /// Controller Method to get a list of films by Genre
        /// </summary>
        /// <param name="genre"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet, Produces("application/json")]
        [ProducesResponseType(typeof(MovieResult), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("GetMoviesByGenre")]
        public IActionResult GetMoviesByGenre(string genre, int limit, int page)
        {
            var result = new MovieResult();
            try
            {
                var movies = _movieService.GetMoviesByGenre(genre, limit, page);

                if (movies == null || movies.Any() == false)
                {
                    return NotFound();
                }
                result.Movies = movies;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            return Ok(result);
        }


    }
}
