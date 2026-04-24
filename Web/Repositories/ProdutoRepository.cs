using Dapper;
using Microsoft.Data.SqlClient;
using Web.Models.Produto;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Web.Repositories
{
    public class ProdutoRepository
    {
        private readonly string _connectionString;

        public ProdutoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public async Task<IEnumerable<ProdutoModel>> ListarTodosAsync()
        {
            using var db = new SqlConnection(_connectionString);
            string sql = @"SELECT prd_id, prd_cod, prd_gtin_ean, prd_descricao, prd_un_compra, prd_un_venda, 
                                  prd_preco_compra, prd_margem_venda, prd_preco_venda, prd_ativo, prd_data_criacao
                           FROM TB_PRD_PRODUTO
                           ORDER BY prd_descricao";
            return await db.QueryAsync<ProdutoModel>(sql);
        }

        public async Task<ProdutoModel?> BuscarPorIdAsync(int id)
        {
            using var db = new SqlConnection(_connectionString);
            string sql = @"SELECT prd_id, prd_cod, prd_gtin_ean, prd_descricao, prd_un_compra, prd_un_venda, 
                                  prd_preco_compra, prd_margem_venda, prd_preco_venda, prd_ativo, prd_data_criacao
                           FROM TB_PRD_PRODUTO
                           WHERE prd_id = @Id";
            return await db.QueryFirstOrDefaultAsync<ProdutoModel>(sql, new { Id = id });
        }

        public async Task InserirAsync(ProdutoModel produto)
        {
            using var db = new SqlConnection(_connectionString);
            string sql = @"INSERT INTO TB_PRD_PRODUTO 
                            (prd_cod, prd_gtin_ean, prd_descricao, prd_un_compra, prd_un_venda, 
                             prd_preco_compra, prd_margem_venda, prd_preco_venda, prd_ativo, prd_data_criacao)
                           VALUES
                            (@prd_cod, @prd_gtin_ean, @prd_descricao, @prd_un_compra, @prd_un_venda, 
                             @prd_preco_compra, @prd_margem_venda, @prd_preco_venda, @prd_ativo, @prd_data_criacao)";
            await db.ExecuteAsync(sql, produto);
        }

        public async Task AtualizarAsync(ProdutoModel produto)
        {
            using var db = new SqlConnection(_connectionString);
            string sql = @"UPDATE TB_PRD_PRODUTO SET
                                prd_cod = @prd_cod,
                                prd_gtin_ean = @prd_gtin_ean,
                                prd_descricao = @prd_descricao,
                                prd_un_compra = @prd_un_compra,
                                prd_un_venda = @prd_un_venda,
                                prd_preco_compra = @prd_preco_compra,
                                prd_margem_venda = @prd_margem_venda,
                                prd_preco_venda = @prd_preco_venda,
                                prd_ativo = @prd_ativo,
                                prd_data_criacao = @prd_data_criacao
                           WHERE prd_id = @prd_id";
            await db.ExecuteAsync(sql, produto);
        }
    }
}




