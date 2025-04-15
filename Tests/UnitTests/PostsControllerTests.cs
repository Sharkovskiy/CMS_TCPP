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
    public class PostsControllerTests
    {
        private Mock<ContentflowContext> _mockContext;
        private PostsController _controller;
        private Mock<DbSet<Post>> _mockPosts;
        private DbContextOptions<ContentflowContext> _options;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<ContentflowContext>()
                .UseInMemoryDatabase(databaseName: "TestPostsDb")
                .Options;

            _mockContext = new Mock<ContentflowContext>(_options);
            _mockPosts = new Mock<DbSet<Post>>();

            // Create a list of posts
            var posts = new List<Post>
            {
                new Post { Id = 1, Title = "Test Post 1", Body = "Content 1" },
                new Post { Id = 2, Title = "Test Post 2", Body = "Content 2" }
            }.AsQueryable();

            // Setup the mock DbSet
            _mockPosts.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(posts.Provider);
            _mockPosts.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(posts.Expression);
            _mockPosts.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(posts.ElementType);
            _mockPosts.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(posts.GetEnumerator());

            // Setup the mock context to return the mock DbSet
            var mockSet = _mockPosts.Object;
            _mockContext.SetupGet(c => c.Post).Returns(mockSet);

            _controller = new PostsController(_mockContext.Object);
        }

        [Test]
        public async Task GetPosts_ReturnsAllPosts()
        {
            // Act
            var result = await _controller.GetPost();

            // Assert
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetPost_WithValidId_ReturnsPost()
        {
            // Arrange
            var post = new Post { Id = 1, Title = "Test Post", Body = "Content" };
            _mockContext.Setup(c => c.Post.FindAsync(1)).ReturnsAsync(post);

            // Act
            var result = await _controller.GetPost(1);

            // Assert
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value.Id, Is.EqualTo(1));
        }

        [Test]
        public async Task GetPost_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockContext.Setup(c => c.Post.FindAsync(1)).ReturnsAsync((Post)null);

            // Act
            var result = await _controller.GetPost(1);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task CreatePost_ValidPost_ReturnsCreatedAtAction()
        {
            // Arrange
            var post = new Post { Id = 1, Title = "New Post", Body = "Content" };
            _mockContext.Setup(c => c.Post.Add(post));
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _controller.PostPost(post);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());
            var createdAtResult = result.Result as CreatedAtActionResult;
            Assert.That(createdAtResult.ActionName, Is.EqualTo("GetPost"));
            Assert.That(createdAtResult.RouteValues["id"], Is.EqualTo(1));
        }

        [Test]
        public async Task UpdatePost_ValidPost_ReturnsNoContent()
        {
            // Arrange
            var post = new Post { Id = 1, Title = "Updated Post", Body = "Updated Content" };
            _mockContext.Setup(c => c.Post.FindAsync(1)).ReturnsAsync(post);
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _controller.PutPost(1, post);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task DeletePost_ValidId_ReturnsNoContent()
        {
            // Arrange
            var post = new Post { Id = 1, Title = "Post to Delete", Body = "Content" };
            _mockContext.Setup(c => c.Post.FindAsync(1)).ReturnsAsync(post);
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _controller.DeletePost(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }
    }
} 