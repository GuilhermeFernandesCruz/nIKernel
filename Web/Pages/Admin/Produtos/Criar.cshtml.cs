using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Repositories;
using Web.Models.Produto;
using System.Threading.Tasks;

namespace Web.Pages.Admin.Produtos
{
    public class CriarModel : PageModel
    {
        private readonly ProdutoRepository _repo;
        public CriarModel(ProdutoRepository repo) => _repo = repo;

        [BindProperty]
        public ProdutoModel Produto { get; set; } = new ProdutoModel();

        public IActionResult OnGet()
        {
            // Permissão: ajuste conforme sua lógica de claims
            var claim = User.FindFirst("Permissao_Produtos")?.Value;
            if (string.IsNullOrEmpty(claim) || claim.Split(',')[1] != "S")
                return RedirectToPage("/Index");
            Produto.prd_ativo = true;
            Produto.prd_data_criacao = System.DateTime.Now;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            Produto.prd_data_criacao = System.DateTime.Now;
            await _repo.InserirAsync(Produto);
            return RedirectToPage("Index");
        }
    }
}
