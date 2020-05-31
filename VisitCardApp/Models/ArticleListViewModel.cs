namespace VisitCardApp.Models
{
    using System.Collections.Generic;
    using VisitCardApp.BusinessLogic.Models;
    using VisitCardApp.Helpers;

    public class ArticleListViewModel
    {
        public IEnumerable<ArticleModel> Articles { get; set; }

        public PaginationModel Pagination { get; set; }

        public int? CategoryId { get; set; }

    }
}
