using System;
using System.Collections.Generic;
using System.Text;

namespace VisitCardApp.DataAccess.Entities
{
    public class ArticleImage
    {
        public int Id { get; set; }

        public string ImagePath { get; set; }

        public string ImageMimeType { get; set; }

        public Article Article { get; set; }
    }
}
