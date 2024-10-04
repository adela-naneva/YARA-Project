namespace WarehouseManagement.API.Models.Domain
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public required string Name { get; set; }
        public int Hazardous { get; set; }
        public int UnitSize { get; set; }
    }
}
