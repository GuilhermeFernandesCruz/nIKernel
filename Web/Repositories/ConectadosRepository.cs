using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;

namespace nIKernel.Repositories
{
    public class ConectadosRepository
    {
        private readonly string _connectionString;
        public ConectadosRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        // Retorna um tipo dinâmico pois é um JOIN entre sessões e Usuários
        public async Task<IEnumerable<dynamic>> ListarTodosAsync()
        {
            using var db = new SqlConnection(_connectionString);
            string sql = @"SELECT 
            C.UCN_ID,
            C.UCN_DTA_INC,
            C.UCN_SESSION_ID,
            C.UCN_AGT,
            U.USU_LOG
            FROM TB_UCN_USUARIOS_CONECTADOS C
            INNER JOIN TB_USU_USUARIOS U ON C.USU_ID = U.USU_ID
            ORDER BY C.UCN_DTA_INC DESC";
            return await db.QueryAsync(sql);
        }

        // Apaga o registro da sessão, forçando o usuário a logar novamente
        public async Task DeletarSessaoAsync(int id)
        {
            using var db = new SqlConnection(_connectionString);
            string sql = "DELETE FROM TB_UCN_USUARIOS_CONECTADOS WHERE UCN_ID = @Id";
            await db.ExecuteAsync(sql, new { Id = id });
        }
    }
}