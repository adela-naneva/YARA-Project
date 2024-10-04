namespace WarehouseManagement.API.Models.DTO
{
    public class WarehouseDto
    {
        public Guid WarehouseId { get; set; }
        public required string Name { get; set; }
        public required int MaxCapacity { get; set; }
        public int CurrentCapacity { get; set; }
        public required string Country { get; set; }
        public string? Zipcode { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
    }
}
