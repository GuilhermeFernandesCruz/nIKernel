using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Repositories;
using Web.Models.Produto;
using System.Threading.Tasks;

namespace Web.Pages.Admin.Produtos
{
    public class EditarModel : PageModel
    {
        private readonly ProdutoRepository _repo;
        public EditarModel(ProdutoRepository repo) => _repo = repo;

        [BindProperty]
        public ProdutoModel Produto { get; set; } = new ProdutoModel();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Permissão: ajuste conforme sua lógica de claims
            var claim = User.FindFirst("Permissao_Produtos")?.Value;
            if (string.IsNullOrEmpty(claim) || claim.Split(',')[2] != "S")
                return RedirectToPage("/Index");

            var produto = await _repo.BuscarPorIdAsync(id);
            if (produto == null)
                return RedirectToPage("Index");
            Produto = produto;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            await _repo.AtualizarAsync(Produto);
            return RedirectToPage("Index");
        }
    }
}
