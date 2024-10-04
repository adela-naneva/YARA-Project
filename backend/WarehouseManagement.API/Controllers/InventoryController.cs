using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.API.Data;
using WarehouseManagement.API.Models.Domain;
using WarehouseManagement.API.Models.DTO;

namespace WarehouseManagement.API.Controllers
{
    // http://localhost:xxxx/api/inventory
    [Route("api/inventory")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public InventoryController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //TODO: add pagination and filtering&sorting
        // Get current WH inventory
        [Route("getInventory")]
        [HttpGet]
        public async Task<ActionResult<Inventory>> getInventory(string warehouseId)
        {
            IList<Inventory> currentInventory = dbContext
                .Inventory.Where(r => r.WarehouseId.Contains(warehouseId))
                .ToList();

            if (currentInventory.Count == 0)
            {
                return NotFound();
            }

            // we basically need all the products in the WH.
            IList<Product> products = [];
            foreach (var record in currentInventory)
            {
                Guid.TryParse(record.ProductId, out var newGuidPr);
                var p = await dbContext.Products.FindAsync(newGuidPr);
                products.Add(p);
            }

            if (products.Count == 0)
            {
                return NotFound();
            }

            //TODO: make a response object to hold total count of results
            return Ok(products);
        }

        // Get all WHs containing a product
        [Route("getWarehousesWithProduct")]
        [HttpGet]
        public async Task<ActionResult<WarehouseProduct>> getWHwithProduct(
            string productId,
            string warehouseId
        )
        {
            IList<Inventory> currentInventory = dbContext
                .Inventory.Where(r =>
                    r.ProductId.Contains(productId) && !r.WarehouseId.Contains(warehouseId)
                )
                .ToList();

            if (currentInventory.Count == 0)
            {
                return NotFound();
            }

            var result = new List<WarehouseProduct>();

            foreach (Inventory record in currentInventory)
            {
                Guid.TryParse(record.WarehouseId, out var newGuidWh);
                var w = await dbContext.Warehouses.FindAsync(newGuidWh);
                if (w != null)
                {
                    var r = new WarehouseProduct { warehouse = w, Amount = record.Amount };
                    result.Add(r);
                }
            }

            if (result.Count == 0)
            {
                return NotFound();
            }

            //TODO: make a response object to hold total count of results
            return Ok(result);
        }

        // Add product to WH inventory
        [Route("addProductToInventory")]
        [HttpPost]
        public async Task<ActionResult<Inventory>> addProductToInventory(
            string warehouseId,
            string productId,
            int amount
        )
        {
            // If the product is not present in WH - add it;
            // If the product is already existing - update its amount and total;
            // Check if product matches the hazardousness of the other products;
            // If product doesn't match hazardousness - return error;

            if (Guid.TryParse(warehouseId, out var newGuidWH))
            {
                //that's good
            }
            else
            {
                Console.WriteLine($"Unable to convert {warehouseId} to a Guid");
            }

            if (Guid.TryParse(productId, out var newGuidPr))
            {
                // that's good too
            }
            else
            {
                Console.WriteLine($"Unable to convert {productId} to a Guid");
            }

            // hazardousness is actually a real word: https://www.oed.com/dictionary/hazardousness_n

            var inventoryRecord = dbContext
                .Inventory.Where(r =>
                    r.ProductId.Contains(productId) && r.WarehouseId.Contains(warehouseId)
                )
                .ToList();

            var product = await dbContext.Products.FindAsync(newGuidPr);
            var warehouse = await dbContext.Warehouses.FindAsync(newGuidWH);

            // Check if inventory has enough space


            if (inventoryRecord.Count() == 0)
            {
                // Product is missing. Maybe WH is empty?
                var anyStock = dbContext
                    .Inventory.Where(r => r.WarehouseId.Contains(warehouseId))
                    .ToList();

                if (anyStock.Count() > 0)
                {
                    // Product is just brand new for the WH so check it's hazardousness
                    var hazardousnessMatch = dbContext
                        .Inventory.Where(r => r.Hazardous == product.Hazardous)
                        .ToList();

                    if (hazardousnessMatch.Count() > 0)
                    {
                        // Products in the WH are matching the new Product's hazardous flag. Go ahead and add them!

                        // Map DTO to the Domain Model
                        var newRecord = new Inventory
                        {
                            WarehouseId = warehouse.WarehouseId.ToString(),
                            WarehouseName = warehouse.Name,
                            ProductId = product.ProductId.ToString(),
                            ProductName = product.Name,
                            Hazardous = product.Hazardous,
                            UnitSize = product.UnitSize,
                            Amount = amount,
                            Total = amount * product.UnitSize,
                        };

                        await dbContext.Inventory.AddAsync(newRecord);
                        await dbContext.SaveChangesAsync();

                        // Domain model to a DTO
                        var response = new InventoryDto
                        {
                            InventoryId = newRecord.InventoryId,
                            WarehouseId = newRecord.WarehouseId,
                            WarehouseName = newRecord.WarehouseName,
                            ProductId = newRecord.ProductId,
                            ProductName = newRecord.ProductName,
                            Hazardous = newRecord.Hazardous,
                            UnitSize = newRecord.UnitSize,
                            Amount = newRecord.Amount,
                            Total = newRecord.Total,
                        };

                        return Ok(response);
                    }
                    else
                    {
                        // Product is not matching the others. Return error
                        return BadRequest(
                            new { message = "This product should not be added to this warehouse." }
                        );
                    }
                }
                else
                {
                    // Our WH is currently empty. Add the product :)
                    var newRecord = new Inventory
                    {
                        WarehouseId = warehouse.WarehouseId.ToString(),
                        WarehouseName = warehouse.Name,
                        ProductId = product.ProductId.ToString(),
                        ProductName = product.Name,
                        Hazardous = product.Hazardous,
                        UnitSize = product.UnitSize,
                        Amount = amount,
                        Total = amount * product.UnitSize,
                    };

                    await dbContext.Inventory.AddAsync(newRecord);
                    await dbContext.SaveChangesAsync();

                    // Domain model to a DTO
                    var response = new InventoryDto
                    {
                        InventoryId = newRecord.InventoryId,
                        WarehouseId = newRecord.WarehouseId,
                        WarehouseName = newRecord.WarehouseName,
                        ProductId = newRecord.ProductId,
                        ProductName = newRecord.ProductName,
                        Hazardous = newRecord.Hazardous,
                        UnitSize = newRecord.UnitSize,
                        Amount = newRecord.Amount,
                        Total = newRecord.Total,
                    };

                    return Ok(response);
                }
            }
            else
            {
                // the product is present in the warehouse and inventoryRecord is not empty
                foreach (var record in inventoryRecord)
                {
                    var rec = inventoryRecord[0];
                    rec.Amount = rec.Amount + amount;
                    rec.Total = rec.Total + rec.UnitSize * amount;
                }

                await dbContext.SaveChangesAsync();

                return Ok(inventoryRecord);
            }
        }

        // update stock
        // delete stock
    }
}
