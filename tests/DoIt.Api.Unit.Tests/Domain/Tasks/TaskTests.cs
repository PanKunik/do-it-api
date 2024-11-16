using DoIt.Api.TestUtils;
using FluentAssertions;
using Task = DoIt.Api.Domain.Tasks.Task;

namespace DoIt.Api.Unit.Tests.Domain.Tasks
{
    public class TaskTests
    {
        [Fact]
        public async System.Threading.Tasks.Task Task_WhenCalled_ShouldCreateExpectedObject()
        {
            // Arrange
            var createTask = () => new Task(
                Constants.Tasks.TaskId,
                Constants.Tasks.Title,
                Constants.Tasks.CreatedAt,
                Constants.Tasks.Done,
                Constants.Tasks.Important
            );

            // Act
            var result = createTask();

            // Assert
            result
                .Should()
                .NotBeNull();

            result
                .Should()
                .Match<Task>(
                    t => t.Id == Constants.Tasks.TaskId
                      && t.Title == Constants.Tasks.Title
                      && t.CreatedAt == Constants.Tasks.CreatedAt
                      && t.IsDone == Constants.Tasks.Done
                      && t.IsImportant == Constants.Tasks.Important
                );

            await System.Threading.Tasks.Task.CompletedTask;
        }

        [Fact]
        public async System.Threading.Tasks.Task Task_WhenPassedNullTaskId_ShouldThrowException()
        {
            // Arrange
            var createTask = () => new Task(
                taskId: null!,
                title: Constants.Tasks.Title,
                createdAt: Constants.Tasks.CreatedAt,
                isDone: Constants.Tasks.NotDone,
                Constants.Tasks.NotImportant
            );

            // Act & Assert
            createTask
                .Should()
                .ThrowExactly<ArgumentNullException>()
                .WithParameterName("id");

            await System.Threading.Tasks.Task.CompletedTask;
        }

        [Fact]
        public async System.Threading.Tasks.Task Task_WhenPassedNullTitle_ShouldThrowException()
        {
            // Arrange
            var createTask = () => new Task(
                taskId: Constants.Tasks.TaskId,
                title: null!,
                createdAt: Constants.Tasks.CreatedAt,
                isDone: Constants.Tasks.NotDone,
                Constants.Tasks.Important
            );

            // Act & Assert
            createTask
                .Should()
                .ThrowExactly<ArgumentNullException>()
                .WithParameterName("title");

            await System.Threading.Tasks.Task.CompletedTask;
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public async System.Threading.Tasks.Task ChangeState_WhenCalled_ShouldSwitchIsDoneFlag(
            bool actualState, bool desiredState
        )
        {
            // Arrange
            var cut = new Task(
                Constants.Tasks.TaskId,
                Constants.Tasks.Title,
                Constants.Tasks.CreatedAt,
                actualState,
                Constants.Tasks.NotImportant
            );

            // Act
            cut.ChangeState();

            // Assert
            cut
                .IsDone
                .Should()
                .Be(desiredState);

            await System.Threading.Tasks.Task.CompletedTask;
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public async System.Threading.Tasks.Task ChangeImportance_WhenCalled_ShouldSwitchIsImportantFlag(
            bool actualState, bool desiredState    
        )
        {
            // Arrange
            var cut = new Task(
                Constants.Tasks.TaskId,
                Constants.Tasks.Title,
                Constants.Tasks.CreatedAt,
                Constants.Tasks.NotDone,
                actualState
            );

            // Act
            cut.ChangeImportance();

            // Assert
            cut
                .IsImportant
                .Should()
                .Be(desiredState);

            await System.Threading.Tasks.Task.CompletedTask;
        }

        [Fact]
        public async System.Threading.Tasks.Task Equals_WhenCalledForObjectWithOtherValuesButSameTaskId_ShouldReturnTrue()
        {
            // Arrange
            var left = new Task(
                Constants.Tasks.TaskId,
                Constants.Tasks.TitleFromIndex(0),
                Constants.Tasks.CreatedAtFromIndex(0),
                Constants.Tasks.NotDone,
                Constants.Tasks.NotImportant
            );

            var right = new Task(
                Constants.Tasks.TaskId,
                Constants.Tasks.TitleFromIndex(1),
                Constants.Tasks.CreatedAtFromIndex(1),
                Constants.Tasks.Done,
                Constants.Tasks.Important
            );

            // Act
            var result = left.Equals(right);

            // Assert
            result
                .Should()
                .BeTrue();

            await System.Threading.Tasks.Task.CompletedTask;
        }

        [Fact]
        public async System.Threading.Tasks.Task Equals_WhenCalledForObjectWithSameValuesButOtherTaskId_ShouldReturnFalse()
        {
            // Arrange
            var left = new Task(
                Constants.Tasks.TaskIdFromIndex(0),
                Constants.Tasks.Title,
                Constants.Tasks.CreatedAt,
                Constants.Tasks.NotDone,
                Constants.Tasks.NotImportant
            );

            var right = new Task(
                Constants.Tasks.TaskIdFromIndex(1),
                Constants.Tasks.Title,
                Constants.Tasks.CreatedAt,
                Constants.Tasks.NotDone,
                Constants.Tasks.NotImportant
            );

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
