using System.ComponentModel.DataAnnotations;

namespace contentflow_cms.server.Data;

public class Comment
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Author { get; set; }
    public string Content { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;

    public int PostId { get; set; }
    public Post Post { get; set; }
}
