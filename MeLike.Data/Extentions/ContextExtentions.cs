using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace MeLike.Data.Extentions
{
    public static class ContextExtentions
    {
        public static void Add<T>(this IMongoCollection<T> collection)
        {

        }
        public static void Remove<T>(this IMongoCollection<T> collection)
        {

        }
        public static void Where<T>(this IMongoCollection<T> collection, Expression<Func<T, bool>> predicate)
        {
            collection.FindAsync(x => true);
        }
        public static Task<List<T>> ToListAsync<T>(this IMongoCollection<T> collection)
        {
            return collection.AsQueryable().ToListAsync();
        }
        public static void Select<T>(this IMongoCollection<T> collection)
        {
            collection.AsQueryable();
        }
    }
}
