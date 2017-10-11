using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using ProjetoModelo.Domain.Entities;
using ProjetoModelo.Domain.Interfaces.Repository.ReadOnly;
using ProjetoModelo.Domain.ValueObjects;

namespace ProjetoModelo.Infra.Data.Repositories.ReadOnly
{
    public class ClienteReadOnlyRepository : RepositoryBaseReadOnly, IClienteReadOnlyRepository
    {


        public Cliente GetById(Guid id)
        {
            using (IDbConnection cn = Connection)
            {
                var sql = @"Select * From Cliente c " +
                          "WHERE c.ClienteId = @sid "+
                          "Select * From EnderecosCliente ec " +
                          "Inner join Endereco e " +
                          "On e.EnderecoId = ec.EnderecoId " +
                          "Inner join Estado s " +
                          "On s.EstadoId = e.EstadoId " +
                          "Inner join Cidade cl " +
                          "On cl.EstadoId = s.EstadoId " +
                          "WHERE ec.ClienteId = @sid";

                cn.Open();

                var multi = cn.QueryMultiple(sql, new {sid = id});
                var clientes = multi.Read<Cliente>();
                var enderecos = multi.Read<Endereco, Estado, Cidade, Endereco>(
                (ec, e, c) =>
                {
                    ec.EstadoId = e.EstadoId;
                    ec.CidadeId = c.CidadeId;
                    ec.Estado = e;
                    ec.Cidade = c;

                    return ec;

                }, splitOn: "EnderecoId, EstadoId, CidadeId");

                var cliente = clientes.First();

                foreach (var endereco in enderecos)
                {
                    cliente.EnderecoList.Add(endereco);
                }
                
                return cliente;
            }
        }
      
        public IEnumerable<Cliente> GetAll()
        {
            using (IDbConnection cn = Connection)
            {
                cn.Open();

                var sql = @"SELECT * FROM Cliente
                            ORDER BY Nome asc";

                var clientes = cn.Query<Cliente>(sql);

                return clientes;
            }
        }


        public IEnumerable<Cliente> ObterClientesGrid(int page, string pesquisa)
        {
            using (IDbConnection cn = Connection)
            {
                cn.Open();

                var sql = @"SELECT * FROM Cliente
                            WHERE (@nomePesquisa IS NULL OR Nome Like '%' + @nomePesquisa + '%')
                            ORDER BY Nome asc
                            OFFSET @pagina ROWS
                            FETCH NEXT 5 ROWS ONLY";

                var clientes = cn.Query<Cliente>(sql, new { nomePesquisa = pesquisa, pagina = page});

                return clientes;
            }
        }


        public int ObterTotalRegistros(string pesquisa)
        {
            using (IDbConnection cn = Connection)
            {
                cn.Open();

                var sql = @"SELECT COUNT(NOME) 'Total' FROM CLIENTE
                          WHERE (@nomePesquisa IS NULL OR Nome Like '%' + @nomePesquisa + '%')";

                var total = (int)cn.ExecuteScalar(sql, new { nomePesquisa = pesquisa } );

                return total;
            }
        }
    }
}
