using WarehouseManagement.API.Models.Domain;

namespace WarehouseManagement.API.Models.DTO
{
    public class TransactionDto
    {
        public Guid TransactionId { get; set; }
        public required DateTime CreationDate { get; set; }
        public required string ProductId { get; set; }

        public required string ProductName { get; set; }
        public required int Amount { get; set; }
        public string? SourceWarehouseId { get; set; }
        public string? SourceWarehouseName { get; set; }
        public string? TargetWarehouseId { get; set; }
        public string? TargetWarehouseName { get; set; }

        public Warehouse? SourceWarehouse { get; set; }
        public Warehouse? TargetWarehouse { get; set; }
    }
}
