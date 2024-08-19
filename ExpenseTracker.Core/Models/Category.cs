using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.Models
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;

        public ICollection<Transaction> Transactions { get; set; }
        //what does this line mean?
        //This line means that a category can have multiple transactions.
        //how does it work?
        //It works by creating a collection of transactions in the category class.

        public ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();

    }
}
