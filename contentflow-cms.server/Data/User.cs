using System.ComponentModel.DataAnnotations;

namespace contentflow_cms.server.Data;

public class User
{
    [Key]
    public int Id { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required bool EmailConfirmed { get; set; }
}
