using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using nIKernel.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace nIKernel.Pages.Login
{
    public class IndexModel : PageModel
    {
     private readonly UsuarioRepository _usuarioRepository;

     // Injeção de Dependência: O ASP.NET entrega o repositório pronto para uso aqui
     public IndexModel(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }   

        [BindProperty] public InputModel Input {get; set;} = new();
        public class InputModel
        {
            [Required(ErrorMessage = "Por favor, informe o seu usuário.")]
            public string Usuario {get; set;} =string.Empty;

            [Required(ErrorMessage = "A senha é obrigatória.")]
            [DataType(DataType.Password)]
            public string Senha {get; set; } = string.Empty;
        }

        public void Onget() {}

        // Mudamos para Async porque a gravação do Cookie exige paralelismo (await)
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) {return Page(); }
             // 1. CAPTURANDO DADOS DA MÁQUINA DO USUÁRIO
                string ipCliente = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Desconhecido";
                string hostNavegador = Request.Headers["User-Agent"].ToString();

                // Forçamos o inicio da sessão para o ASP.NET gerar i, Session ID único
                HttpContext.Session.SetString("LoginIniciado", "Sim");
                string sessionId = HttpContext.Session.Id;

                try {
                    var usuario = _usuarioRepository.ValidarLogin(Input.Usuario, Input.Senha, ipCliente, hostNavegador, sessionId);
                    
                    if (usuario != null) {
                        if (usuario.USU_NAM == "SESSAO_ATIVA") 
                        {
                            ModelState.AddModelError(string.Empty, "Acesso Negado: Este usuário já possui uma sessão ativa no sistema.");
                            return Page();
                        }

                        var identity = new ClaimsIdentity(usuario.ClaimsDinamicas, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);

                        // NOVA BLINDAGEM DO COOKIE
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = false, //Força a ser um cookie de sessão (morte ao fechar o navegador)
                            ExpiresUtc= DateTimeOffset.UtcNow.AddMinutes(30) //Expira automaticamente no servidor após 30 min de inatividade
                        };

                        // Injetamos as propriedades aqui no SignIn
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

                        return RedirectToPage("/Index");
                } else
                {
                    ModelState.AddModelError(string.Empty, "Usuário, senha incorreta ou acesso inativo.");
                    return Page();
                }
            } catch (Exception ex)
            {
                // Erro genérico
                ModelState.AddModelError(string.Empty, "Erro interno ao validar login. Tente novamente.");
                return Page();
            }
        }
    }
}