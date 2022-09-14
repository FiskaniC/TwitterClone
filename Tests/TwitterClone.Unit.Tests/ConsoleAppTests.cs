namespace TwitterClone.Unit.Tests
{
    using TwitterClone.Application.Constants;
    using TwitterClone.Application.Validators;
    using Xunit;

    public class ConsoleAppTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("fake.txt")]
        [InlineData("user.txt")]
        [InlineData("tweet.txt")]
        public void ApplicationShouldThrowExceptionWithIncorrectArguments(params string[] arguments)
        {
            // Arrange
            var missingArguements = ApplicationConstants.MandatoryArguments.Except(arguments);
            var expectedErrorMessage = $"The files were not passed as arguments, missing arguments: {string.Join(", ", missingArguements.ToList())}";
            var argumentValidator = new ArgumentValidator();

            try
            {
                // Act
                argumentValidator.ValidateArguments(arguments);

                // Assert
                Assert.Fail(expectedErrorMessage);
            }
            catch (ArgumentException exception)
            {
                // Assert
                Assert.Equal(expectedErrorMessage, exception.Message);
            }
        }

        [Fact]
        public void ApplicationShouldContinueWithCorrectArguments()
        {
            // Arrange
            var arguments = new string[] { "user.txt", "tweet.txt" };
            var argumentValidator = new ArgumentValidator();

            try
            {
                // Act
                argumentValidator.ValidateArguments(arguments);
            }
            catch (ArgumentException)
            {
                // Assert
                Assert.Fail("Expected no ArgumentException to be thrown");
            }
        }
    }
}