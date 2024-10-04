namespace WarehouseManagement.API.Models.Domain
{
    public class Warehouse
    {
        public Guid WarehouseId { get; set; }
        public required string Name { get; set; }
        public required int MaxCapacity { get; set; }
        // CurrentCapacity is 0 when empty, equal to MaxCapacity if WH is full.
        public int CurrentCapacity { get; set; }

        // Location attributes
        public required string Country { get; set; }
        public string? Zipcode { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
    }
}
