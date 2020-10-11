namespace VisitCardApp.BusinessLogic.Test.Models
{
    using System.Collections.Generic;
    using VisitCardApp.BusinessLogic.Models;
    using VisitCardApp.DataAccess.Entities;
    using Xunit;

    public class ModelsFixture
    {
        #region ArticleImageModel
        [Fact]
        public void ArticleImageModelTestToEntity()
        {
            // Preparing
            ArticleModel articleModel = new ArticleModel { Id = 443 };

            ArticleImageModel model = new ArticleImageModel
            {
                File = new byte[] { 1, 0, 0, 0, 1, 1, 1 },
                FileName = "FileNamedsgh",
                Id = 47,
                ImageMimeType = "ImageMimeTypesdhhddhdg",
                UrlPath = "http://UrlPath/asgldn.skg",
                Article = articleModel,
                ArticleId = articleModel.Id
            };

            // RUN
            ArticleImage entity = model.ToEntity();

            // Validate result
            Assert.NotNull(entity);
            Assert.Equal(entity.File.Length, model.File.Length);
            Assert.Equal(entity.Id, model.Id);
            Assert.Equal(entity.ImageMimeType, model.ImageMimeType);
            Assert.Equal(entity.UrlPath, model.UrlPath);
            Assert.Equal(entity.Article?.Id, model.Article?.Id);
        }

        [Fact]
        public void ArticleImageModelTest1Ctor()
        {
            // Input values
            ArticleImage entity = new ArticleImage
            {
                File = new byte[] { 1, 0, 0, 0, 1, 1, 1 },
                FileName = "FileNamedsgh",
                Id = 47,
                ImageMimeType = "ImageMimeTypesdhhddhdg",
                UrlPath = "http://UrlPath/asgldn.skg"
            };

            // RUN
            ArticleImageModel model = new ArticleImageModel(entity);

            // Validate result
            Assert.Equal(entity.Id, model.Id);
            Assert.Equal(entity.ImageMimeType, model.ImageMimeType);
            Assert.Equal(entity.UrlPath, model.UrlPath);
        }

        [Fact]
        public void ArticleImageModelTest2ToModel()
        {
            // Input values
            ArticleImage entity = new ArticleImage
            {
                File = new byte[] { 1, 0, 0, 0, 1, 1, 1 },
                FileName = "FileNamedsgh",
                Id = 47,
                ImageMimeType = "ImageMimeTypesdhhddhdg",
                UrlPath = "http://UrlPath/asgldn.skg"
            };

            // RUN
            ArticleImageModel model = new ArticleImageModel();
            model.ToModel(entity);

            // Validate result
            Assert.Equal(model.Id, entity.Id);
            Assert.Equal(model.ImageMimeType, entity.ImageMimeType);
            Assert.Equal(model.UrlPath, entity.UrlPath);
        } 
        #endregion

        #region ArticleModel
        [Fact]
        public void ArticleModelTestToEntity()
        {
            // Preparing
            List<ArticleImageModel> articleImageModels = new List<ArticleImageModel>
            {
                new ArticleImageModel(),
                new ArticleImageModel(),
                new ArticleImageModel(),
            };

            List<CommentModel> commentModels = new List<CommentModel>
            {
                new CommentModel(),
                new CommentModel(),
                new CommentModel(),
                new CommentModel()
            };

            ArticleModel model = new ArticleModel
            {
                Id = 23,
                CategoryId = 1,
                Text = "Textasfg",
                Topic = "topicsfafg",
                UserId = "UserIdskdgfhfd",

                ArticleImages = articleImageModels,
                Comments = commentModels
            };

            // RUN
            Article entity = model.ToEntity();

            // Validate result
            Assert.NotNull(entity);
            Assert.Equal(model.Id, entity.Id);
            Assert.Equal(model.CategoryId, entity.CategoryId);
            Assert.Equal(model.Text, entity.Text);
            Assert.Equal(model.Topic, entity.Topic);
            Assert.Equal(model.UserId, entity.UserId);
            Assert.Equal(model.ArticleImages.Count, entity.ArticleImages.Count);
            Assert.Equal(model.Comments.Count, entity.Comments.Count);
        }

        [Fact]
        public void ArticleModelTest2ToModel()
        {
            // Input values
            List<ArticleImage> articleImages = new List<ArticleImage>
            {
                new ArticleImage(),
                new ArticleImage(),
                new ArticleImage(),
                new ArticleImage()
            };

            List<Comment> comments = new List<Comment>
            {
                new Comment(),
                new Comment(),
                new Comment()
            };

            Article entity = new Article
            {
                Id = 23,
                CategoryId = 1,
                Text = "Textasfg",
                Topic = "topicsfafg",
                UserId = "UserIdskdgfhfd",

                ArticleImages = articleImages,
                Comments = comments
            };

            // RUN
            ArticleModel model = new ArticleModel();
            model.ToModel(entity);

            // Validate result
            Assert.Equal(entity.Id, model.Id);
            Assert.Equal(entity.CategoryId, model.CategoryId);
            Assert.Equal(entity.Text, model.Text);
            Assert.Equal(entity.Topic, model.Topic);
            Assert.Equal(entity.UserId, model.UserId);
            Assert.Equal(entity.ArticleImages.Count, model.ArticleImages.Count);
            Assert.Equal(entity.Comments.Count, model.Comments.Count);
        }
        #endregion

        #region CartLineModel
        [Fact]
        public void CartLineModelTestCtor()
        {
            // RUN
            CartLineModel model = new CartLineModel { Quantity = 5443, ProjectCase = new ProjectCaseModel() };

            Assert.Equal(5443, model.Quantity);
            Assert.NotNull(model.ProjectCase);
        }
        #endregion

        #region CartModel
        [Fact]
        public void CartModelTestCtor()
        {
            // RUN
            CartModel model = new CartModel();

            // Validate result
            Assert.NotNull(model.Items);
        }
        #endregion

        #region CategoryModel
        [Fact]
        public void CategoryModelTestToEntity()
        {
            // Preparing
            CategoryModel model = new CategoryModel
            {
                Id = 4545,
                Name = "CategoryModeldsgdgs",
                Type = DataAccess.Enums.CategoryType.Article
            };

            // RUN
            Category entity = model.ToEntity();

            // Validate result
            Assert.NotNull(entity);
            Assert.Equal(model.Id, entity.Id);
            Assert.Equal(model.Name, entity.Name);
            Assert.Equal(model.Type, entity.Type);
        }

        [Fact]
        public void CategoryModelTest2ToModel()
        {
            // Input values
            Category entity = new Category
            {
                Id = 4545,
                Name = "CategoryModeldsgdgs",
                Type = DataAccess.Enums.CategoryType.Article
            };

            // RUN
            CategoryModel model = new CategoryModel();
            model.ToModel(entity);

            // Validate result
            Assert.Equal(entity.Id, model.Id);
            Assert.Equal(entity.Name, model.Name);
            Assert.Equal(entity.Type, model.Type);
        }
        #endregion

        #region CommentModel
        [Fact]
        public void CommentModelTestToEntity()
        {
            // Preparing
            CommentModel model = new CommentModel
            {
                ArticleId = 434,
                Id = 4364,
                Text = "DSGSFG",
                UserId = "sagfdga",
                UserName = "namesfkskhdbg"
            };

            // RUN
            Comment entity = model.ToEntity();

            // Validate result
            Assert.NotNull(entity);
            Assert.Equal(model.Id, entity.Id);
            Assert.Equal(model.Text, entity.Text);
            Assert.Equal(model.UserId, entity.UserId);
            Assert.Equal(model.ArticleId, entity.ArticleId);
            Assert.Equal(model.UserName, entity.UserName);
        }

        [Fact]
        public void CommentModelTest2Ctor()
        {
            // Input values
            Comment entity = new Comment
            {
                ArticleId = 434,
                Id = 4364,
                Text = "DSGSFG",
                UserId = "sagfdga",
                UserName = "namesfkskhdbg"
            };

            // RUN
            CommentModel model = new CommentModel(entity);

            // Validate result
            Assert.Equal(entity.Id, model.Id);
            Assert.Equal(entity.Text, model.Text);
            Assert.Equal(entity.UserId, model.UserId);
            Assert.Equal(entity.ArticleId, model.ArticleId);
            Assert.Equal(entity.UserName, model.UserName);
        }

        [Fact]
        public void CommentModelTest2ToModel()
        {
            // Input values
            Comment entity = new Comment
            {
                ArticleId = 434,
                Id = 4364,
                Text = "DSGSFG",
                UserId = "sagfdga",
                UserName = "namesfkskhdbg"
            };

            // RUN
            CommentModel model = new CommentModel();
            model.ToModel(entity);

            // Validate result
            Assert.Equal(entity.Id, model.Id);
            Assert.Equal(entity.Text, model.Text);
            Assert.Equal(entity.UserId, model.UserId);
            Assert.Equal(entity.ArticleId, model.ArticleId);
            Assert.Equal(entity.UserName, model.UserName);
        }
        #endregion

        #region ProjectCaseModel
        [Fact]
        public void ProjectCaseModelTestToEntity()
        {
            // Preparing
            ProjectCaseModel model = new ProjectCaseModel
            {
                Id = 23,
                CategoryId = 1,
                Cost = 2,
                Description = "dsgdsd",
                DescriptionPath = "sagdsg",
                Image = new byte[] {1,0,0,1,1,0,1},
                ImageFileName = "dsgdsgds",
                ImageMimeType = "sajhbgf",
                ImagePath = "dskjjkf",
                ProjectName = "skjnv fn v",
                UrlPath = "dsfkjbkggggg"
            };

            // RUN
            ProjectCase entity = model.ToEntity();

            // Validate result
            Assert.NotNull(entity);
            Assert.Equal(model.Id, entity.Id);
            Assert.Equal(model.CategoryId, entity.CategoryId);
            Assert.Equal(model.Cost, entity.Cost);
            Assert.Equal(model.Description, entity.Description);
            Assert.Equal(model.DescriptionPath, entity.DescriptionPath);
            Assert.Equal(model.Image, entity.Image);
            Assert.Equal(model.ImageFileName, entity.ImageFileName);
            Assert.Equal(model.ImageMimeType, entity.ImageMimeType);
            Assert.Equal(model.ImagePath, entity.ImagePath);
            Assert.Equal(model.ProjectName, entity.ProjectName);
            Assert.Equal(model.UrlPath, entity.UrlPath);
        }

        [Fact]
        public void ProjectCaseModelTestCost()
        {
            // Preparing
            ProjectCaseModel modelLessZero = new ProjectCaseModel
            {
                Id = 23,
                Cost = -2
            };

            ProjectCaseModel modelRound = new ProjectCaseModel
            {
                Id = 23,
                Cost = 2.33564M
            };

            Assert.Equal(0, modelLessZero.Cost);
            Assert.Equal(2.34M, modelRound.Cost);
        }

        [Fact]
        public void ProjectCaseModelTest2ToModel()
        {
            // Input values
            ProjectCase entity = new ProjectCase
            {
                Id = 23,
                CategoryId = 1,
                Cost = 2,
                Description = "dsgdsd",
                DescriptionPath = "sagdsg",
                Image = new byte[] { 1, 0, 0, 1, 1, 0, 1 },
                ImageFileName = "dsgdsgds",
                ImageMimeType = "sajhbgf",
                ImagePath = "dskjjkf",
                ProjectName = "skjnv fn v",
                UrlPath = "dsfkjbkggggg"
            };

            // RUN
            ProjectCaseModel model = new ProjectCaseModel();
            model.ToModel(entity);

            // Validate result
            Assert.Equal(entity.Id, model.Id);
            Assert.Equal(entity.CategoryId, model.CategoryId);
            Assert.Equal(entity.Cost, model.Cost);
            Assert.Equal(entity.Description, model.Description);
            Assert.Equal(entity.DescriptionPath, model.DescriptionPath);
            Assert.Equal(entity.Image, model.Image);
            Assert.Equal(entity.ImageFileName, model.ImageFileName);
            Assert.Equal(entity.ImageMimeType, model.ImageMimeType);
            Assert.Equal(entity.ImagePath, model.ImagePath);
            Assert.Equal(entity.ProjectName, model.ProjectName);
            Assert.Equal(entity.UrlPath, model.UrlPath);
        }
        #endregion





        #region TMPTEMPLATE

        [Fact]
        public void ArticleeModelTestToEntity()
        {
            // Preparing
            ArticleModel model = new ArticleModel
            {
                Id = 23,
                CategoryId = 1,
                Text = "Textasfg",
                Topic = "topicsfafg",
                UserId = "UserIdskdgfhfd"
            };

            // RUN
            Article entity = model.ToEntity();

            // Validate result
            Assert.NotNull(entity);
            Assert.Equal(model.Id, entity.Id);
            Assert.Equal(model.CategoryId, entity.CategoryId);
            Assert.Equal(model.Text, entity.Text);
            Assert.Equal(model.Topic, entity.Topic);
            Assert.Equal(model.UserId, entity.UserId);
        }

        [Fact]
        public void ArticleeModelTest2ToModel()
        {
            // Input values
            Article entity = new Article
            {
                Id = 23,
                CategoryId = 1,
                Text = "Textasfg",
                Topic = "topicsfafg",
                UserId = "UserIdskdgfhfd"
            };

            // RUN
            ArticleModel model = new ArticleModel();
            model.ToModel(entity);

            // Validate result
            Assert.Equal(entity.Id, model.Id);
            Assert.Equal(entity.CategoryId, model.CategoryId);
            Assert.Equal(entity.Text, model.Text);
            Assert.Equal(entity.Topic, model.Topic);
            Assert.Equal(entity.UserId, model.UserId);
        }
        #endregion
    }
}
