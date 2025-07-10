using System;
using System.ComponentModel.DataAnnotations;

namespace Yes.Models;

public class Role
{
    [Key]
    public string Id { get; set; } = string.Empty;

    public virtual ICollection<User> Users { get; set; } = [];
    
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;
}
