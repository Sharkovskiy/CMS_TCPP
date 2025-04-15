using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using contentflow_cms.server.Controllers;
using contentflow_cms.server.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Tests.IntegrationTests
{
    [TestFixture]
    public class UsersIntegrationTests
    {
        private ContentflowContext _context;
        private UsersController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ContentflowContext>()
                .UseInMemoryDatabase(databaseName: "TestUsersDb")
                .Options;

            _context = new ContentflowContext(options);
            _controller = new UsersController(_context);

            // Seed the database
            _context.User.AddRange(
                new User { Id = 1, UserName = "user1", Email = "user1@test.com", Password = "pass1", PhoneNumber = "123", EmailConfirmed = true },
                new User { Id = 2, UserName = "user2", Email = "user2@test.com", Password = "pass2", PhoneNumber = "456", EmailConfirmed = true }
            );
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
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
            // Act
            var result = await _controller.GetUser(1);

            // Assert
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value.Id, Is.EqualTo(1));
            Assert.That(result.Value.UserName, Is.EqualTo("user1"));
        }

        [Test]
        public async Task CreateUser_ValidUser_ReturnsCreatedAtAction()
        {
            // Arrange
            var newUser = new User 
            { 
                UserName = "newuser", 
                Email = "new@test.com", 
                Password = "pass", 
                PhoneNumber = "789", 
                EmailConfirmed = true 
            };

            // Act
            var result = await _controller.PostUser(newUser);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());
            var createdAtResult = result.Result as CreatedAtActionResult;
            Assert.That(createdAtResult.ActionName, Is.EqualTo("GetUser"));
            
            // Verify the user was actually created
            var createdUser = await _context.User.FindAsync(3); // Next available ID
            Assert.That(createdUser, Is.Not.Null);
            Assert.That(createdUser.UserName, Is.EqualTo("newuser"));
        }

        [Test]
        public async Task UpdateUser_ValidUser_UpdatesSuccessfully()
        {
            // Arrange
            var updatedUser = new User 
            { 
                Id = 1, 
                UserName = "updateduser", 
                Email = "updated@test.com", 
                Password = "newpass", 
                PhoneNumber = "999", 
                EmailConfirmed = true 
            };

            // Act
            var result = await _controller.PutUser(1, updatedUser);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
            
            // Verify the user was actually updated
            var user = await _context.User.FindAsync(1);
            Assert.That(user.UserName, Is.EqualTo("updateduser"));
            Assert.That(user.PhoneNumber, Is.EqualTo("999"));
        }

        [Test]
        public async Task DeleteUser_ValidId_DeletesSuccessfully()
        {
            // Act
            var result = await _controller.DeleteUser(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
            
            // Verify the user was actually deleted
            var user = await _context.User.FindAsync(1);
            Assert.That(user, Is.Null);
        }
    }
} 