using Moq;
using System.Collections.Generic;
using System.Linq;
using Movies.Controllers;
using Movies.ExternalFacing;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Xunit.Sdk;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Movies.Test
{
    public class ControllerTests
    {
        const string TITLE = "movie title";
        const string GENRE = "movie genre";

        const string MOVIE1_TITLE = "movie1";
        const string MOVIE2_TITLE = "movie2";

        private readonly MovieController _movieController;
        private readonly Mock<IMovieService> _movieService;

        public ControllerTests()
        {
            _movieService = new Mock<IMovieService>();
            _movieController = new MovieController(_movieService.Object);
        }

        [Fact]
        public void GetMoviesByTitle_HappyPath()
        {
            //ARRANGE
            List<Movie> movieList = new List<Movie>();
            movieList.Add(new Movie { Title = MOVIE1_TITLE });
            movieList.Add(new Movie { Title = MOVIE2_TITLE });

            _movieService.Setup(x => x.GetMoviesByTitle(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(movieList);

            //ACT
            var result = _movieController.GetMoviesByTitle(TITLE, 0, 0);


            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.NotNull(okResult.Value);
            Assert.IsType<MovieResult>(okResult.Value);

            var parsedResult = (MovieResult)okResult.Value;
            Assert.Null(parsedResult.ErrorMessage);
            Assert.IsType<List<Movie>>(parsedResult.Movies);
            Assert.NotNull(parsedResult.Movies);
            Assert.Equal(2, parsedResult.Movies.Count());

            var parsedMovieList = (List<Movie>)parsedResult.Movies;
            Assert.Equal(MOVIE1_TITLE, parsedMovieList.First().Title);
            Assert.Equal(MOVIE2_TITLE, parsedMovieList.Last().Title);

            //Check Service is called with only the title passed, rather than title and genre
            _movieService.Verify(x => x.GetMoviesByTitle(It.Is<string>(y=> y==TITLE), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GetMoviesByTitle_Exception()
        {
            const string ERR_MESSAGE = "Error getting movie by name";

            //ARRANGE
            _movieService.Setup(x => x.GetMoviesByTitle(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception(ERR_MESSAGE));

            //ACT
            var result = _movieController.GetMoviesByTitle(TITLE, 0, 0);

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
            var objectResult = (ObjectResult)result;
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.IsType<MovieResult>(objectResult.Value);
            Assert.NotNull(objectResult.Value);
            var parsedResult = (MovieResult)objectResult.Value;
            Assert.NotEmpty(parsedResult.ErrorMessage);
            Assert.Equal(ERR_MESSAGE, parsedResult.ErrorMessage);
          

        }

        [Fact]
        public void GetMoviesByTitle_ReturnNothing()
        {
            //ARRANGE
            _movieService.Setup(x => x.GetMoviesByTitle(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new List<Movie>());

            //ACT
            var result = _movieController.GetMoviesByTitle(TITLE, 0, 0);

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            var notFoundResult = (NotFoundResult)result;
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);

        }

        [Fact]
        public void GetMoviesByByGenre_HappyPath()
        {

            //ARRANGE
            List<Movie> movieList = new List<Movie>();
            movieList.Add(new Movie { Title = MOVIE1_TITLE });
            movieList.Add(new Movie { Title = MOVIE2_TITLE });


            _movieService.Setup(x => x.GetMoviesByGenre(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(movieList);

            //ACT
            var result = _movieController.GetMoviesByGenre(GENRE, 0, 0);


            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.NotNull(okResult.Value);
            Assert.IsType<MovieResult>(okResult.Value);
            var parsedResult = (MovieResult)okResult.Value;
            Assert.Null(parsedResult.ErrorMessage);
            Assert.IsType<List<Movie>>(parsedResult.Movies);
            Assert.NotNull(parsedResult.Movies);
            Assert.Equal(2, parsedResult.Movies.Count());

            var parsedMovieList = (List<Movie>)parsedResult.Movies;
            Assert.Equal(MOVIE1_TITLE, parsedMovieList.First().Title);
            Assert.Equal(MOVIE2_TITLE, parsedMovieList.Last().Title);

            //Check Service is called with title AND genre
            _movieService.Verify(x => x.GetMoviesByGenre(It.Is<string>(z => z == GENRE), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }



        [Fact]
        public void GetMoviesByGenre_Exception()
        {
            const string ERR_MESSAGE = "Error getting movie by name";

            //ARRANGE
            _movieService.Setup(x => x.GetMoviesByGenre(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception(ERR_MESSAGE));

            //ACT
            var result = _movieController.GetMoviesByGenre(GENRE, 0, 0);

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
            var objectResult = (ObjectResult)result;
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.IsType<MovieResult>(objectResult.Value);
            Assert.NotNull(objectResult.Value);
            var parsedResult = (MovieResult)objectResult.Value;
            Assert.NotEmpty(parsedResult.ErrorMessage);
            Assert.Equal(ERR_MESSAGE, parsedResult.ErrorMessage);


            //Check Service is called with title AND genre
            _movieService.Verify(x => x.GetMoviesByGenre(It.Is<string>(z => z == GENRE), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GetMoviesByByGenre_ReturnNothing()
        {
            //ARRANGE
            _movieService.Setup(x => x.GetMoviesByGenre(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new List<Movie>());

            //ACT
            var result = _movieController.GetMoviesByGenre(GENRE, 0, 0);

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            var notFoundResult = (NotFoundResult)result;
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);

            //Check Service is called with title AND genre
            _movieService.Verify(x => x.GetMoviesByGenre(It.Is<string>(y => y == GENRE), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }
    }
}