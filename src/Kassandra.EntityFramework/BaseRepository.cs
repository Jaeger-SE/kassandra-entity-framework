using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Data.Entity;

namespace Kassandra.EntityFramework
{
    public abstract class BaseRepository<TItem, TDbContext> where TItem : class where TDbContext : DbContext
    {
        protected readonly TDbContext Context;

        protected BaseRepository(TDbContext context)
        {
            Context = context;
        }

        private IEnumerable<TItem> GetIncludeIntoContextSet(params Expression<Func<TItem, object>>[] includes)
        {
            return includes.Aggregate(Context.Set<TItem>().AsQueryable(),
                (query, expression) => query.Include(expression));
        }

        public virtual TItem Get(Func<TItem, bool> query, params Expression<Func<TItem, object>>[] includes)
        {
            return GetIncludeIntoContextSet(includes).SingleOrDefault(query);
        }

        public virtual IList<TItem> GetAll(params Expression<Func<TItem, object>>[] includes)
        {
            return GetIncludeIntoContextSet(includes).ToList();
        }

        public virtual IList<TItem> GetAll(int page, int pageSize = 10,
            params Expression<Func<TItem, object>>[] includes)
        {
            return GetIncludeIntoContextSet(includes).Skip(page*pageSize).Take(pageSize).ToList();
        }

        public virtual void Delete(Func<TItem, bool> query)
        {
            Delete(Get(query));
        }

        public virtual void Delete(TItem entity)
        {
            Context.Entry(entity).State = EntityState.Deleted;
        }

        public TItem PushNew(TItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            Context.Set<TItem>().Attach(item);
            Context.Entry(item).State = EntityState.Added;

            return item;
        }

        public TItem PushUpdate(TItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            Context.Set<TItem>().Attach(item);
            Context.Entry(item).State = EntityState.Modified;

            return item;
        }
    }
}