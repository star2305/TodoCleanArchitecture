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
    public class DeleteTodoUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_WhenTodoExists_ShouldDelete_AndReturnTrue()
        {             
            // Arrange
            var repo = new Mock<ITodoRepository>();
            var todo = new TodoItem("Test Todo");

            repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(todo);
            var useCase = new DeleteTodoUseCase(repo.Object);

            // Act
            var result = await useCase.ExecuteAsync(1);

            // Assert
            Assert.True(result);
            repo.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WhenTodoDoesNotFound_ShouldReturnFalse()
        {
            // Arrange
            var repo = new Mock<ITodoRepository>();
            repo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((TodoItem?)null);

            var useCase = new DeleteTodoUseCase(repo.Object);

            // Act
            var result = await useCase.ExecuteAsync(99); // Non-existent ID

            // Assert
            Assert.False(result);

            repo.Verify(r => r.DeleteAsync(99), Times.Never);
        }
    }
}