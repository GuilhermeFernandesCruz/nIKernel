namespace nIKernel.Models.Perfil
{
    public class PerfilModel
    {
        public int PRF_ID {get; set;}
        public string PRF_DSC {get; set;} = string.Empty; // Node do Perfil (Ex: Adm, Operador)
        public string PRF_STA {get; set;} = "A"; // Situação: Ativo(A) e Inativo(I)
    }
}