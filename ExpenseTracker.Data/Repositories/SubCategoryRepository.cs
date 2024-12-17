using System.Data.SqlClient;

using Dapper;

using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Data.Repositories
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        private readonly string _connectionString;

        public SubCategoryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Guid> AddSubCategoryAsync(SubCategory subCategory)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO SubCategories (Id, Name, CategoryId)
                            VALUES (@Id, @Name, @CategoryId)";

            await connection.ExecuteAsync(sql, subCategory);

            return subCategory.Id;
        }

        public Task<bool> DeleteSubCategoryAsync(Guid id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "DELETE FROM SubCategories WHERE Id = @Id";
            //delete the subCategory
            connection.Execute(sql, new { Id = id });
            return Task.FromResult(true);
        }

        public async Task<IEnumerable<SubCategory>> GetAllSubCategoriesAsync()
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<SubCategory>("SELECT * FROM SubCategories");
        }

        public async Task<SubCategory> GetSubCategoryByIdAsync(Guid id)
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryFirstAsync<SubCategory>("SELECT * FROM SubCategories WHERE Id = @Id", new { Id = id });
        }

        public Task<SubCategory?> UpdateSubCategoryAsync(SubCategory subCategory)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "UPDATE SubCategories SET Name = @Name, CategoryId = @CategoryId WHERE Id = @Id";
            //update the subCategory
            connection.Execute(sql, subCategory);
            return Task.FromResult<SubCategory?>(subCategory);
        }
    }
}
