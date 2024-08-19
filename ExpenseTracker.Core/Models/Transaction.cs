namespace ExpenseTracker.Core.Models
{
    public enum TransactionTypeEnum
    {
        Income = 1,
        Expense = 2
    }
    public class Transaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public bool IsRecurrent { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;


        public Transaction()
        {
            Id = Guid.NewGuid();
        }
    }
}
