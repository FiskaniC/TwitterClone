namespace TwitterClone.Unit.Tests
{
    using Moq;
    using System.Text;
    using TwitterClone.Application.Interfaces;
    using TwitterClone.Infrastructure;
    using Xunit;

    public class FileReaderTests
    {
        private readonly string[] fakePaths = new string[] { "fake/" };
        private readonly Mock<IStreamReader> fakeStreamReader;
        public FileReaderTests()
        {
            fakeStreamReader = new Mock<IStreamReader>();
        }

        [Fact]
        public void FileReaderShouldThrowArgumentExceptionWithEmptyFileContents()
        {
            // Arrange
            var expectedErrorMessage = "File has empty contents";
            var fakeMemoryStreamWithContents = new MemoryStream(Encoding.UTF8.GetBytes(""));
            fakeStreamReader.Setup(x => x.StreamReader(It.IsAny<string>())).Returns(new StreamReader(fakeMemoryStreamWithContents));
            var sut = new FileReader(fakeStreamReader.Object);

            // Act
            var result = Assert.Throws<ArgumentException>(() => sut.ReadFileContentsAsArrays(fakePaths));

            // Assert
            Assert.Contains(expectedErrorMessage, result.Message);
            fakeStreamReader.Verify(r => r.StreamReader(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void FileReaderShouldThrowIOExceptionWithMissingFile()
        {
            // Arrange
            var expectedErrorMessage = "The file could not be read, error message";
            fakeStreamReader.Setup(x => x.StreamReader(It.IsAny<string>())).Throws(new IOException());
            var sut = new FileReader(fakeStreamReader.Object);

            // Act
            var  result = Assert.Throws<IOException>(() => sut.ReadFileContentsAsArrays(fakePaths));

            // Assert
            Assert.Contains(expectedErrorMessage, result.Message);
            fakeStreamReader.Verify(r => r.StreamReader(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void FileReaderShouldReadFileContentsAsArrays()
        {
            // Arrange
            var fakeContents = "fake file contents";
            var fakeContentsArray = new List<string[]>() { new string[] { fakeContents } };
            var fakeMemoryStreamWithContents = new MemoryStream(Encoding.UTF8.GetBytes(fakeContents));
            fakeStreamReader.Setup(x => x.StreamReader(It.IsAny<string>())).Returns(new StreamReader(fakeMemoryStreamWithContents));
            var sut = new FileReader(fakeStreamReader.Object);

            //Act
            var result = sut.ReadFileContentsAsArrays(fakePaths);

            //Assert
            Assert.Equal(fakeContentsArray, result);
            fakeStreamReader.Verify(r => r.StreamReader(It.IsAny<string>()), Times.Once);
        }
    }
}
