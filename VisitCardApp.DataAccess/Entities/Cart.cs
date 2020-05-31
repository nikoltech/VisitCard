namespace VisitCardApp.DataAccess.Entities
{
    using System.Collections.Generic;

    public class Cart
    {
        public int Id { get; set; }

        public List<CartLine> Items { get; set; }

        public bool IsPaid { get; set; }

        public string UserId { get; set; }

        public virtual AppUser User { get; set; }
    }
}
