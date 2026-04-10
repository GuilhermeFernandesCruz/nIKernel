using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using nIKernel.Models.Perfil;
using nIKernel.Repositories;

namespace nIKernel.Pages.Admin.Perfis
{
    public class IndexModel : PageModel
    {
      private readonly PerfilRepository _repo;

      public IndexModel (PerfilRepository repo)
      {
        _repo = repo;
      }

      public IEnumerable<PerfilModel> Perfis { get; set; } = new List<PerfilModel>();

      public async Task<IActionResult> OnGetAsync()
      {
        var claimPerfis = User.FindFirst("Permissao_Perfis")?.Value;
        bool podeConsultar = !string.IsNullOrEmpty(claimPerfis) && claimPerfis.Split(',')[0].Trim().ToUpper() == "S";
        
        if (!podeConsultar)
        {
            return RedirectToPage("/Index");
        }

        Perfis = await _repo.ListarTodosAsync();
        return Page();
      }

      public async Task<IActionResult> OnPostDeletarAsync(int id)
      {
        var claimPerfis = User.FindFirst("Permissao_Perfis")?.Value;
        bool podeDeletar = !string.IsNullOrEmpty(claimPerfis) && claimPerfis.Split(',')[2].Trim().ToUpper() == "S";
        
        if (!podeDeletar)
        {
            await _repo.DeletarAsync(id);
        }
        return RedirectToPage();
      }
    }
}