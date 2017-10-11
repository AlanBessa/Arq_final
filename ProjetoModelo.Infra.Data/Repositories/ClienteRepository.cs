using System.Data.Entity;
using ProjetoModelo.Domain.Entities;
using ProjetoModelo.Domain.Interfaces.Repository;
using ProjetoModelo.Infra.Data.Context;

namespace ProjetoModelo.Infra.Data.Repositories
{
    public class ClienteRepository : RepositoryBase<Cliente, ProjetoModeloContext>, IClienteRepository
    {
        public override void Remove(Cliente cliente)
        {
            var entry = Context.Entry(cliente);
            DbSet.Attach(cliente);
            entry.State = EntityState.Deleted;
        }
    }
}
