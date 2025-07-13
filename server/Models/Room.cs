using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yes.Models;

public class Room
{
    [Key]
    public string Id { get; set; } = string.Empty;

    public virtual ICollection<RoomTicket> RoomTickets { get; set; } = [];
    public virtual ICollection<ServiceTicket> ServiceTickets { get; set; } = [];
    [Required]
    public string RoomType_id { get; set; } = string.Empty;
    [ForeignKey("RoomType_id")]
    public virtual RoomType RoomType { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string Label { get; set; } = string.Empty;
    [Required]
    public int Status { get; set; } = 0;
}
