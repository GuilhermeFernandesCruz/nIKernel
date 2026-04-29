using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace nIKernel.Pages
{
    [Authorize] // Garante que apenas usuários logados acessem a página inicial (Home)
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}