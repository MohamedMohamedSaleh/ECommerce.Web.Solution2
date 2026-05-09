using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared
{
    public class PaginatedResult<TEntity>
    {
        public PaginatedResult(int pageIndex, int pageCount, int countPerPage, int count, IEnumerable<TEntity> data)
        {
            this.PageIndex = pageIndex;
            this.PageCount = pageCount;
            this.CountPerPage = countPerPage;
            this.Count = count;
            this.Data = data;
        }

        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public int CountPerPage { get; set; }
        public int Count { get; set; }
        public bool HasNext => PageIndex * CountPerPage < Count;
        public IEnumerable<TEntity> Data { get; set; } = Enumerable.Empty<TEntity>();
    }
}
