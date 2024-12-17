using System.Data.SqlClient;

using Dapper;

using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string _connectionString;
    
        public CategoryRepository(string connectionString)
        {
        _connectionString = connectionString;
        }
    
        public async Task<Guid> AddCategoryAsync(Category category)
        {
        using var connection = new SqlConnection(_connectionString);
        var sql = @"INSERT INTO Categories (Id, Name)
                            VALUES (@Id, @Name)";
    
        await connection.ExecuteAsync(sql, category);
    
        return category.Id;
        }
    
        public Task<bool> DeleteCategoryAsync(Guid id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "DELETE FROM Categories WHERE Id = @Id";
            //delete the category
            connection.Execute(sql, new { Id = id });
            return Task.FromResult(true);
        }
    
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
        using var connection = new SqlConnection(_connectionString);
    
        return await connection.QueryAsync<Category>("SELECT * FROM Categories");
        }
    
        public async Task<Category> GetCategoryByIdAsync(Guid id)
        {
        using var connection = new SqlConnection(_connectionString);
    
        return await connection.QueryFirstAsync<Category>("SELECT * FROM Categories WHERE Id = @Id", new { Id = id });
        }
    
        public Task<Category?> UpdateCategoryAsync(Category category)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "UPDATE Categories SET Name = @Name WHERE Id = @Id";
            //update the category
            connection.Execute(sql, category);
            return Task.FromResult<Category?>(category);
        }
    }
}
