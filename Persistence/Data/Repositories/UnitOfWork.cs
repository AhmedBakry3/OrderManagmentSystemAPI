


namespace Persistence.Data.Repositories
{
    public class UnitOfWork(OrderManagementDbContext _dbContext) : IUnitOfWork
    {
        private readonly Dictionary<string, object> _Repositories = [];
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>    
        {
            var TypeName = typeof(TEntity).Name;

            if (_Repositories.TryGetValue(TypeName, out object? Value))
                return (IGenericRepository<TEntity, TKey>)Value;
            else
            {
                var Repo = new GenericRepository<TEntity, TKey>(_dbContext);
                _Repositories["TypeName"] = Repo;
                return Repo;
            }
        }

        public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();
    }
}
