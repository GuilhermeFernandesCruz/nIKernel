using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using nIKernel.Repositories;
using nIKernel.Models.Usuario;

namespace nIKernel.Pages.Admin.Usuarios
{
    public class IndexModel : PageModel
    {
        private readonly UsuarioRepository _repo;
        public IndexModel (UsuarioRepository repo) => _repo = repo;

        public IEnumerable<UsuarioModel> Usuarios {get; set;} = new List<UsuarioModel>();

        public async Task<IActionResult> OnGetAsync()
        {
            var claim = User.FindFirst("Permissao_Usuarios")?.Value;
            if (string.IsNullOrEmpty(claim) || claim.Split(',')[0] != "S") return RedirectToPage("/Index");

            Usuarios = await _repo.ListarTodosAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostDeletarAsync (int id)
        {
            await _repo.DeletarAsync(id);
            return RedirectToPage();
        }
    }
}