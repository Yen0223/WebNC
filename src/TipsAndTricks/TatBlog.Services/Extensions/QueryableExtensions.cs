using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Services.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<TSource> WhereIf<TSource>(
            this IQueryable<TSource> source,
            bool condition,
            Expression<Func<TSource, bool>> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }
    }
}
