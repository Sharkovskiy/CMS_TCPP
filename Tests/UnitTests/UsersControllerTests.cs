using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Moq;
using contentflow_cms.server.Controllers;
using contentflow_cms.server.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Tests.UnitTests
{
    [TestFixture]
    public class UsersControllerTests
    {
        private Mock<ContentflowContext> _mockContext;
        private UsersController _controller;
        private Mock<DbSet<User>> _mockUsers;
        private DbContextOptions<ContentflowContext> _options;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<ContentflowContext>()
                .UseInMemoryDatabase(databaseName: "TestUsersDb")
                .Options;

            _mockContext = new Mock<ContentflowContext>(_options);
            _mockUsers = new Mock<DbSet<User>>();

            // Create a list of users
            var users = new List<User>
            {
                new User { Id = 1, UserName = "user1", Email = "user1@test.com", Password = "pass1", PhoneNumber = "123", EmailConfirmed = true },
                new User { Id = 2, UserName = "user2", Email = "user2@test.com", Password = "pass2", PhoneNumber = "456", EmailConfirmed = true }
            }.AsQueryable();

            // Setup the mock DbSet
            _mockUsers.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            _mockUsers.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            _mockUsers.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            _mockUsers.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            // Setup the mock context to return the mock DbSet
            var mockSet = _mockUsers.Object;
            _mockContext.SetupGet(c => c.User).Returns(mockSet);

            _controller = new UsersController(_mockContext.Object);
        }

        [Test]
        public async Task GetUsers_ReturnsAllUsers()
        {
            // Act
            var result = await _controller.GetUser();

            // Assert
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetUser_WithValidId_ReturnsUser()
        {
            // Arrange
            var user = new User { Id = 1, UserName = "testuser", Email = "test@test.com", Password = "pass", PhoneNumber = "123", EmailConfirmed = true };
            _mockContext.Setup(c => c.User.FindAsync(1)).ReturnsAsync(user);

            // Act
            var result = await _controller.GetUser(1);

            // Assert
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value.Id, Is.EqualTo(1));
            Assert.That(result.Value.UserName, Is.EqualTo("testuser"));
        }

        [Test]
        public async Task CreateUser_ValidUser_ReturnsCreatedAtAction()
        {
            // Arrange
            var user = new User { Id = 1, UserName = "newuser", Email = "new@test.com", Password = "pass", PhoneNumber = "123", EmailConfirmed = true };
            _mockContext.Setup(c => c.User.Add(user));
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _controller.PostUser(user);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());
            var createdAtResult = result.Result as CreatedAtActionResult;
            Assert.That(createdAtResult.ActionName, Is.EqualTo("GetUser"));
            Assert.That(createdAtResult.RouteValues["id"], Is.EqualTo(1));
        }

        [Test]
        public async Task UpdateUser_ValidUser_ReturnsNoContent()
        {
            // Arrange
            var user = new User { Id = 1, UserName = "updateduser", Email = "updated@test.com", Password = "pass", PhoneNumber = "123", EmailConfirmed = true };
            _mockContext.Setup(c => c.User.FindAsync(1)).ReturnsAsync(user);
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _controller.PutUser(1, user);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteUser_ValidId_ReturnsNoContent()
        {
            // Arrange
            var user = new User { Id = 1, UserName = "todelete", Email = "delete@test.com", Password = "pass", PhoneNumber = "123", EmailConfirmed = true };
            _mockContext.Setup(c => c.User.FindAsync(1)).ReturnsAsync(user);
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _controller.DeleteUser(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }
    }
} 