using Microsoft.Data.Entity;

namespace Kassandra.EntityFramework
{
    public interface IEntityBuilder
    {
        void Configure(ModelBuilder modelBuilder);
    }
}