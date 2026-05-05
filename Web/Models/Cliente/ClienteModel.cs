using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace nIKernel.Models.Cliente
{
    public class ClienteModel
    {
        public int CL_id {get; set;}

        [Required(ErrorMessage = "A inserção do CPF ou CNPJ é obrigatória.")]
        public string CL_cpf_cnpj {get; set;} = String.Empty;

        public string CL_rg_ie {get; set;} = String.Empty;

        public string CL_nome {get; set;} = String.Empty;

        public string CL_apelido {get; set;} = String.Empty;

        [Required(ErrorMessage = "A inserção do Status atual do cliente é obrigatória.")]
        [StringLength(1)]
        public string CL_status { get; set; } = "A";

        public DateTime CL_data_inclusao {get; set;}
    }
}