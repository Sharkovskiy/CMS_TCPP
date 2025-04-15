using System.ComponentModel.DataAnnotations;

namespace contentflow_cms.server.Data;

public class Tag
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public List<Post> Posts { get; set; }
} 