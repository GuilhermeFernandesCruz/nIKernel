using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using nIKernel.Models.Cliente;
using nIKernel.Repositories;

namespace nIKernel.Pages.Clientes
{
    public class CriarModel : PageModel
    {
        private readonly ClienteRepository _clienteRepo;

        public CriarModel(ClienteRepository clienteRepo)
        {
            _clienteRepo = clienteRepo;
        }

        [BindProperty]
        public ClienteModel Cliente {get; set;} = new ClienteModel();

        public IActionResult OnGet()
        {
            var claim = User.FindFirst("Permissao_Clientes")?.Value;

            // Inserir
            if (string.IsNullOrEmpty(claim) || claim.Split(',')[1] != "S")
                return RedirectToPage("/Index");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var claim = User.FindFirst("Permissao_Clientes")?.Value;

            // Inserir
            if (string.IsNullOrEmpty(claim) || claim.Split(',')[1] != "S")
            {
                return RedirectToPage("/Index");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _clienteRepo.InserirAsync(Cliente);
            return RedirectToPage("/Clientes/Index");
        }
    }
}