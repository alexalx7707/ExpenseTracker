using System.Data.SqlClient;

using Dapper;

using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Data.Repositories
{
    public class TransactionsRespository : ITransactionRepository
    {
        private readonly string _connectionString;
        private string TableName => "[Transactions]";

        public TransactionsRespository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Guid> AddTransactionAsync(Transaction transaction)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = $@"INSERT INTO {TableName} 
                              (Id, Description, Amount, Date, Category, IsRecurrent, TransactionType, SubCategory, UserId)
                          VALUES (@Id, @Description,@Amount, @Date, @Category, @IsRecurrent, @TransactionType, @SubCategory, @UserId)";

            await connection.ExecuteAsync(query, transaction);

            return transaction.Id;
        }

        public async Task<bool> DeleteTransactionAsync(Guid transactionId)
        {
            using var conn = new SqlConnection(_connectionString);

            var query = $"DELETE FROM {TableName} WHERE Id = @Id; SELECT @ROWCOUNT AS Affected";

            var affectedRows = await conn.ExecuteScalarAsync<int>(query, new { Id = transactionId });

            return affectedRows == 1;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            using var conn = new SqlConnection(_connectionString);

            var query = $"SELECT * FROM {TableName}";

            return await conn.QueryAsync<Transaction>(query);
        }

        public async Task<Transaction> GetTransactionByIdAsync(Guid transactionId)
        {
            using var conn = new SqlConnection(_connectionString);

            var query = $"SELECT * FROM {TableName} WHERE Id = @Id";

            return await conn.QuerySingleAsync<Transaction>(query, new { Id = transactionId });
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByTypeAsync(int transactionType)
        {
            using var conn = new SqlConnection(_connectionString);

            var query = $"SELECT * FROM {TableName} WHERE TransactionType = @TransactionType";

            return await conn.QueryAsync<Transaction>(query, new { TransactionType = transactionType });
        }

        public async Task<Transaction?> UpdateTransactionAsync(Transaction transaction)
        {
            using var conn = new SqlConnection(_connectionString);

            var query = $@"UPDATE {TableName}
                              SET (
                                  Description = @Description,
                                  Amount = @Amount,
                                  Date = @Date,
                                  Category = @Category,
                                  IsRecurrent = @IsRecurrent,
                                  TransactionType = @TransactionType
                                  SubCategory = @SubCategory
                                  UserId = @UserId
                              )
                          WHERE Id = @Id";

            var result = await conn.ExecuteAsync(query, transaction);

            if (result == 0)
            {
                return null;
            }

            return transaction;
        }


        public async Task<IEnumerable<Transaction>> GetMonthlyReportAsync(int year, int month)
        {
            using var connection = new SqlConnection(_connectionString);

            string sql = @"SELECT * FROM Transactions 
                       WHERE YEAR(Date) = @Year AND MONTH(Date) = @Month";

            return await connection.QueryAsync<Transaction>(sql, new { Year = year, Month = month }); //what does this line do?
            // This line executes the query and returns the result as a list of Transaction objects. The query is parameterized with the year and month values, which are passed as an anonymous object.
        }
    }
}
