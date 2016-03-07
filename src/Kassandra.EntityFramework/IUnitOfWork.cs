using System;

namespace Kassandra.EntityFramework
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
        void Dispose(bool disposing);
    }
}