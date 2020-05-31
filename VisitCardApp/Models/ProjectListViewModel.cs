namespace VisitCardApp.Models
{
    using System.Collections.Generic;
    using VisitCardApp.BusinessLogic.Models;
    using VisitCardApp.Helpers;

    public class ProjectListViewModel
    {
        public IEnumerable<ProjectCaseModel> ProjectCases { get; set; }

        public PaginationModel Pagination { get; set; }

        public int? CategoryId { get; set; }
    }
}
