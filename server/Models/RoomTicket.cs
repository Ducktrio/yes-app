using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yes.Models;

public class RoomTicket
{
    [Key]
    public string Id { get; set; } = string.Empty;

    [Required]
    public string Customer_id { get; set; } = string.Empty;
    [ForeignKey("Customer_id")]
    public virtual Customer Customer { get; set; } = null!;
    [Required]
    public string Room_id { get; set; } = string.Empty;
    [ForeignKey("Room_id")]
    public virtual Room Room { get; set; } = null!;

    public DateTime? CheckInDate { get; set; } = null;
    public DateTime? CheckOutDate { get; set; } = null;
    [Required]
    public int Number_of_occupants { get; set; } = 1;
    [Required]
    public int Status { get; set; } = 0;
    [Required]
    public DateTime Created_at { get; set; } = DateTime.Now;
}
