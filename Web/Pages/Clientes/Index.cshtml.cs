using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using nIKernel.Repositories;
using nIKernel.Models.Cliente;

namespace nIKernel.Pages.Clientes
{
    public class IndexModel : PageModel
    {
        private readonly ClienteRepository _clienteRepo;

        public IndexModel(ClienteRepository clienteRepo)
        {
            _clienteRepo = clienteRepo;
        }

        public IEnumerable<ClienteModel> Clientes {get; set;} = new List<ClienteModel>();

        public async Task<IActionResult> OnGetAsync()
        {
            Clientes = await _clienteRepo.ListarTodosAsync();
            return Page();
        }

    }
}