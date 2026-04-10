using Dapper;
using Microsoft.Data.SqlClient;
using nIKernel.Models.Perfil;
namespace nIKernel.Repositories
{
    public class PerfilRepository
    {
        private readonly string _connectionString;
        public PerfilRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }
        public async Task<IEnumerable<PerfilModel>> ListarTodosAsync()
        {
            using var db = new SqlConnection(_connectionString);
            string sql = "SELECT PRF_ID, PRF_DSC, PRF_STA FROM TB_PRF_PERFIL_ACESSO ORDER BY PRF_DSC";
            return await db.QueryAsync<PerfilModel>(sql);
        }

        public async Task<PerfilModel?> BuscarPorIdAsync(int id)
        {
            using var db = new SqlConnection(_connectionString);
            string sql = "SELECT PRF_ID, PRF_DSC, PRF_STA FROM TB_PRF_PERFIL_ACESSO WHERE PRF_ID = @Id";
            return await db.QueryFirstOrDefaultAsync<PerfilModel>(sql, new {Id= id});
        }

        public async Task InserirAsync(PerfilModel perfil)
        {
            using var db = new SqlConnection(_connectionString);
            string sql = "INSERT INTO TB_PRF_PERFIL_ACESSO (PRF_DSC, PRF_STA) VALUES (@Dsc, @Sta)";
            await db.ExecuteAsync(sql, new {Dsc = perfil.PRF_DSC, Sta = perfil.PRF_STA, Id = perfil.PRF_ID});
        }

        public async Task AtualizarAsync(PerfilModel perfil)
        {
            using var db = new SqlConnection(_connectionString);
            string sql = "UPDATE TB_PRF_PERFIL_ACESSO SET PRF_DSC = @Dsc, PRF_STA = @Sta WHERE PRF_ID = @Id";
            await db.ExecuteAsync(sql, new {Dsc = perfil.PRF_DSC, Sta = perfil.PRF_STA, Id = perfil.PRF_ID });
        }

        public async Task DeletarAsync(int id)
        {
            using var db = new SqlConnection(_connectionString);
            string sql = "DELETE FROM TB_PRF_PERFIL_ACESSO WHERE PRF_ID = @Id";
            await db.ExecuteAsync(sql, new {Id = id});
        }
    }
}