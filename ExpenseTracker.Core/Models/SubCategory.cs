using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.Models
{
    public class SubCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Foreign key to Category
        public Guid ParentCategoryId { get; set; }
        
    }
}
