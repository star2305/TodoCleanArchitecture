using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoCleanArchitecture.Application.Interfaces;
using TodoCleanArchitecture.Application.UseCases;
using TodoCleanArchitecture.Domain.Entities;

namespace TodoCleanArchitecture.Application.Tests
{
    public class CompleteTodoUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_WhenTodoExists_ShouldMarkComplete_AndUpdate()
        {
            // Arrange
            var repo = new Mock<ITodoRepository>();

            var todo = new TodoItem("Test Todo");
            repo.Setup(r => r.GetByIdAsync(todo.Id)).ReturnsAsync(todo);

            var useCase = new UpdateTodoUseCase(repo.Object);

            // Act
            var result = await useCase.ExecuteAsync(todo.Id);

            // Assert
            Assert.True(result);
            Assert.True(todo.IsCompleted);

            repo.Verify(r => r.UpdateAsync(todo), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WhenTodoDoesNotFound_ShouldReturnFalse()
        {
            // Arrange
            var repo = new Mock<ITodoRepository>();
            repo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((TodoItem?)null);

            var useCase = new UpdateTodoUseCase(repo.Object);

            // Act
            var result = await useCase.ExecuteAsync(99); // Non-existent ID

            // Assert
            Assert.False(result);

            repo.Verify(r => r.UpdateAsync(It.IsAny<TodoItem>()), Times.Never);
        }
    }
}
