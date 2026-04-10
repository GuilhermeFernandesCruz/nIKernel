using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using nIKernel.Models.Perfil;
using nIKernel.Repositories;

namespace nIKernel.Pages.Admin.Perfis
{
    public class EditarModel : PageModel
    {
        private readonly PerfilRepository _repo;

        public EditarModel(PerfilRepository repo)
        {
            _repo = repo;
        }

        [BindProperty]
        public PerfilModel Perfil { get; set; } = new PerfilModel();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var claimPerfis = User.FindFirst("Permissao_Perfis")?.Value;
            bool podeEditar = !string.IsNullOrEmpty(claimPerfis) && claimPerfis.Split(',')[2]?.Trim().ToUpper() == "S";

            if (!podeEditar)
            {
                return RedirectToPage("./Index");
            }

            var perfilDoBanco = await _repo.BuscarPorIdAsync(id);
            if (perfilDoBanco == null)
            {
                return RedirectToPage("./Index");
            }
            Perfil = perfilDoBanco;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _repo.AtualizarAsync(Perfil);
            return RedirectToPage("./Index");
        }
    }
}