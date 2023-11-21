using Microsoft.Extensions.Diagnostics.HealthChecks;
using Movies.Datalayer.Models;


namespace Movies.Mapping
{
    /// <summary>
    /// CLass to provide mapping functionality for Movie objects
    /// </summary>
    public static class MapMovie
    {
        /// <summary>
        /// Map Movie Dto to an object suitabke for returning to a client
        /// </summary>
        /// <param name="movieDto"></param>
        /// <returns></returns>
        public static ExternalFacing.Movie MapMovieFromDTO(Datalayer.Models.Movie movieDto) 
        {
            var result = new ExternalFacing.Movie {
                OriginalLanguage = movieDto.OriginalLanguage,
                Overview = movieDto.Overview,
                Popularity = movieDto.Popularity,
                ReleaseDate = movieDto.ReleaseDate,
                Title = movieDto.Title,
                VoteAverage = movieDto.VoteAverage,
                VoteCount = movieDto.VoteCount,
                Genres = new List<string>()
            };
            
            foreach (Genre genre in movieDto.Genre)
            {
                result.Genres.Add(genre.GenreDescription);
            }

            return result;
        }

        /// <summary>
        /// Map a Movie object to a movie Dto object
        /// </summary>
        /// <param name="movieExternal"></param>
        /// <returns></returns>
        public static Datalayer.Models.Movie MapMovieToDTO(ExternalFacing.Movie movieExternal)
        {
            var result = new Datalayer.Models.Movie
            {
                OriginalLanguage = movieExternal.OriginalLanguage,
                Overview = movieExternal.Overview,
                Popularity = movieExternal.Popularity,
                ReleaseDate = movieExternal.ReleaseDate,
                Title = movieExternal.Title,
                VoteAverage = movieExternal.VoteAverage,
                VoteCount = (short)movieExternal.VoteCount
            };

            return result;
        }
    }
}
