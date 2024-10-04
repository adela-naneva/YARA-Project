using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.API.Data;
using WarehouseManagement.API.Models.Domain;
using WarehouseManagement.API.Models.DTO;

namespace WarehouseManagement.API.Controllers
{
    // http://localhost:xxxx/api/products
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public ProductsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //TODO: add pagination and filtering&sorting
        // Get all products
        [Route("getAllProducts")]
        [HttpGet]
        public async Task<ActionResult<Product>> GetAllProducts()
        {
            IList<Product> products = dbContext.Products.ToList();


            if (products.Count == 0)
            {
                return NotFound();
            }

            //TODO: make a response object to hold total count of results
            return Ok(products);
        }

        // Get a product by id
        [Route("getProductById")]
        [HttpGet]
        public async Task<ActionResult<Product>> GetProductById(Guid id)
        {
            var product = await dbContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }


        // Create a new product
        [Route("createProduct")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductRequestDto request)
        {
            // Transform strings
            int size = 0;
            try
            {
                size = int.Parse(request.UnitSize.ToString());
            }
            catch (FormatException)
            {
                Console.WriteLine($"Unable to parse the unit size");
            }

            // Map DTO to the Domain Model
            var product = new Product
            {
                Name = request.Name,
                Hazardous = request.Hazardous,
                UnitSize = size
            };

            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();

            // Domain model to a DTO
            var response = new ProductDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Hazardous = product.Hazardous,
                UnitSize = product.UnitSize
            };

            return Ok(response);

            //TODO: Add error branches
        }
    }
}