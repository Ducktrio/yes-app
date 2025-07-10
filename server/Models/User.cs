using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yes.Models;

public class User
{
    [Key]
    public string Id { get; set; } = string.Empty;

    [Required]
    public string Role_id { get; set; } = string.Empty;
    [ForeignKey("Role_id")]
    public virtual Role Role { get; set; } = null!;
    
    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;
    [Required]
    [MaxLength(100)]
    public string Password { get; set; } = string.Empty;
    [Required]
    [MaxLength(255)]
    public string Description { get; set; } = string.Empty;
}
