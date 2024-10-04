using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.API.Data;
using WarehouseManagement.API.Models.Domain;
using WarehouseManagement.API.Models.DTO;

namespace WarehouseManagement.API.Controllers
{
    // http://localhost:xxxx/api/transactions
    [Route("api/transactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public TransactionsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //TODO: add pagination and filtering&sorting
        // Get all transactions for a WH
        [Route("getTransactionHistory")]
        [HttpGet]
        public async Task<ActionResult<Transaction>> GetTransactionHistory(string CurrentWHId)
        {
            var transactions = dbContext
                .Transactions.Where(b =>
                    b.SourceWarehouseId.Contains(CurrentWHId)
                    || b.TargetWarehouseId.Contains(CurrentWHId)
                )
                .ToList();

            if (transactions.Count == 0)
            {
                return NotFound();
            }

            //TODO: make a response object to hold total count of results
            return Ok(transactions);
        }

        // Create a new transaction
        [Route("createTransaction")]
        [HttpPost]
        public async Task<IActionResult> CreateTransaction(CreateTransactionDto request)
        {
            var convertedDateSuccess = DateTime.TryParse(request.CreationDate.ToString(), out var createdDate);
            if (convertedDateSuccess)
            {
                // Map DTO to the Domain Model
                var transaction = new Transaction
                {
                    CreationDate = createdDate,
                    ProductId = request.ProductId,
                    ProductName = request.ProductName,
                    Amount = request.Amount,
                    SourceWarehouseId = request.SourceWarehouseId,
                    TargetWarehouseId = request.TargetWarehouseId,
                };

                await dbContext.Transactions.AddAsync(transaction);
                await dbContext.SaveChangesAsync();

                // Domain model to a DTO
                var response = new TransactionDto
                {
                    TransactionId = transaction.TransactionId,
                    CreationDate = transaction.CreationDate,
                    ProductId = transaction.ProductId,
                    ProductName = transaction.ProductName,
                    Amount = transaction.Amount,
                    SourceWarehouseId = transaction.SourceWarehouseId,
                    TargetWarehouseId = transaction.TargetWarehouseId,
                };

                return Ok(response);
            }
            else
            {
                return BadRequest(new { message = "This creationDate is invalid date." });
            }
        }
    }
}
