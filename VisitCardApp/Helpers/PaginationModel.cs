using System;

namespace VisitCardApp.Helpers
{
    public class PaginationModel
    {
        public long TotalItems { get; set; }

        public int AmountPerPage { get; set; }

        public int Page { get; set; }

        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling((double)TotalItems / AmountPerPage);
            }
        }
    }
}
