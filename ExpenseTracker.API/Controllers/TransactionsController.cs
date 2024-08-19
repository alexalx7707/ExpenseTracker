using ExpenseTracker.API.DTOs;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.Services;

using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionService _transactionService;
        private readonly ILogger<TransactionsController> _logger; //added logger

        public TransactionsController(TransactionService transactionService, ILogger<TransactionsController> logger) //added logger
        {
            _transactionService = transactionService;
            _logger = logger; //added logger
        }
            // GET: api/transactions/list-all
            [HttpGet]
        [Route("list-all")]
        public async Task<ActionResult<List<Transaction>>> GetAllTransactions()
        {
            return (await _transactionService.GetAllTransactionsAsync()).ToList();
        }

            // GET: api/transactions/get-by-id/{id}
            [HttpGet]
        [Route("get-by-id/{id:guid}")]
        public async Task<ActionResult<Transaction>> GetTransactionById(Guid id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

            // POST: api/transactions/create
            [HttpPost]
        [Route("create")]
        public async Task<ActionResult<Transaction>> AddTransaction(TransactionDTO transactionModel)
        {
            if (transactionModel == null)
            {
                return BadRequest();
            }

            if (transactionModel.Amount <= 0)
            {
                return BadRequest("Amount cannot be less or equal to 0");
            }

            if (string.IsNullOrEmpty(transactionModel.Category.Name))
            {
                transactionModel.Category.Name = "Diverse";
            }

            if (transactionModel.Date <= DateTime.MinValue)
            {
                return BadRequest($"Date cannot be lower than ${DateTime.MinValue.Date.ToShortDateString()}");
            }

            Guid result;

            try
            {
                result = await _transactionService.AddTransactionAsync(new Transaction()
                {
                    Amount = transactionModel.Amount,
                    Category = transactionModel.Category,
                    Date = transactionModel.Date,
                    Description = transactionModel.Description,
                    IsRecurrent = transactionModel.IsRecurrent,
                    //TransactionType = transactionModel.TransactionType,
                    TransactionType = (TransactionTypeEnum)transactionModel.TransactionType,
                });
            }
            catch (Exception ex)
            {
                // @TODO - LOG THE ERROR
                //DONE as zice eu
                _logger.LogError(ex, "An error occurred while adding a transaction.");
                return BadRequest("Could not register the transaction");
            }

            return Ok(result);
        }

            // PUT: api/transactions/update/{id}
            [HttpPut]
        [Route("update/{id:guid}")]
        public async Task<ActionResult<Transaction>> UpdateTransaction(Guid id, TransactionDTO transactionModel)
        {
            if (transactionModel == null)
            {
                return BadRequest();
            }

            if (transactionModel.Amount <= 0)
            {
                return BadRequest("Amount cannot be less or equal to 0");
            }

            if (string.IsNullOrEmpty(transactionModel.Category.Name))
            {
                transactionModel.Category.Name = "Diverse";
            }

            if (transactionModel.Date <= DateTime.MinValue)
            {
                return BadRequest($"Date cannot be lower than ${DateTime.MinValue.Date.ToShortDateString()}");
            }

            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            transaction.Amount = transactionModel.Amount;
            transaction.Category = transactionModel.Category;
            transaction.Date = transactionModel.Date;
            transaction.Description = transactionModel.Description;
            transaction.IsRecurrent = transactionModel.IsRecurrent;
            transaction.TransactionType = (TransactionTypeEnum)transactionModel.TransactionType;

            try
            {
                var result = await _transactionService.UpdateTransactionAsync(transaction);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // @TODO - LOG THE ERROR
                _logger.LogError(ex, "An error occurred while updating a transaction.");
                return BadRequest("Could not update the transaction");
            }
        }

            // DELETE: api/transactions/delete/{id}
            [HttpDelete]
        [Route("delete/{id:guid}")]
        public async Task<ActionResult> DeleteTransaction(Guid id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            try
            {
                var result = await _transactionService.DeleteTransactionAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // @TODO - LOG THE ERROR
                _logger.LogError(ex, "An error occurred while deleting a transaction.");
                return BadRequest("Could not delete the transaction");
            }
        }

            // GET: api/transactions/type/{transactionType}
            [HttpGet]
        [Route("type/{transactionType:int}")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsByType(int transactionType)
        {
            if (!Enum.IsDefined(typeof(TransactionTypeEnum), transactionType))
            {
                return BadRequest("Invalid transaction type.");
            }

            var transactions = await _transactionService.GetTransactionsByTypeAsync(transactionType);
            return Ok(transactions);
        }
    }
}
