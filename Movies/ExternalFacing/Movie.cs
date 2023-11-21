namespace Movies.ExternalFacing
{
    public class Movie
    {        /// <summary>
             /// Release date for the Movie
             /// </summary>
        public DateOnly ReleaseDate { get; set; }

        /// <summary>
        /// The Title of the Movie
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Brief summary of the movie.
        /// </summary>
        public string Overview { get; set; }

        /// <summary>
        /// a very important metric computed by TMDB developers based on the number of views per day, votes per day, number of users marked it as "favorite" and "watchlist" for the data, release date and more other metrics.
        /// </summary>
        public double Popularity { get; set; }

        /// <summary>
        /// Total votes received from the viewers.
        /// </summary>
        public int VoteCount { get; set; }

        /// <summary>
        /// Average rating based on vote count and the number of viewers out of 10.
        /// </summary>
        public double VoteAverage { get; set; }

        /// <summary>
        /// Original language of the movies. Dubbed version is not considered to be original language.
        /// </summary>
        public string OriginalLanguage { get; set; }

        /// <summary>
        /// List of genres this film belongs to
        /// </summary>
        public List<string> Genres { get; set; }
    }
}
