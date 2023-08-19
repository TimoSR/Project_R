namespace x_endpoints.DomainModels._Interfaces
{
    public interface IItems
    {
        string Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        decimal Price { get; set; }
    }
}
