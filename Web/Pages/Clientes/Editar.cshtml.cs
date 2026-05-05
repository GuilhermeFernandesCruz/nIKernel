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
            var client = await _clienteRepo.BuscarPorIdAsync(id);
            if (client == null) { 
                return RedirectToPage("/Clientes/Index");
            }
            Cliente = client;

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
            await _clienteRepo.AtualizarAsync(Cliente);
            return RedirectToPage("/Clientes/Index");   
        }
        public async Task<IActionResult> OnPostDeletarAsync(int id)
        {
            await _clienteRepo.DeletarAsync(id);
            return RedirectToPage("/Clientes/Index");
        }
    }
    
}