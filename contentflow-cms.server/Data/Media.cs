using System.ComponentModel.DataAnnotations;

namespace contentflow_cms.server.Data;

public class Media
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string FileName { get; set; }

    public string FileType { get; set; }
    
    public string FilePath { get; set; }
    
    public long FileSize { get; set; }
    
    public DateTime UploadDate { get; set; }
    
    public List<Post> Posts { get; set; }
} 