namespace x_App.DataTransferObjects;

public class ProductDto
{
    //We Use DTO's to remove Data We don't want to share with the client
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
}