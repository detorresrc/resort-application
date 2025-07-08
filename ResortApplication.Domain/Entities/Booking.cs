using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResortApplication.Domain.Entities;

public class Booking
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }
    [ForeignKey("UserId")]
    public ApplicationUser User { get; set; }
    
    [Required]
    public int VillaId { get; set; }
    [ForeignKey("VillaId")]
    public required Villa Villa { get; set; }
    
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    
    [Required]
    public double TotalCost { get; set; }
    public int Nights { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    
    [Required]
    public DateOnly BookingDate { get; set; }
    [Required]
    public required DateOnly CheckInDate { get; set; }
    [Required]
    public DateOnly CheckOutDate { get; set; }
    
    public bool IsPaymentCompleted { get; set; } = false;
    public DateTime PaymentDate { get; set; }

    public string? StripeSessionId { get; set; }
    public string? StripePaymentIntentId { get; set; }
    
    public DateTime ActualCheckInDate { get; set; }
    public DateTime ActualCheckOutDate { get; set; }
    public int? VillaNumberId { get; set; }
    [ForeignKey("VillaNumberId")]
    public VillaNumber? VillaNumber { get; set; }
}