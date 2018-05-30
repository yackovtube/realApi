using Entities;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class CategoriesDal
    {
        public IEnumerable<Category> GetAllReports()
        {
            using (var dbContext = new DashboardContext())
            {
                return dbContext.Categorys.ToList();
            }
        }
        public IEnumerable<Category> GetAllCategoriesIncludeReports()
        {
            using (var dbContext = new DashboardContext())
            {
                return dbContext.Categorys.Include("Reports").ToList();
            }
        }

        public Category GetCategoryById(int categoryId)
        {
            using (var dbContext = new DashboardContext())
            {
                return dbContext.Categorys.FirstOrDefault(c => c.CategoryId == categoryId);
            }
        }
    }
}
