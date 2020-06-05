using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitCardApp.Models
{
    public class AppSettings
    {
        public string Environment { get; set; }
        public string ApplicationId { get; set; }
        public string AccessToken { get; set; }
        public string LocationId { get; set; }
    }
}
