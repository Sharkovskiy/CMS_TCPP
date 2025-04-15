using contentflow_cms.server.Controllers;
using contentflow_cms.server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Tests.IntegrationTests
{
    [TestFixture]
    public class PostsIntegrationTests
    {
        private ContentflowContext _context;
        private PostsController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ContentflowContext>()
                .UseInMemoryDatabase(databaseName: "TestPostsDb")
                .Options;

            _context = new ContentflowContext(options);
            _controller = new PostsController(_context);

            // Seed the database
            _context.Post.AddRange(
                new Post { Id = 1, Title = "Test Post 1", Body = "Content 1" },
                new Post { Id = 2, Title = "Test Post 2", Body = "Content 2" }
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
            // Act
            var result = await _controller.GetPost(1);

            // Assert
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value.Id, Is.EqualTo(1));
            Assert.That(result.Value.Title, Is.EqualTo("Test Post 1"));
        }

        [Test]
        public async Task CreatePost_ValidPost_ReturnsCreatedAtAction()
        {
            // Arrange
            var newPost = new Post { Title = "New Post", Body = "New Content" };

            // Act
            var result = await _controller.PostPost(newPost);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());
            var createdAtResult = result.Result as CreatedAtActionResult;
            Assert.That(createdAtResult.ActionName, Is.EqualTo("GetPost"));

            // Verify the post was actually created
            var createdPost = await _context.Post.FindAsync(3); // Next available ID
            Assert.That(createdPost, Is.Not.Null);
            Assert.That(createdPost.Title, Is.EqualTo("New Post"));
        }

        [Test]
        public async Task UpdatePost_ValidPost_UpdatesSuccessfully()
        {
            // Arrange
            var updatedPost = new Post { Id = 1, Title = "Updated Post", Body = "Updated Content" };

            // Act
            var result = await _controller.PutPost(1, updatedPost);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());

            // Verify the post was actually updated
            var post = await _context.Post.FindAsync(1);
            Assert.That(post.Title, Is.EqualTo("Updated Post"));
        }

        [Test]
        public async Task DeletePost_ValidId_DeletesSuccessfully()
        {
            // Act
            var result = await _controller.DeletePost(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());

            // Verify the post was actually deleted
            var post = await _context.Post.FindAsync(1);
            Assert.That(post, Is.Null);
        }
    }
}