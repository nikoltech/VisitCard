namespace VisitCardApp.DataAccess.Entities
{
    public class CartLine
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public int ProjectCaseId { get; set; }

        public ProjectCase ProjectCase { get; set; }
    }
}
