using System;
using System.ComponentModel.DataAnnotations;

namespace Yes.Models;

public class Service
{
    [Key]
    public string Id { get; set; } = string.Empty;

    public virtual ICollection<ServiceTicket> ServiceTickets { get; set; } = [];

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [Required]
    public int Price { get; set; } = 0;
}
