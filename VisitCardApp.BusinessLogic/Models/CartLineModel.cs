namespace VisitCardApp.BusinessLogic.Models
{
    using VisitCardApp.DataAccess.Entities;

    public class CartLineModel
    {
        public ProjectCaseModel ProjectCase { get; set; }

        public int Quantity { get; set; }
    }
}
