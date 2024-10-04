namespace WarehouseManagement.API.Models.DTO
{
    public class CreateInventoryRequestDto
    {
        public required string WarehouseId { get; set; }
        public required string WarehouseName { get; set; }
        public required string ProductId { get; set; }
        public required string ProductName { get; set; }
        public int Hazardous { get; set; }
        public int UnitSize { get; set; }
        public int Amount { get; set; }
        public int Total { get; set; }
    }
}