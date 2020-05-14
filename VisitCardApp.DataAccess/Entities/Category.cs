namespace VisitCardApp.DataAccess.Entities
{
    using VisitCardApp.DataAccess.Enums;

    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public CategoryType Type { get; set; }
    }
}
