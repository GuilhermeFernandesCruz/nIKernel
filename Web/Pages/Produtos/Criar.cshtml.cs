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
            Produto.prd_ativo = "S";
            Produto.prd_data_criacao = System.DateTime.Now;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            Produto.prd_data_criacao = System.DateTime.Now;
            
            // Calcula o preco de venda baseando-se no custo e margem
            Produto.prd_preco_venda = Produto.prd_preco_compra + (Produto.prd_preco_compra * (Produto.prd_margem_venda / 100));

            await _repo.InserirAsync(Produto);
            return RedirectToPage("Index");
        }
    }
}


