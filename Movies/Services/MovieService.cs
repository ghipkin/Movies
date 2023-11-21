using Movies.ExternalFacing;
using Movies.Datalayer.Models;
using Movies.Datalayer;
using Movies.Mapping;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Movies.Services
{
    /// <summary>
    /// CLass to allow processing of requestes via the MovieController class
    /// </summary>
    public class MovieService : IMovieService
    {
        internal const string ERR_GETMOVIES_BY_TITLE = "could not retrieve movies by Title.";
        internal const string ERR_GETMOVIES_BY_GENRE = "could not retrieve movies by Genre.";
        private MoviesContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="MoviesContext"></param>
        public MovieService(MoviesContext MoviesContext) 
        {
            _context = MoviesContext;
        }

        /// <summary>
        /// Manage the request for Movies by Title, including liiting the number of records returned and paging of results
        /// </summary>
        /// <param name="title"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<ExternalFacing.Movie> GetMoviesByTitle(string title, int limit = 0, int page = 0)
        {
            try 
            { 
                List<Datalayer.Models.Movie> retrievedMovies = _context.Movie.Where(x=>x.Title == title).Include(x=>x.Genre).ToList();

                List<Datalayer.Models.Movie> pagedMovies = ManageLimitAndPaging(retrievedMovies, limit, page);

                return MapMovieCollection(pagedMovies);
            }
            catch(Exception ex) 
            { 
                throw new Exception(ERR_GETMOVIES_BY_TITLE, ex);
            }
        }

        /// <summary>
        /// Manage the request for Movies by Genre, including liiting the number of records returned and paging of results
        /// </summary>
        /// <param name="genre"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<ExternalFacing.Movie> GetMoviesByGenre(string genre, int limit = 0, int page = 0)
        {
            try
            {
                List<Datalayer.Models.Movie> retrievedMovies = _context.Movie
                    .Where(x => x.Genre
                        .Where(y=>y.GenreDescription == genre).Any()).Include(x=>x.Genre).ToList();

                List<Datalayer.Models.Movie> pagedMovies = ManageLimitAndPaging(retrievedMovies, limit, page);

                return MapMovieCollection(pagedMovies);

            }
            catch (Exception ex)
            {
                throw new Exception(ERR_GETMOVIES_BY_GENRE, ex);
            }
        }

        /// <summary>
        /// Private methods to manage the limiting and paging of a list of moviews
        /// </summary>
        /// <param name="retrievedMovies"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        private List<Datalayer.Models.Movie> ManageLimitAndPaging(List<Datalayer.Models.Movie> retrievedMovies, int limit = 0, int page = 0) 
        {
            var moviesResult = new List<Datalayer.Models.Movie>();
            if (limit > 0)
            {
                if (page > 0)
                {
                    moviesResult = retrievedMovies.Skip(limit * (page - 1)).Take(limit).ToList<Datalayer.Models.Movie>();
                }
                else
                {
                    moviesResult = retrievedMovies.Take(limit).ToList<Datalayer.Models.Movie>();
                }
            }
            else
            {
                moviesResult = retrievedMovies;
            }

            return moviesResult;
        }

        /// <summary>
        /// Private mehgtod to map a collexction of Dto Movie objects to a Movie object suitable to return to a client
        /// </summary>
        /// <param name="movieDtos"></param>
        /// <returns></returns>
        private List<ExternalFacing.Movie> MapMovieCollection(List<Datalayer.Models.Movie> movieDtos)
        {
            var moviesToReturn = new List<ExternalFacing.Movie>();

            if (movieDtos != null && movieDtos.Count > 0)
            {
                foreach (var movieDto in movieDtos)
                {
                    moviesToReturn.Add(MapMovie.MapMovieFromDTO(movieDto));
                }
            }

            return moviesToReturn;
        }
    }
}
