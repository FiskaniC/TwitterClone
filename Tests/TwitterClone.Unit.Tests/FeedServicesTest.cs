namespace TwitterClone.Unit.Tests
{
    using Moq;
    using TwitterClone.Application.Exceptions;
    using TwitterClone.Application.Interfaces;
    using TwitterClone.Application.Models;
    using TwitterClone.Application.Services;
    using Xunit;

    public class FeedServicesTest
    {
        private readonly Mock<IRepository<User>> mockUsersRepository;
        private readonly Mock<IRepository<Post>> mockPostsRepository;

        public FeedServicesTest()
        {
            mockUsersRepository = new Mock<IRepository<User>>();
            mockPostsRepository = new Mock<IRepository<Post>>();
        }

        [Fact]
        public void SimulateFeedShouldThrowNotFoundExceptionBecauseUserInTweetTXTIsNotInUserTXT()
        {
            // Arrange
            var invalidUser = "Jim";
            mockUsersRepository.Setup(x => x.Get(It.IsAny<string[]>())).Returns(new List<User> { new User("TestUser") });
            mockPostsRepository.Setup(x => x.Get(It.IsAny<string[]>())).Returns(new List<Post> { new Post { Username = invalidUser, Message = "Should fail"} });
            var sut = new FeedService(mockUsersRepository.Object, mockPostsRepository.Object);
            var fakeArgs = new string[] { "user.txt", "tweet.txt" };
            var expectedErrorMessage = $"Users do not exist from tweet.txt, users [{invalidUser}]";

            // Act
            var result = Assert.Throws<NotFoundException>(() => sut.SimulateFeed(fakeArgs));

            // Assert
            Assert.Contains(expectedErrorMessage, result.Message);
            mockUsersRepository.Verify(r => r.Get(It.IsAny<string[]>()), Times.Once);
            mockPostsRepository.Verify(r => r.Get(It.IsAny<string[]>()), Times.Once);
        }

        [Theory, MemberData(nameof(ArgumentExceptionData))]
        public void SimulateFeedShouldThrowArgumentExceptionBecauseContentNotFormatedCorrectly(IList<User> users, IList<Post> posts)
        {
            // Arrange
            mockUsersRepository.Setup(x => x.Get(It.IsAny<string[]>())).Returns(users);
            mockPostsRepository.Setup(x => x.Get(It.IsAny<string[]>())).Returns(posts);
            var sut = new FeedService(mockUsersRepository.Object, mockPostsRepository.Object);
            var fakeArgs = new string[] { "user.txt", "tweet.txt" };
            var expectedErrorMessage = $"User or tweet contents does not exist";

            // Act
            var result = Assert.Throws<ArgumentException>(() => sut.SimulateFeed(fakeArgs));

            // Assert
            Assert.Contains(expectedErrorMessage, result.Message);
            mockUsersRepository.Verify(r => r.Get(It.IsAny<string[]>()), Times.Once);
            mockPostsRepository.Verify(r => r.Get(It.IsAny<string[]>()), Times.Once);
        }


        // todo: validate characters?
        [Fact]
        public void SimulateFeedShouldRunWithNoExceptions()
        {
            // Arrange 
            var validUsername = "TestUser";
            mockUsersRepository.Setup(x => x.Get(It.IsAny<string[]>())).Returns(new List<User> { new User(validUsername) });
            mockPostsRepository.Setup(x => x.Get(It.IsAny<string[]>())).Returns(new List<Post> { new Post { Username = validUsername, Message = "Test message" } });
            var sut = new FeedService(mockUsersRepository.Object, mockPostsRepository.Object);
            var fakeArgs = new string[] { "user.txt", "tweet.txt" };

            // Act
            var result = sut.SimulateFeed(fakeArgs);

            // Assert
            Assert.True(result);
            mockUsersRepository.Verify(r => r.Get(It.IsAny<string[]>()), Times.Once);
            mockPostsRepository.Verify(r => r.Get(It.IsAny<string[]>()), Times.Once);
        }

        public static IEnumerable<object[]> ArgumentExceptionData =>
             new List<object[]>
             {
                 new object[] { null, null },
                 new object[] { null, new List<Post> { new Post() } },
                 new object[] { new List<User>() { new User() }, null },
                 new object[] { new List<User>(), new List<Post>() { new Post() } },
                 new object[] { new List<User>() { new User() }, new List<Post>() },
                 new object[] { new List<User>(), new List<Post>() },
             };
    }
}
