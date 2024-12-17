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
                    SubCategory = transactionModel.SubCategory,
                    Date = transactionModel.Date,
                    Description = transactionModel.Description,
                    IsRecurrent = transactionModel.IsRecurrent,
                    //TransactionType = transactionModel.TransactionType,
                    TransactionType = (TransactionTypeEnum)transactionModel.TransactionType,
                    UserId = transactionModel.UserId
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
            transaction.SubCategory = transactionModel.SubCategory;
            transaction.Date = transactionModel.Date;
            transaction.Description = transactionModel.Description;
            transaction.IsRecurrent = transactionModel.IsRecurrent;
            transaction.TransactionType = (TransactionTypeEnum)transactionModel.TransactionType;
            transaction.UserId = transactionModel.UserId;

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

        // GET: api/transactions/monthly-report/{month}/{year}
        [HttpGet]
        [Route("monthly-report/{month:int}/{year:int}")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetMonthlyReport(int month, int year)
        {
            if (month < 1 || month > 12)
            {
                return BadRequest("Invalid month.");
            }

            if (year < 1900 || year > 2100)
            {
                return BadRequest("Invalid year.");
            }

            var transactions = await _transactionService.GetMonthlyReportAsync(month, year);
            var totalSpent = transactions.Where(t => t.TransactionType == TransactionTypeEnum.Expense).Sum(t => t.Amount);
            //it filters the transactions that are expenses and sums the amount of each transaction
            var totalIncome = transactions.Where(t => t.TransactionType == TransactionTypeEnum.Income).Sum(t => t.Amount);
            var total = totalIncome - totalSpent;
            var result = new
            {
                TotalIncome = totalIncome,
                TotalSpent = totalSpent,
                Total = total,
                Transactions = transactions
            };
            return Ok(result);
        }
    }
}
