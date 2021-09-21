using System.Threading.Tasks;

namespace OnoStore.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}