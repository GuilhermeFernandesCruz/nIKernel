using System.Data;
using MySqlConnector;
using Dapper;
using nIKernel.Models.Usuario;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace nIKernel.Repositories
{
    public class UsuarioRepository
    {
        private readonly string _connectionString;
        public UsuarioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        public UsuarioModel? ValidarLogin(
    string loginOuEmail,
    string senhaDigitada,
    string ipCliente,
    string hostNavegador,
    string sessionId)
{
    using var connection = new MySqlConnection(_connectionString);

    connection.Open();

    // =====================================================
    // BUSCA USUÁRIO
    // =====================================================

    var usuario = connection.QueryFirstOrDefault<UsuarioModel>(@"
        SELECT
            USU_ID,
            PRF_ID,
            USU_LOG,
            USU_PWD,
            USU_NAM,
            USU_STA
        FROM TB_USU_USUARIOS
        WHERE
        (
            USU_LOG = @LOGIN
            OR USU_EMAIL = @LOGIN
        )
        AND USU_STA = 'A'
    ", new
    {
        LOGIN = loginOuEmail
    });

    if (usuario == null)
        return null;

    // =====================================================
    // VALIDA SENHA COM SHA256
    // =====================================================

    string senhaHash = GerarHashSHA256(senhaDigitada);

    if (usuario.USU_PWD != senhaHash)
        return null;

    // =====================================================
    // CLAIMS
    // =====================================================

    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, usuario.USU_NAM),

        new Claim(
            "UsuarioID",
            usuario.USU_ID.ToString()
        ),

        new Claim(
            "PerfilID",
            usuario.PRF_ID.ToString()
        ),

        new Claim(
            "SessionID",
            sessionId
        )
    };

    // =====================================================
    // MENU DINÂMICO
    // =====================================================

    var permissoesBanco = connection.Query(@"
        SELECT
            O.OBJ_NAM,
            O.OBJ_DSC,
            P.OBJ_PRF_CNT,
            P.OBJ_PRF_INP,
            P.OBJ_PRF_UPT,
            P.OBJ_PRF_DEL,
            P.OBJ_PRF_PRT
        FROM TB_OBJ_PRF_OBJETO_PERFIL P
        INNER JOIN TB_OBJ_OBJETO_SISTEMA O
            ON P.OBJ_ID = O.OBJ_ID
        WHERE
            P.PRF_ID = @PERFIL
            AND O.OBJ_STA = 'A'
        ORDER BY O.OBJ_NAM
    ", new
    {
        PERFIL = usuario.PRF_ID
    });

    foreach (var perm in permissoesBanco)
    {
        string direitos =
            $"{perm.OBJ_PRF_CNT}," +
            $"{perm.OBJ_PRF_INP}," +
            $"{perm.OBJ_PRF_UPT}," +
            $"{perm.OBJ_PRF_DEL}," +
            $"{perm.OBJ_PRF_PRT}";

        claims.Add(new Claim(
            $"Permissao_{perm.OBJ_NAM}",
            direitos
        ));

        if (perm.OBJ_PRF_CNT == "S")
        {
            claims.Add(new Claim(
                "MenuItem",
                $"{perm.OBJ_NAM}|{perm.OBJ_DSC}|fa-solid fa-folder"
            ));
        }
    }

    usuario.ClaimsDinamicas = claims;

    return usuario;
}

        public async Task<IEnumerable<UsuarioModel>> ListarTodosAsync()
        {
            using var db = new MySqlConnection(_connectionString);
            string slq = @"SELECT U.*, P.PRF_DSC as PerfilDescricao FROM tb_usu_usuarios U 
            INNER JOIN tb_prf_perfil_acesso P ON U.PRF_ID = P.PRF_ID ORDER BY U.USU_NAM";
            return await db.QueryAsync<UsuarioModel>(slq);
        }

        public async Task InserirAsync(UsuarioModel usuario)
        {
            using var db = new MySqlConnection(_connectionString);
            
            usuario.USU_PWD = GerarHashSha256(usuario.USU_PWD);
            usuario.USU_DTA_INC = DateTime.Now;
            usuario.USU_CNT = "N";
            string sql = @"INSERT INTO TB_USU_USUARIOS (PRF_ID, USU_LOG, USU_PWD, USU_NAM, USU_DTA_INC, 
            USU_STA, USU_CNT, USU_EMAIL, USU_CEL) VALUES (@PRF_ID, @USU_LOG, @USU_PWD, @USU_NAM, @USU_DTA_INC, @USU_STA, @USU_CNT, @USU_EMAIL, @USU_CEL)";
            await db.ExecuteAsync(sql, usuario);
        }

        public async Task DeletarAsync(int id)
        {
            using var db = new MySqlConnection(_connectionString);
            string sql = "DELETE FROM TB_USU_USUARIOS WHERE USU_ID = @Id";
            await db.ExecuteAsync(sql, new { Id = id });
        }

        public async Task RegistrarLogoutAsync(int usuId)
        {
            using IDbConnection db = new MySqlConnection(_connectionString);
            string sql = "UPDATE TB_USU_USUARIOS SET USU_CNT = 'N' WHERE USU_ID = @UsuId";
            await db.ExecuteAsync(sql, new { UsuId = usuId });
        }

        private string GerarHashSha256(string texto)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(texto));
                StringBuilder builder = new StringBuilder();
                for ( int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private string GerarHashSHA256(string texto)
    {
        using var sha256 = SHA256.Create();

        byte[] bytes = Encoding.UTF8.GetBytes(texto);

        byte[] hash = sha256.ComputeHash(bytes);

        StringBuilder builder = new StringBuilder();

        foreach (byte b in hash)
        {
            builder.Append(b.ToString("x2"));
        }

        return builder.ToString();
    }

        public async Task<UsuarioModel?> BuscarPorIdAsync(int id)
        {
            using var db = new MySqlConnection(_connectionString);
            string sql = @"SELECT * FROM TB_USU_USUARIOS WHERE USU_ID = @Id";
            return await db.QueryFirstOrDefaultAsync<UsuarioModel>(sql, new { Id = id });
        }

        public async Task AtualizarAsync(UsuarioModel usuario)
        {
            using var db = new MySqlConnection(_connectionString);
            string sql = @"UPDATE TB_USU_USUARIOS
            SET PRF_ID = @PRF_ID, 
            USU_LOG = @USU_LOG, 
            USU_NAM = @USU_NAM, 
            USU_EMAIL = @USU_EMAIL, 
            USU_CEL = @USU_CEL 
            WHERE USU_ID = @USU_ID";
            await db.ExecuteAsync(sql, usuario);
        }
    }
}
