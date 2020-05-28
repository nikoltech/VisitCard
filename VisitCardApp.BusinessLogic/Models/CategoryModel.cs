namespace VisitCardApp.BusinessLogic.Models
{
    using VisitCardApp.DataAccess.Entities;
    using VisitCardApp.DataAccess.Enums;

    public class CategoryModel : IEntityModel<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public CategoryType Type { get; set; }

        public Category ToEntity()
        {
            return new Category
            {
                Id = this.Id,
                Name = this.Name,
                Type = this.Type
            };
        }

        public void ToModel(Category entity)
        {
            if (entity != null)
            {
                this.Id = entity.Id;
                this.Name = entity.Name;
                this.Type = entity.Type;
            }
        }
    }
}
