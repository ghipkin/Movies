using Movies.ExternalFacing;

namespace Movies
{
    /// <summary>
    /// Interface to allow mocking of the MovieService class
    /// </summary>
    public interface IMovieService
    {
        List<Movie> GetMoviesByTitle(string title, int limit=0, int page=0);
        List<Movie> GetMoviesByGenre(string genre, int limit = 0, int page = 0);
    }
}
