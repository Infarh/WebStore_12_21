namespace WebStore.Domain.DTO;

public class CreateProductDTO
{
    public string Name { get; set; }
    public int Order { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public string Section { get; set; }
    public string? Brand { get; set; }

}