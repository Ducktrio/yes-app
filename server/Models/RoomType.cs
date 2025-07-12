using System;
using System.ComponentModel.DataAnnotations;

namespace Yes.Models;

public class RoomType
{
    [Key]
    public string Id { get; set; } = string.Empty;

    public virtual ICollection<Room> Rooms { get; set; } = [];

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(255)]
    public string? Description { get; set; } = null;
    [Required]
    public int Price { get; set; } = 0;
}
