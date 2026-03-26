using FluentAssertions;
using Weather.Model.Extensions;

namespace Weather.Tests
{
    public class MiscellaneousTests
    {
        public class ToSnakeCase
        {
            [Test]
            public async Task WithNull_ReturnsNull()
            {
                // Arrange
                string? input = null;

                // Act
                var result = StringExtensions.ToSnakeCase(input!);

                // Assert
                result.Should().BeNull();

                await Task.CompletedTask;
            }

            [Test]
            public async Task WithEmptyString_ReturnsEmptyString()
            {
                // Arrange
                const string input = "";

                // Act
                var result = StringExtensions.ToSnakeCase(input);

                // Assert
                result.Should().BeEmpty();

                await Task.CompletedTask;
            }

            [Test]
            public async Task WithWhitespace_ReturnsInputUnchanged()
            {
                // Arrange
                const string input = "   ";

                // Act
                var result = StringExtensions.ToSnakeCase(input);

                // Assert
                result.Should().Be(input);

                await Task.CompletedTask;
            }

            [Test]
            public async Task WithPascalCase_ReturnsSnakeCase()
            {
                // Arrange
                const string input = "ObservedAt";

                // Act
                var result = StringExtensions.ToSnakeCase(input);

                // Assert
                result.Should().Be("observed_at");

                await Task.CompletedTask;
            }

            [Test]
            public async Task WithSingleWordStartingUppercase_ReturnsLowercaseWord()
            {
                // Arrange
                const string input = "Location";

                // Act
                var result = StringExtensions.ToSnakeCase(input);

                // Assert
                result.Should().Be("location");

                await Task.CompletedTask;
            }

            [Test]
            public async Task WithAlreadyLowercaseWord_ReturnsInputUnchanged()
            {
                // Arrange
                const string input = "inside";

                // Act
                var result = StringExtensions.ToSnakeCase(input);

                // Assert
                result.Should().Be("inside");

                await Task.CompletedTask;
            }

            [Test]
            public async Task WithAllUppercaseWord_ReturnsSingleLowercaseWordWithoutExtraUnderscores()
            {
                // Arrange
                const string input = "INSIDE";

                // Act
                var result = StringExtensions.ToSnakeCase(input);

                // Assert
                result.Should().Be("inside");

                await Task.CompletedTask;
            }

            [Test]
            public async Task WithScreamingSnakeCase_ReturnsLowerSnakeCase()
            {
                // Arrange
                const string input = "VERY_HIGH";

                // Act
                var result = StringExtensions.ToSnakeCase(input);

                // Assert
                result.Should().Be("very_high");

                await Task.CompletedTask;
            }
        }
    }
}