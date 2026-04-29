using System;

namespace Web.Models.Produto
{
    public class ProdutoModel
    {
        public int prd_id { get; set; }
        public string prd_cod { get; set; }
        public string prd_gtin_ean { get; set; }
        public string prd_descricao { get; set; }
        public string prd_un_compra { get; set; }
        public string prd_un_venda { get; set; }
        public decimal prd_preco_compra { get; set; }
        public decimal prd_margem_venda { get; set; }
        public decimal prd_preco_venda { get; set; }
        public string prd_ativo { get; set; } // Alterado de bool para string
        public DateTime prd_data_criacao { get; set; }
    }
}