using DoIt.Api.Domain.Tasks;
using DoIt.Api.TestUtils;
using FluentAssertions;

namespace DoIt.Api.Unit.Tests.Domain.Tasks
{
    public class TitleTests
    {
        [Theory]
        [InlineData("T")]
        [InlineData("Task title")]
        [InlineData("M%@HUf3Hp#7AG%JyweXCvwaOWaRw2aHY+#HGp#tQCu$+YtxbG9WysXKr*xOw0vJey@XTst3#1BTOubDN5F2OYOtoHfEc15FOq3Qs")]
        public async System.Threading.Tasks.Task Title_WhenCalledWithProperValue_ShouldReturnExpectedObject(string value)
        {
            // Arrange
            var createTitle = () => new Title(value);

            // Act
            var result = createTitle();

            // Assert
            result
                .Should()
                .NotBeNull();

            result
                .Should()
                .Match<Title>(t => t.Value == value);

            await System.Threading.Tasks.Task.CompletedTask;
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async System.Threading.Tasks.Task Title_WhenPassedNullOrWhiteSpace_ShouldThrowException(string value)
        {
            // Arrange
            var createTitle = () => new Title(value);

            // Act & Assert
            createTitle
                .Should()
                .ThrowExactly<ArgumentException>()
                .WithMessage("Value cannot be empty. (Parameter 'value')")
                .WithParameterName("value");

            await System.Threading.Tasks.Task.CompletedTask;
        }

        [Fact]
        public async System.Threading.Tasks.Task Title_WhenPassedValueWithOver100Characters_ShouldThrowException()
        {
            // Arrange
            var createTitle = () => new Title("dwWY3pM5eakP3qbsku37HrW3bMEaA@%T9Q+aKZeRW%FzWwwucpjnFRXU2q9$pH!$j#M+azz72WQ&4vrFbw*8%eca5r$kps48d%REs");

            // Act & Assert
            createTitle
                .Should()
                .ThrowExactly<ArgumentException>()
                .WithMessage("Title cannot exceed 100 characters. (Parameter 'value')")
                .WithParameterName("value");

            await System.Threading.Tasks.Task.CompletedTask;
        }

        [Fact]
        public async System.Threading.Tasks.Task Equals_WhenCalledForObjectWithSameValue_ShouldReturnTrue()
        {
            // Arrange
            var left = new Title(Constants.Tasks.Title.Value);
            var right = new Title(Constants.Tasks.Title.Value);

            // Act
            var result = left.Equals(right);

            // Assert
            result
                .Should()
                .BeTrue();

            await System.Threading.Tasks.Task.CompletedTask;
        }

        [Fact]
        public async System.Threading.Tasks.Task Equals_WhenCalledForObjectWithDifferentValue_ShouldReturnFalse()
        {
            // Arrange
            var left = new Title(Constants.Tasks.TitleFromIndex(0).Value);
            var right = new Title(Constants.Tasks.TitleFromIndex(1).Value);

            // Act
            var result = left.Equals(right);

            // Assert
            result
                .Should()
                .BeFalse();

            await System.Threading.Tasks.Task.CompletedTask;
        }
    }
}
