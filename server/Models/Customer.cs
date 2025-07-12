using System;
using System.ComponentModel.DataAnnotations;

namespace Yes.Models;

public class Customer
{
    [Key]
    public string Id { get; set; } = string.Empty;

    public virtual ICollection<RoomTicket> RoomTickets { get; set; } = [];
    public virtual ICollection<ServiceTicket> ServiceTickets { get; set; } = [];

    [Required]
    [MaxLength(100)]
    public string Courtesy_title { get; set; } = string.Empty;
    [Required]
    [MaxLength(100)]
    public string Full_name { get; set; } = string.Empty;
    [Required]
    public int Age { get; set; } = 0;
    [MaxLength(100)]
    public string? Phone_number { get; set; } = null;
    [MaxLength(255)]
    public string? Contact_info { get; set; } = null;
}
