using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yes.Models;

public class ServiceTicket
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
    [Required]
    public string Service_id { get; set; } = string.Empty;
    [ForeignKey("Service_id")]
    public virtual Service Service { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    public string Details { get; set; } = string.Empty;
    [Required]
    public int Status { get; set; } = 0;
}
