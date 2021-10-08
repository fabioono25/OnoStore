using OnoStore.Core.Data;
using System.Threading.Tasks;

namespace NSE.Pedidos.Domain
{
    public interface IVoucherRepository : IRepository<Voucher>
    {
        Task<Voucher> GetVoucherByCode(string codigo);
        void Atualizar(Voucher voucher);
    }
}