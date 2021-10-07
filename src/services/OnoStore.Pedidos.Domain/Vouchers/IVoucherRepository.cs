using System.Threading.Tasks;
using OnoStore.Core.Data;

namespace NSE.Pedidos.Domain
{
    public interface IVoucherRepository : IRepository<Voucher>
    {
        Task<Voucher> GetVoucherByCode(string codigo);
        void Atualizar(Voucher voucher);
    }
}