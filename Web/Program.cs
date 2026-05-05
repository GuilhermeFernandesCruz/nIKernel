using nIKernel.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os serviços do Razor Pages
builder.Services.AddRazorPages();

// 1. INJEÇÃO DE DEPENDÊNCIA: Ensina o sistema a instanciar o seu repositório
builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<ConectadosRepository>();
builder.Services.AddScoped<PerfilRepository>();
builder.Services.AddScoped<ClienteRepository>();
builder.Services.AddScoped<Web.Repositories.ProdutoRepository>();

// 2. AUTENTICAÇÃO: configura os cookies de segurança do site
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index"; // Se tentar burlar o painel é jogado para cá
    });

// 3. SESSÃO: Ativa o motor que vai gerar o ID da sessão (UCN_SESSION_ID)
builder.Services.AddSession();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting(); 

// 4. ATIVANDO OS MOTORES (A ordem aqui é vital!)
app.UseSession();
app.UseAuthentication(); // Descobre QUEM é o usuário
app.UseAuthorization();  // Descobre o que o usuário pode fazer

app.MapRazorPages();
app.Run();