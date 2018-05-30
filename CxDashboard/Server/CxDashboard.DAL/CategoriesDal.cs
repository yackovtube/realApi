using CxDashboard.Entities;
using System.Collections.Generic;
using System.Linq;

namespace CxDashboard.DAL
{
    public class CategoriesDal
    {
        public IEnumerable<Category> GetAllCategories()
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

        public int GetCategoryIdByName(string categoryName)
        {
            using (var dbContext = new DashboardContext())
            {
                var category = dbContext.Categorys.FirstOrDefault(c => c.CategoryName == categoryName);
                if (category != null)
                {
                    return category.CategoryId;
                }

                return 0;
            }
        }
    }
}
