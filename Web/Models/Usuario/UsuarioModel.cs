using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace nIKernel.Models.Usuario
{
    public class UsuarioModel
    {
        public int USU_ID {get; set;}

        [Required(ErrorMessage = "A seleção do perfil é obrigatória.")]
        public int PRF_ID {get; set;}

        [Required(ErrorMessage = "O Login é obrigatório.")]
        public string USU_LOG {get; set;} = String.Empty;

        [Required(ErrorMessage = "O Nome é obrigatório.")]
        public string USU_NAM {get; set;} = String.Empty;

        [Required(ErrorMessage = "A Senha é obrigatória.")]
        [DataType(DataType.Password)]
        public string USU_PWD {get; set;} = String.Empty;

        [Required(ErrorMessage = "A confirmação de senha é obrigatória.")]
        [DataType(DataType.Password)]
        [Compare("USU_PWD", ErrorMessage = "As senha não conferem.")]
        public string? ConfirmarSenha {get; set;}

        public DateTime USU_DTA_INC {get; set;}
        public DateTime? USU_DTA_FIN {get; set;}
        public string? USU_STA {get; set; } = "A";
        public string? USU_CNT {get; set; } = "N";

        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Formato de e-mail inválido.")]
        public string? USU_EMAIL {get; set;}

        [RegularExpression(@"^\(?\d\d{2}\)?[\s-]?\d{4,5}-?\d{4}$", ErrorMessage = "Formato de celular inválido.")]
        public string? USU_CEL {get; set;}

        public List<Claim>? ClaimsDinamicas {get; set;}
        public string? PerfilDescricao {get; set;}
    }
}