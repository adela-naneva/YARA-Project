using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseManagement.API.Models.Domain
{
    public class Transaction
    {
        public Guid TransactionId { get; set; }
        public required DateTime CreationDate { get; set; }

        [ForeignKey("ProductId")]
        public required string ProductId { get; set; }

        public required string ProductName { get; set; }
        public required int Amount { get; set; }

        // Transaction foes FROM Source WH TO Target WH
        // If current WH is SourceWH - transaction is export
        // If current WH is TargetHS - transaction is import
        // If SourceWH is null - it is a direct/inital import
        // If TargetWH is null - it is basically a deletion
        public string? SourceWarehouseId { get; set; }
        public string? TargetWarehouseId { get; set; }

        public string? SourceWarehouseName { get; set; }
        public string? TargetWarehouseName { get; set; }

        [NotMapped]
        public Warehouse? SourceWarehouse { get; set; }
        [NotMapped]
        public Warehouse? TargetWarehouse { get; set; }
    }
}
