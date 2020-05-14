namespace VisitCardApp.BusinessLogic.Models
{
    public interface IEntityModel<TEntity> 
        where TEntity : class
    {
        void ToModel(TEntity entity);

        TEntity ToEntity();
    }
}
