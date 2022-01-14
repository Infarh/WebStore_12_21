using System.ComponentModel.DataAnnotations;

namespace WebStore.ViewModels;

public class OrderViewModel
{
    [Required]
    public string Address { get; set; } = null!;

    [Required]
    [DataType(DataType.PhoneNumber)]
    public string Phone { get; set; } = null!;

    [MaxLength(200)]
    public string? Description { get; set; }
}