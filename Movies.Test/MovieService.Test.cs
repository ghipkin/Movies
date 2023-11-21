using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using Movies;
using Movies.Datalayer;
using Movies.Services;
using Xunit.Sdk;
using Movies.Datalayer.Models;

namespace Movies.Test
{
    
    public class MovieServiceTest
    {
        const string TITLE = "movie title";
        const string GENRE = "movie genre";

        const string MOVIE1_TITLE = "movie1";
        const string MOVIE2_TITLE = "movie2";
        const string MOVIE3_TITLE = "movie3";
        const string MOVIE_TITLE_DUPLICATED = "duplicate title";

        const string GENRE1 = "genre1";
        const string GENRE2 = "genre2";
        const string GENRE_DUPLICATED = "Duplicate genre";

        const int VOTE1 = 1;
        const int VOTE2 = 2;
        const int VOTE3 = 3;
        const int VOTE4 = 4;

        private readonly MovieService _movieService;
        private readonly Mock<MoviesContext> _mockContext;

        public MovieServiceTest()
        {
            _mockContext = new Mock<MoviesContext>();
            _movieService = new MovieService(_mockContext.Object);
        }

        [Fact]
        public void GetMoviesByTitle_Exception()
        {
            const string ERR_MESSAGE = "Service error getting movies by title";
            //ARRANGE
            _mockContext.Setup(x => x.Movie).Throws(new Exception(ERR_MESSAGE));

            //ACT
            Exception? trappedException = null;
            try
            {
                var result = _movieService.GetMoviesByTitle(TITLE);
            }
            catch (Exception ex) 
            { 
                trappedException = ex; 
            }

            //ASSERT
            Assert.NotNull(trappedException);
            Exception mainEx = (Exception)trappedException;
            Assert.Equal(MovieService.ERR_GETMOVIES_BY_TITLE, mainEx.Message);
            Assert.NotNull(mainEx.InnerException);
            Exception innerEx = (Exception)mainEx.InnerException;
            Assert.Equal(ERR_MESSAGE, innerEx.Message);
        }

        [Fact]
        public void GetMoviesByTitle_ReturnNothing()
        {
            //ARRANGE
            var emptyModelList = new List<Datalayer.Models.Movie>();
            _mockContext.Setup(x => x.Movie).ReturnsDbSet(emptyModelList);

            //ACT
            var result = _movieService.GetMoviesByTitle(TITLE);

            //ASSERT
            Assert.NotNull(result);
            Assert.Empty(result);
        }
        
        [Fact]
        public void GetMovies_ByTitle_HappyPath()
        {
            //ARRANGE
            var movieData = GetMovieList();
            _mockContext.Setup(x => x.Movie).ReturnsDbSet(movieData);

            //ACT
            var result = _movieService.GetMoviesByTitle(MOVIE1_TITLE);

            //ASSERT
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            
            var parsedResult = (List<ExternalFacing.Movie>)result;
            Assert.Single(parsedResult);
            Assert.Equal(MOVIE1_TITLE, parsedResult.First().Title);

        }

        [Fact]
        public void GetMovies_ByDuplicateTitle_HappyPath()
        {
            //ARRANGE
            var movieData = GetMovieList();
            _mockContext.Setup(x => x.Movie).ReturnsDbSet(movieData);

            //ACT
            var result = _movieService.GetMoviesByTitle(MOVIE_TITLE_DUPLICATED);

            //ASSERT
            Assert.NotNull(result);
            Assert.NotEmpty(result);

            var parsedResult = (List<ExternalFacing.Movie>)result;
            Assert.Equal(4, parsedResult.Count());
        }

        [Fact]
        public void GetMovies_ByDuplicateTitle_WithLimit_HappyPath()
        {
            const int LIMIT = 2;

            //ARRANGE
            var movieData = GetMovieList();
            _mockContext.Setup(x => x.Movie).ReturnsDbSet(movieData);

            //ACT
            var result = _movieService.GetMoviesByTitle(MOVIE_TITLE_DUPLICATED, LIMIT, 0);

            //ASSERT
            Assert.NotNull(result);
            Assert.NotEmpty(result);

            var parsedResult = (List<ExternalFacing.Movie>)result;
            Assert.Equal(LIMIT, parsedResult.Count());
        }

        [Fact]
        public void GetMovies_ByDuplicateTitle_WithLimit_Page1_HappyPath()
        {
            const int LIMIT = 2;

            //ARRANGE
            var movieData = GetMovieList();
            _mockContext.Setup(x => x.Movie).ReturnsDbSet(movieData);

            //ACT
            var result = _movieService.GetMoviesByTitle(MOVIE_TITLE_DUPLICATED, LIMIT, 1);

            //ASSERT
            Assert.NotNull(result);
            Assert.NotEmpty(result);

            var parsedResult = (List<ExternalFacing.Movie>)result;
            Assert.Equal(LIMIT, parsedResult.Count());
            Assert.True(parsedResult.Where(x => x.VoteCount == VOTE1).Any());
            Assert.True(parsedResult.Where(x => x.VoteCount == VOTE2).Any());
            Assert.False(parsedResult.Where(x => x.VoteCount == VOTE3).Any());
            Assert.False(parsedResult.Where(x => x.VoteCount == VOTE4).Any());
        }

        [Fact]
        public void GetMovies_ByDuplicateTitle_WithLimit_Page2_HappyPath()
        {
            const int LIMIT = 2;

            //ARRANGE
            var movieData = GetMovieList();
            _mockContext.Setup(x => x.Movie).ReturnsDbSet(movieData);

            //ACT
            var result = _movieService.GetMoviesByTitle(MOVIE_TITLE_DUPLICATED, LIMIT, 2);

            //ASSERT
            Assert.NotNull(result);
            Assert.NotEmpty(result);

            var parsedResult = (List<ExternalFacing.Movie>)result;
            Assert.Equal(LIMIT, parsedResult.Count());
            Assert.False(parsedResult.Where(x => x.VoteCount == VOTE1).Any());
            Assert.False(parsedResult.Where(x => x.VoteCount == VOTE2).Any());
            Assert.True(parsedResult.Where(x => x.VoteCount == VOTE3).Any());
            Assert.True(parsedResult.Where(x => x.VoteCount == VOTE4).Any());
        }

        [Fact]
        public void GetMovies_ByDuplicateTitle_WithLimit_Page3_HappyPath()
        {
            const int LIMIT = 2;

            //ARRANGE
            var movieData = GetMovieList();
            _mockContext.Setup(x => x.Movie).ReturnsDbSet(movieData);

            //ACT
            var result = _movieService.GetMoviesByTitle(MOVIE_TITLE_DUPLICATED, LIMIT, 3);

            //ASSERT
            Assert.NotNull(result);

            var parsedResult = (List<ExternalFacing.Movie>)result;
            Assert.Empty(parsedResult);
        }

        [Fact]
        public void GetMovies_ByGenre_HappyPath()
        {
            //ARRANGE
            var movieData = GetMovieList();

            _mockContext.Setup(x => x.Movie).ReturnsDbSet(movieData);

            //ACT
            var result = _movieService.GetMoviesByGenre(GENRE1);

            //ASSERT
            Assert.NotNull(result);
            Assert.NotEmpty(result);

            var parsedResult = (List<ExternalFacing.Movie>)result;
            Assert.Equal(2, parsedResult.Count());
            Assert.Equal(MOVIE1_TITLE, parsedResult.First().Title);
            Assert.Equal(MOVIE2_TITLE, parsedResult.Last().Title);
            Assert.True(parsedResult.Where(x=>x.Title == MOVIE3_TITLE).Count() == 0);
        }

        [Fact]
        public void GetMovies_ByDuplicatedGenre_WithLimit_HappyPath()
        {
            const int LIMIT = 2;

            //ARRANGE
            var movieData = GetMovieList();
            _mockContext.Setup(x => x.Movie).ReturnsDbSet(movieData);

            //ACT
            var result = _movieService.GetMoviesByGenre(GENRE_DUPLICATED, LIMIT);

            //ASSERT
            Assert.NotNull(result);
            Assert.NotEmpty(result);

            var parsedResult = (List<ExternalFacing.Movie>)result;
            Assert.Equal(LIMIT, parsedResult.Count());
            Assert.True(parsedResult.Where(x => x.Genres.Contains(GENRE_DUPLICATED)).Count() == LIMIT);
        }


        [Fact]
        public void GetMovies_ByDuplicateGenre_WithLimit_Page1_HappyPath()
        {
            const int LIMIT = 2;

            //ARRANGE
            var movieData = GetMovieList();
            _mockContext.Setup(x => x.Movie).ReturnsDbSet(movieData);

            //ACT
            var result = _movieService.GetMoviesByGenre(GENRE_DUPLICATED, LIMIT, 1);

            //ASSERT
            Assert.NotNull(result);
            Assert.NotEmpty(result);

            var parsedResult = (List<ExternalFacing.Movie>)result;
            Assert.Equal(LIMIT, parsedResult.Count());
            Assert.True(parsedResult.Where(x => x.VoteCount == VOTE1).Any());
            Assert.True(parsedResult.Where(x => x.VoteCount == VOTE2).Any());
            Assert.False(parsedResult.Where(x => x.VoteCount == VOTE3).Any());
            Assert.False(parsedResult.Where(x => x.VoteCount == VOTE4).Any());
        }

        [Fact]
        public void GetMovies_ByDuplicateGenre_WithLimit_Page2_HappyPath()
        {
            const int LIMIT = 2;

            //ARRANGE
            var movieData = GetMovieList();
            _mockContext.Setup(x => x.Movie).ReturnsDbSet(movieData);

            //ACT
            var result = _movieService.GetMoviesByGenre(GENRE_DUPLICATED, LIMIT, 2);

            //ASSERT
            Assert.NotNull(result);
            Assert.NotEmpty(result);

            var parsedResult = (List<ExternalFacing.Movie>)result;
            Assert.Equal(LIMIT, parsedResult.Count());
            Assert.False(parsedResult.Where(x => x.VoteCount == VOTE1).Any());
            Assert.False(parsedResult.Where(x => x.VoteCount == VOTE2).Any());
            Assert.True(parsedResult.Where(x => x.VoteCount == VOTE3).Any());
            Assert.True(parsedResult.Where(x => x.VoteCount == VOTE4).Any());
        }

        [Fact]
        public void GetMovies_ByDuplicateGenre_WithLimit_Page3_HappyPath()
        {
            const int LIMIT = 2;

            //ARRANGE
            var movieData = GetMovieList();
            _mockContext.Setup(x => x.Movie).ReturnsDbSet(movieData);

            //ACT
            var result = _movieService.GetMoviesByGenre(GENRE_DUPLICATED, LIMIT, 3);

            //ASSERT
            Assert.NotNull(result);

            var parsedResult = (List<ExternalFacing.Movie>)result;
            Assert.Empty(parsedResult);
        }

        [Fact]
        public void GetMoviesByGenre_Exception()
        {
            const string ERR_MESSAGE = "Service error getting movies by Genre";
            //ARRANGE
            _mockContext.Setup(x => x.Movie).Throws(new Exception(ERR_MESSAGE));

            //ACT
            Exception? trappedException = null;
            try
            {
                var result = _movieService.GetMoviesByGenre(GENRE);
            }
            catch (Exception ex)
            {
                trappedException = ex;
            }

            //ASSERT
            Assert.NotNull(trappedException);
            Exception mainEx = (Exception)trappedException;
            Assert.Equal(MovieService.ERR_GETMOVIES_BY_GENRE, mainEx.Message);
            Assert.NotNull(mainEx.InnerException);
            Exception innerEx = (Exception)mainEx.InnerException;
            Assert.Equal(ERR_MESSAGE, innerEx.Message);
        }

        [Fact]
        public void GetMoviesByGenre_ReturnNothing()
        {
            //ARRANGE
            var emptyMovieList = new List<Datalayer.Models.Movie>();
            _mockContext.Setup(x => x.Movie).ReturnsDbSet(emptyMovieList);

            //ACT
            var result = _movieService.GetMoviesByGenre(GENRE, 0, 0);

            //ASSERT
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        private List<Datalayer.Models.Movie> GetMovieList()
        {
            return new List<Datalayer.Models.Movie>
            {
                new Datalayer.Models.Movie { Title = MOVIE1_TITLE, Genre = new List<Genre> {new Genre{ GenreDescription = GENRE1} } },
                new Datalayer.Models.Movie { Title = MOVIE2_TITLE, Genre = new List<Genre>{ new Genre {GenreDescription = GENRE1},new Genre {GenreDescription = GENRE2} } },
                new Datalayer.Models.Movie { Title = MOVIE3_TITLE, Genre = new List<Genre>{ new Genre{ GenreDescription = GENRE2}  } },
                new Datalayer.Models.Movie { Title = MOVIE_TITLE_DUPLICATED, Genre = new List<Genre>{ new Genre{ GenreDescription = GENRE_DUPLICATED}  }, VoteCount = VOTE1 },
                new Datalayer.Models.Movie { Title = MOVIE_TITLE_DUPLICATED, Genre = new List<Genre>{ new Genre{ GenreDescription = GENRE_DUPLICATED }  }, VoteCount = VOTE2 },
                new Datalayer.Models.Movie { Title = MOVIE_TITLE_DUPLICATED, Genre = new List<Genre>{ new Genre{ GenreDescription = GENRE_DUPLICATED }  }, VoteCount = VOTE3 },
                new Datalayer.Models.Movie { Title = MOVIE_TITLE_DUPLICATED, Genre = new List<Genre>{ new Genre{ GenreDescription = GENRE_DUPLICATED }  }, VoteCount = VOTE4 },
            };
        }

    }
}
