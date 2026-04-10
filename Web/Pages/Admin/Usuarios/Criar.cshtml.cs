using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using nIKernel.Models.Usuario;
using nIKernel.Repositories;

namespace nIKernel.Pages.Admin.Usuarios
{
    public class CriarModel : PageModel
    {
        private readonly UsuarioRepository _usuarioRepo;
        private readonly PerfilRepository _perfilRepo;

        public CriarModel(UsuarioRepository usuarioRepo, PerfilRepository perfilRepo)
        {
            _usuarioRepo = usuarioRepo;
            _perfilRepo = perfilRepo;
        }

        [BindProperty]
        public UsuarioModel Usuario {get; set;} = new UsuarioModel();

        public SelectList? Perfis {get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var listaPerfis = await _perfilRepo.ListarTodosAsync();
            Perfis = new SelectList(listaPerfis, "PRF_ID", "PRF_DSC");
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var listaPerfis = await _perfilRepo.ListarTodosAsync();
                Perfis = new SelectList(listaPerfis, "PRF_ID", "PRF_DSC");
                return Page();
            }

            await _usuarioRepo.InserirAsync(Usuario);
            return RedirectToPage("./Index");
        }
    }
}