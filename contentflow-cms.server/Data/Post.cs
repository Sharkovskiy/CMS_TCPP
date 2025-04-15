using System.ComponentModel.DataAnnotations;

namespace contentflow_cms.server.Data;

public class Post
{
    [Key]
    public int Id { get; set; }

    public string Title { get; set; }
    public string Body { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }

    public List<Post> Posts { get; set; }
}
