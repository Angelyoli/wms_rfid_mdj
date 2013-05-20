using System.Data.Entity;

namespace THOK.Common.Ef.Interfaces
{
    /// <summary>
    /// Context across all repositories
    /// </summary>
    public interface IRepositoryContext
    {
        DbSet<T> GetDbSet<T>() where T : class;
        DbContext DbContext { get; }
        int SaveChanges();
        void Terminate();
    }
}
