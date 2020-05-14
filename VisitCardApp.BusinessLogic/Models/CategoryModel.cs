namespace VisitCardApp.DataAccess.Entities
{
    using VisitCardApp.BusinessLogic.Models;
    using VisitCardApp.DataAccess.Enums;

    public class CategoryModel : IEntityModel<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public CategoryType Type { get; set; }

        public Category ToEntity()
        {
            throw new System.NotImplementedException();
        }

        public void ToModel(Category entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
