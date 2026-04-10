using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using nIKernel.Models.Usuario;
using nIKernel.Repositories;

namespace nIKernel.Pages.Admin.Usuarios
{
    public class EditarModel : PageModel
    {
        private readonly UsuarioRepository _usuarioRepo;
        private readonly PerfilRepository _perfilRepo;

        public EditarModel(UsuarioRepository usuarioRepo, PerfilRepository perfilRepo)
        {
            _usuarioRepo = usuarioRepo;
            _perfilRepo = perfilRepo;
        }

        [BindProperty]
        public UsuarioModel Usuario {get; set;} = new UsuarioModel();
        public SelectList? Perfis {get; set;}
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var user = await _usuarioRepo.BuscarPorIdAsync(id);
            if (user == null) return RedirectToPage("./Index");

            Usuario = user;

            var listaPerfis = await _perfilRepo.ListarTodosAsync();
            Perfis = new SelectList(listaPerfis, "PEF_ID", "PRF_DSC");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Removemos a validação de senha e confirmação para o  EDITAR
            ModelState.Remove("Usuario.USU_PWD");
            ModelState.Remove("Usuario.ConfirmarSenha");

            if (!ModelState.IsValid)
            {
                var listaPerfis = await _perfilRepo.ListarTodosAsync();
                Perfis = new SelectList(listaPerfis, "PRF_ID", "PRF_DSC");

                return Page();
            }

            await _usuarioRepo.AtualizarAsync(Usuario);
            return RedirectToPage("./Index");
        }

    }
}