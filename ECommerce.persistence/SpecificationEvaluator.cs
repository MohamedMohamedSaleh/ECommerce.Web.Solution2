using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.persistence
{
    public class SpecificationEvaluator
    {
        // Method to create a query based on the specification
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> EntryPoint, ISpecification<TEntity, TKey> specification)
        where TEntity : BaseEntity<TKey>
        {
            var query = EntryPoint;
            // There add any filtering logic based on
            // the specification here if needed (e.g., where clauses, sorting, etc.)

            // Apply includes from the specification
            #region Where Expression
            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }
            #endregion

            #region Include Expression
            if (specification.IncludeExpression != null && specification.IncludeExpression.Any())
            {


                /// simplified for loop using LINQ's Aggregate method to apply all include expressions
                ///foreach (var includeExp in specification.IncludeExpression)
                ///{
                ///    query = query.Include(includeExp);
                ///}

                /// Using LINQ's Aggregate method to apply all include expressions in a more concise way
                query = specification.IncludeExpression.Aggregate
                    (query, (current, includeExp) => current.Include(includeExp));


            }
            #endregion

            #region Ordering
            // Apply ordering if specified
            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }
            #endregion

            #region Pagination 
            if (specification.IsPagingEnabled)
            {
                query = query.Skip(specification.Skip).Take(specification.Take);
            }
            #endregion



            return query;
        }
    }
}
