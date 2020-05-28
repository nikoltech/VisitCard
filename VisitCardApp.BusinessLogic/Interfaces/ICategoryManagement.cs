namespace VisitCardApp.BusinessLogic.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VisitCardApp.BusinessLogic.Models;
    using VisitCardApp.DataAccess.Enums;

    public interface ICategoryManagement
    {
        Task<List<CategoryModel>> GetCategoryListAsync(CategoryType type = CategoryType.All);

        Task<CategoryModel> CreateCategoryAsync(CategoryModel category);

        Task<CategoryModel> UpdateCategoryAsync(CategoryModel category);

        Task<bool> RemoveCategoryAsync(CategoryModel category);
    }
}
