using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoCleanArchitecture.Application.Dtos;
using TodoCleanArchitecture.Application.Interfaces;
using TodoCleanArchitecture.Application.UseCases;
using TodoCleanArchitecture.Domain.Entities;

namespace TodoCleanArchitecture.Application.Tests
{
    public class CreateTodoUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_Should_CreatedTodo_And_ReturnResponse()
        {
            // Arrange
            var repo = new Mock<ITodoRepository>();

            repo.Setup(r => r.AddAsync(It.IsAny<TodoItem>()))
                .ReturnsAsync((TodoItem t) => t);

            var useCase = new CreateTodoUseCase(repo.Object);

            var req = new CreateTodoRequest
            {
                Title = "Test Todo"
            };

            // Act
            var result = await useCase.ExecuteAsync(req);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Todo", result.Title);

            repo.Verify(r => r.AddAsync(It.Is<TodoItem>(t => t.Title == "Test Todo")), Times.Once);
        }
    }
}
