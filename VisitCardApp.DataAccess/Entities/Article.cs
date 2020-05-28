﻿namespace VisitCardApp.DataAccess.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Article
    {
        public int Id { get; set; }

        public string Topic { get; set; }

        public string TextPath { get; set; }

        public List<ArticleImage> ArticleImages { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        [NotMapped]
        public string Text { get; set; }
    }
}
