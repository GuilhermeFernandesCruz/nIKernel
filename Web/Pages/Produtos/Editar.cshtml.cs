using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Repositories;
using MySqlConnector;
using Web.Models.Produto;
using System;
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
                
            // Recalcula o preco de venda caso a margem ou custo sofram alteracao
            Produto.prd_preco_venda = Produto.prd_preco_compra + (Produto.prd_preco_compra * (Produto.prd_margem_venda / 100));
            
            try
            {
                await _repo.AtualizarAsync(Produto);
                return RedirectToPage("Index");
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                AddDuplicateKeyError(ex);
                return Page();
            }
        }

        private void AddDuplicateKeyError(MySqlException ex)
        {
            var message = ex.Message ?? string.Empty;
            if (message.Contains("prd_gtin_ean", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("Produto.prd_gtin_ean", "GTIN/EAN ja cadastrado.");
                return;
            }
            if (message.Contains("prd_cod", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("Produto.prd_cod", "Codigo ja cadastrado.");
                return;
            }

            ModelState.AddModelError(string.Empty, "Nao foi possivel salvar o produto porque um valor duplicado foi encontrado.");
        }
    }
}
