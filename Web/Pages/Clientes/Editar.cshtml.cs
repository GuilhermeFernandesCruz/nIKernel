using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using nIKernel.Models.Cliente;
using nIKernel.Repositories;

namespace nIKernel.Pages.Clientes
{
    public class EditarModel : PageModel
    {
        private readonly ClienteRepository _clienteRepo;

        public EditarModel(ClienteRepository clienteRepo)
        {
            _clienteRepo = clienteRepo;
        }

        [BindProperty]
        public ClienteModel Cliente {get; set;} = new ClienteModel();
    
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var claim = User.FindFirst("Permissao_Clientes")?.Value;

            // Editar
            if (string.IsNullOrEmpty(claim) || claim.Split(',')[2] != "S")
            {
                return RedirectToPage("/Index");
            }

            var cliente = await _clienteRepo.BuscarPorIdAsync(id);

            if (cliente == null)
            {
                return RedirectToPage("/Clientes/Index");
            }

            Cliente = cliente;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var claim = User.FindFirst("Permissao_Clientes")?.Value;

            // Editar
            if (string.IsNullOrEmpty(claim) || claim.Split(',')[2] != "S")
            {
                return RedirectToPage("/Index");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _clienteRepo.AtualizarAsync(Cliente);
            return RedirectToPage("/Clientes/Index");
        }

        public async Task<IActionResult> OnPostDeletarAsync(int id)
        {
            var claim = User.FindFirst("Permissao_Clientes")?.Value;

            // Excluir
            if (string.IsNullOrEmpty(claim) || claim.Split(',')[3] != "S")
            {
                return RedirectToPage("/Index");
            }

            await _clienteRepo.DeletarAsync(id);
            return RedirectToPage("/Clientes/Index");
        }
    }
    
}