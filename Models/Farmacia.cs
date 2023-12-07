using System.ComponentModel.DataAnnotations;

namespace LocFarma.Models
{
    public class Farmacia : Usuario
    {
        [Required(ErrorMessage = "Campo Nome é obrigatório!", AllowEmptyStrings = false)]
        [Display(Name = "Farmácia")]
        public new string Nome { get; set; }
        [Required]
        public string CNPJ { get; set; }
        public List<Produto>? Produtos { get; set; }

        public Farmacia() : base()
        {
            this.Nome = string.Empty;
            this.CNPJ = string.Empty;
        }

        public Farmacia(TipoUsuario tipo, string nome, string email, string senha, string cnpj)
            : base(tipo, nome, email, senha)
        {
            this.Nome = nome;
            this.CNPJ = cnpj;
        }
    }
}
