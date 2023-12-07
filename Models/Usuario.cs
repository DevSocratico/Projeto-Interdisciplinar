using System.ComponentModel.DataAnnotations;

namespace LocFarma.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public TipoUsuario Tipo { get; set; }
        [Required(ErrorMessage = "Campo Nome é obrigatório!", AllowEmptyStrings = false)]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Campo Email é obrigatório!", AllowEmptyStrings = false)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Campo Senha é obrigatório!", AllowEmptyStrings = false)]
        public string Senha { get; set; }
        [Display(Name = "Senha Atual")]
        public string SenhaAtual { get; set; }
        [Display(Name = "Nova Senha")]
        public string NovaSenha { get; set; }
        public IFormFile? Foto { get; set; }
        public string? NomeFoto { get; set; }
        public bool Ativo { get; set; }
        public List<Endereco>? Enderecos { get; set; }

        public Usuario()
        {
            this.Id = 0;
            this.Tipo = new TipoUsuario(2, "Conta Pessoal");
            this.Nome = string.Empty;
            this.Email = string.Empty;
            this.Senha = string.Empty;
            this.SenhaAtual = string.Empty;
            this.NovaSenha = string.Empty;
        }

        public Usuario(TipoUsuario tipo, string nome, string email, string senha)
        {
            this.Tipo = tipo;
            this.Nome = nome;
            this.Email = email;
            this.Senha = senha;
            this.SenhaAtual = string.Empty;
            this.NovaSenha = string.Empty;
        }
    }
}
