using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using nIKernel.Models.Perfil;
using nIKernel.Repositories;

namespace nIKernel.Pages.Admin.Perfis
{
    public class CriarModel : PageModel
    {
        private readonly PerfilRepository _repo;

        public CriarModel(PerfilRepository repo)
        {
            _repo = repo;
        }

        [BindProperty]
        public PerfilModel Perfil { get; set; } = new PerfilModel();

        public IActionResult OnGet()
        {
            var claimPerfis = User.FindFirst("Permissao_Perfis")?.Value;
            bool podeInserir = !string.IsNullOrEmpty(claimPerfis) && claimPerfis.Split(',')[1]?.Trim().ToUpper() == "S";

            if (!podeInserir)
            {
                return RedirectToPage("./Index");
            }    
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _repo.InserirAsync(Perfil);
            return RedirectToPage("./Index");
        }
    }
}