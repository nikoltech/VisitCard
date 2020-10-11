namespace VisitCardApp.BusinessLogic.Test.Managements
{
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using VisitCardApp.BusinessLogic.Models;
    using VisitCardApp.DataAccess;
    using VisitCardApp.DataAccess.Entities;
    using VisitCardApp.DataAccess.Repositories;
    using Xunit;

    public class ArticleManagementFixture
    {
        private Mock<IRepository> repo { get; set; }

        public ArticleManagementFixture()
        {
            this.repo = new Mock<IRepository>();
        }

        // Input values

        // Setups

        // RUN

        // ASSERTS

        #region Tests

        [Fact]
        public async Task CreateArticleAsyncTest1Success()
        {
            // Input values
            string fileFolderPath = "";
            ArticleModel model = new ArticleModel
            {
                Id = 23,
                CategoryId = 1,
                Text = "Textasfg",
                Topic = "topicsfafg",
                UserId = "UserIdskdgfhfd"
            };

            // Setups
            Article entity = new Article
            {
                Id = model.Id,
                CategoryId = model.CategoryId,
                Text = model.Text,
                Topic = model.Topic,
                UserId = model.UserId
            };

            this.repo.Setup(r => r.CreateArticleAsync(It.IsAny<Article>(), It.IsAny<string>())).ReturnsAsync(entity).Verifiable();

            // RUN
            Article result = await this.repo.Object.CreateArticleAsync(model.ToEntity(), fileFolderPath).ConfigureAwait(false);

            // ASSERTS
            Assert.Equal(entity.Id, result.Id);
            Assert.Equal(entity.CategoryId, result.CategoryId);
            Assert.Equal(entity.Text, result.Text);
            Assert.Equal(entity.Topic, result.Topic);
            Assert.Equal(entity.UserId, result.UserId);
        }

        











        #endregion

        #region Private fields

        #endregion
    }
}
