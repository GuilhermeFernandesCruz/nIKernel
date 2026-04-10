using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using nIKernel.Repositories;

namespace nIKernel.Pages.Admin
{
    public class ConectadosModel : PageModel
    {
        private readonly ConectadosRepository _repo;

        public ConectadosModel(ConectadosRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<dynamic> ListaConectados {get; set;} = new List<dynamic>();

        public async Task OnGetAsync()
        {
            ListaConectados = await _repo.ListarTodosAsync();
        }

        public async Task<IActionResult> OnPostDerrubarAsync(int id)
        {
            // Validação dupla de Segurança: Verifica se o usuário que clicou realmente tem a permissão DEL ("S")
            var claimConectados = User.FindFirst("Permissao_Conectados")?.Value;
            bool podeDerrubar = !string.IsNullOrEmpty(claimConectados) && claimConectados.Split(',')[3].Trim().ToUpper() == "S";

            if (podeDerrubar)
            {
                // Se for Sênior (Admin), apaga a sessão do banco;
                await _repo.DeletarSessaoAsync(id); 
            }
            return RedirectToPage();
        }
    }
}