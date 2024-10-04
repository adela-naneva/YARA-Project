using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.API.Data;
using WarehouseManagement.API.Models.Domain;
using WarehouseManagement.API.Models.DTO;

namespace WarehouseManagement.API.Controllers
{
    // http://localhost:xxxx/api/warehouses
    [Route("api/warehouses")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public WarehousesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //TODO: add pagination, filtering and sorting
        // Get all warehouses
        [Route("getAllWarehouses")]
        [HttpGet]
        public async Task<ActionResult<Warehouse>> GetAllWarehouses()
        {
            IList<Warehouse> warehouses = dbContext.Warehouses.ToList();

            if (warehouses.Count == 0)
            {
                return NotFound();
            }

            return Ok(warehouses);
        }

        // Get a warehouse by id
        [Route("getWarehouseById")]
        [HttpGet]
        public async Task<ActionResult<Warehouse>> GetWarehouseById(Guid id)
        {
            var warehouse = await dbContext.Warehouses.FindAsync(id);

            if (warehouse == null)
            {
                return NotFound();
            }

            return warehouse;
        }

        // Create a new warehouse
        [Route("createWarehouse")]
        [HttpPost]
        public async Task<IActionResult> CreateWarehouse(CreateWarehouseRequestDto request)
        {
            // Map DTO to the Domain Model
            var warehouse = new Warehouse
            {
                Name = request.Name,
                MaxCapacity = request.MaxCapacity,
                // new WH is empty so current capacity is 0
                CurrentCapacity = 0,
                Country = request.Country,
                Zipcode = request.Zipcode,
                City = request.City,
                Address = request.Address,
            };

            await dbContext.Warehouses.AddAsync(warehouse);
            await dbContext.SaveChangesAsync();

            // Domain model to a DTO
            var response = new WarehouseDto
            {
                WarehouseId = warehouse.WarehouseId,
                Name = warehouse.Name,
                MaxCapacity = warehouse.MaxCapacity,
                // current capacity equals max capacity
                CurrentCapacity = warehouse.MaxCapacity,
                Country = warehouse.Country,
                Zipcode = warehouse.Zipcode,
                City = warehouse.City,
                Address = warehouse.Address,
            };

            return Ok(response);

            //TODO: Add error branches
        }

        [Route("updateWarehouseCapacity")]
        [HttpPut]
        public async Task<ActionResult<Warehouse>> updateWarehouseCapacity(
            string warehouseId,
            int newCapacity
        )
        {
            if (Guid.TryParse(warehouseId, out var newGuidWH))
            {
                var warehouse = await dbContext.Warehouses.FindAsync(newGuidWH);

                if (warehouse == null)
                {
                    return NotFound();
                }

                warehouse.CurrentCapacity = newCapacity;
                await dbContext.SaveChangesAsync();

                return warehouse;
            }
            else
            {
                return BadRequest(new { message = "This warehouseId is invalid id." });
            }
        }
    }
}
