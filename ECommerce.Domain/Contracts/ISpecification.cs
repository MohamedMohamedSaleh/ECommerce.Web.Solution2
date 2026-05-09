using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Contracts
{
    public interface ISpecification<TEntity, TKey>
     where TEntity : BaseEntity<TKey>
    {
        // Include expressions for eager loading related entities
        public ICollection<Expression<Func<TEntity, object>>> IncludeExpression { get; }

        // Criteria for filtering entities
        public Expression<Func<TEntity, bool>>? Criteria { get; }

        // Sorting expressions (optional)
        public Expression<Func<TEntity, object>>? OrderBy { get; }
        public Expression<Func<TEntity, object>>? OrderByDescending { get; }

        // Pagination properties (optional)
        public int Skip { get; }
        public int Take { get; }
        public bool IsPagingEnabled { get; }
    }
}
