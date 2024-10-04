namespace WarehouseManagement.API.Models.DTO
{
    public class CreateProductRequestDto
    {
        public required string Name { get; set; }
        public int Hazardous { get; set; }
        public int UnitSize { get; set; }
    }
}
