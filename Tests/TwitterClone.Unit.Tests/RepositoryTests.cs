using Moq;
using TwitterClone.Application.Interfaces;
using TwitterClone.Repository.Repositories;
using Xunit;

namespace TwitterClone.Unit.Tests
{
    public  class RepositoryTests
    {
        private readonly Mock<IFileReader> mockFileReader;

        public RepositoryTests()
        {
            mockFileReader = new Mock<IFileReader>();
        }

        [Fact]
        public void RepositoryShouldFetchUsersData()
        {
            // Arrange
            var validUser1 = "James";
            var validUser2 = "John";
            var fakeContents = new List<string[]>() { new string[] { $"{validUser1} follows {validUser2}" } };
            var fakePaths = new string[] { "fake file path" };
            mockFileReader.Setup(x => x.ReadFileContentsAsArrays(fakePaths)).Returns(fakeContents);
            var sut = new UsersRepository(mockFileReader.Object);

            // Act
            var result = sut.Get(fakePaths);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(validUser1, result.First().Username);
            Assert.Equal(validUser2, result.Last().Username);
            Assert.Equal(validUser2, result.First().Followers.Single().Username);
            mockFileReader.Verify(r => r.ReadFileContentsAsArrays(fakePaths), Times.Once);
        }

        [Fact]
        public void RepositoryShouldFetchPostsData()
        {
            // Arrange
            var username = "James";
            var message = "Test message";
            var fakeContents = new List<string[]>() { new string[] { $"{username}> {message}" } };
            var fakePaths = new string[] { "fake file path" };
            mockFileReader.Setup(x => x.ReadFileContentsAsArrays(fakePaths)).Returns(fakeContents);
            var sut = new PostsRepository(mockFileReader.Object);

            // Act
            var result = sut.Get(fakePaths);

            // Assert
            Assert.Equal(1, result.Count);
            Assert.Equal(username, result.Single().Username);
            Assert.Equal(message, result.Single().Message);
            mockFileReader.Verify(r => r.ReadFileContentsAsArrays(fakePaths), Times.Once);
        }
    }
}
