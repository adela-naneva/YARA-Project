namespace WarehouseManagement.API.Models.DTO
{
    public class ProductDto
    {
        public Guid ProductId { get; set; }
        public required string Name { get; set; }
        public int Hazardous { get; set; }
        public int UnitSize { get; set; }
    }
}
