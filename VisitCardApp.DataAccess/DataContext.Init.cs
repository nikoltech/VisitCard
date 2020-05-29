namespace VisitCardApp.DataAccess
{
	using Microsoft.EntityFrameworkCore;
	using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
	using VisitCardApp.DataAccess.Entities;
    using VisitCardApp.DataAccess.Enums;

    public partial class DataContext
    {
        public async Task<int> Initialize()
        {
			try
			{
				int updatedRows = 0;
				List<InitializeCode> initializedCodes = await this.InitializeCodes.ToListAsync();
				using (var dbTransaction = await this.Database.BeginTransactionAsync())
				{
					try
					{
						updatedRows += await this.AddCategoriesAsync(initializedCodes);

						dbTransaction.Commit();
						return updatedRows;
					}
					catch
					{
						dbTransaction.Rollback();
						throw;
					}
				}
			}
			catch
			{
				throw;
			}
        }

		private async Task<int> AddCategoriesAsync(List<InitializeCode> initializedCodes)
		{
			string initCode = "d2a00622-2567-401b-8d2a-b44120e8e1cd";

			int affectedRowsCount = 0;
			if (!initializedCodes.Any(c => c.InitializeCodeId.Equals(initCode)))
			{
				List<Category> categories = new List<Category>
				{
					new Category { Name = "All", Type = CategoryType.All },

					new Category { Name = "Project", Type = CategoryType.Project },
					new Category { Name = "Mark", Type = CategoryType.Project },

					new Category { Name = "Article", Type = CategoryType.Article },
					new Category { Name = "Mark", Type = CategoryType.Article },
				};

				this.Categories.AddRange(categories);

				affectedRowsCount = await this.SaveChangesAsync();

				if (affectedRowsCount > 0)
				{
					await this.SaveInitCode(initCode).ConfigureAwait(false);
				}
			}

			return affectedRowsCount;
		}


		#region SaveInitCode
		private async Task<int> SaveInitCode(string initCode)
		{
			this.InitializeCodes.Add(new InitializeCode { InitializeCodeId = initCode });
			return await this.SaveChangesAsync();
		}
        #endregion
    }
}
