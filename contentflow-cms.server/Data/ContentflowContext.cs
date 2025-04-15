using Microsoft.EntityFrameworkCore;

namespace contentflow_cms.server.Data;

public class ContentflowContext(DbContextOptions<ContentflowContext> options) : DbContext(options)
{
    public DbSet<User> User { get; set; } = default!;
    public DbSet<Comment> Comment { get; set; } = default!;
    public DbSet<Post> Post { get; set; } = default!;
    public DbSet<Tag> Tags { get; set; } = default!;
    public DbSet<Category> Categories { get; set; } = default!;
    public DbSet<Media> Media { get; set; } = default!;
}
