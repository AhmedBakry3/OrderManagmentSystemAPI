


namespace Persistence
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(
            IQueryable<TEntity> inputQuery,
            ISpecifications<TEntity, TKey> specifications) where TEntity : BaseEntity<TKey>
        {
            var query = inputQuery;

            // Apply where clause
            if (specifications.Criteria is not null)
            {
                query = query.Where(specifications.Criteria);
            }

            // Apply includes
            if (specifications.IncludeExpressions is not null && specifications.IncludeExpressions.Count > 0)
            {
                query = specifications.IncludeExpressions.Aggregate(query, (current, includeExpression) => current.Include(includeExpression));
            }

            return query; 
        }
    }
}
