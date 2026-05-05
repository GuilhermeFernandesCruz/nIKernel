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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                foreach (var erro in ModelState)
                {
                    Console.WriteLine($"ERROR {erro.Key}: {string.Join(",", erro.Value.Errors.Select(e => e.ErrorMessage))}");
                }

                return Page();
            }
            
            // Atualizando o cliente selecionado
            await _clienteRepo.InserirAsync(Cliente);
            return RedirectToPage("/Clientes/Index");   
        }
    }
}