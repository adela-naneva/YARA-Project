namespace WarehouseManagement.API.Models.DTO
{
    public class CreateTransactionDto
    {
        public required string CreationDate { get; set; }
        public required string ProductId { get; set; }

        public required string ProductName { get; set; }
        public required int Amount { get; set; }
        public string? SourceWarehouseId { get; set; }
        public string? SourceWarehouseName { get; set; }
        public string? TargetWarehouseId { get; set; }
        public string? TargetWarehouseName { get; set; }
    }
}
