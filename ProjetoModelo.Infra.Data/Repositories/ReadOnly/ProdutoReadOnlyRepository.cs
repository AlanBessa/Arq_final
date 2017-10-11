using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using ProjetoModelo.Domain.Entities;
using ProjetoModelo.Domain.Interfaces.Repository.ReadOnly;

namespace ProjetoModelo.Infra.Data.Repositories.ReadOnly
{
    public class ProdutoReadOnlyRepository : RepositoryBaseReadOnly, IProdutoReadOnlyRepository
    {
        public Produto GetById(Guid id)
        {
            using (IDbConnection cn = Connection)
            {
                cn.Open();

                var sql = @"Select * From Produto p " +
                          "WHERE p.ProdutoId ='" + id + "'";

                var produto = cn.Query<Produto>(sql);

                return produto.FirstOrDefault();
            }
        }

        public IEnumerable<Produto> GetAll()
        {
            using (IDbConnection cn = Connection)
            {
                cn.Open();

                var sql = @"SELECT p.*, f.Nome FROM Produto p 
                            INNER JOIN Fornecedor f ON p.FornecedorId = f.FornecedorId";

                var produto = cn.Query<Produto, Fornecedor, Produto>(
                    sql,
                    (p, f) =>
                    {
                        p.Fornecedor = f;

                        return p;
                    }, splitOn: "ProdutoId, FornecedorId");

                return produto;
            }
        }
    }
}