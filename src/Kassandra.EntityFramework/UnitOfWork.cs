using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;

namespace Kassandra.EntityFramework
{
    public class UnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
    {
        private TDbContext _context;

        public UnitOfWork()
        {
            _context = (TDbContext)Activator.CreateInstance(typeof(TDbContext), new DbContextOptions<TDbContext>());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose(bool disposing)
        {
            if (!disposing) return;

            if (_context == null) return;

            _context.Dispose();
            _context = null;
        }
    }
}
