using System.ComponentModel.DataAnnotations;

namespace ResortApplication.Domain.Entities;

public class Villa
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Villa Name is required")]
    [MaxLength(50, ErrorMessage = "Villa Name cannot exceed 50 characters")]
    public required string Name { get; set; }

    public string? Description { get; set; }

    [Display(Name = "Price per Night")]
    [Range(10, 10000, ErrorMessage = "Price must be between 10 and 10000")]
    public double Price { get; set; }

    public int Sqft { get; set; }

    [Range(1, 10, ErrorMessage = "Occupancy must be between 1 and 10")]
    public int Occupancy { get; set; }

    [Display(Name = "Image Url")] public string? ImageUrl { get; set; }

    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}