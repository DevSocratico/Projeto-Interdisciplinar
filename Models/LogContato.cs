using System.ComponentModel.DataAnnotations;

namespace LocFarma.Models
{
    public class LogContato
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório!")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O email é obrigatório!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "A mensagem é obrigatória!", AllowEmptyStrings = false)]
        [StringLength(500)]
        public string Mensagem { get; set; }
        [Display(Name = "Data Envio")]
        public DateTime DataEnvio { get; set; }

        public LogContato()
        {
            this.Nome = string.Empty;
            this.Email = string.Empty;
            this.Mensagem = string.Empty;
            this.DataEnvio = DateTime.Now;
        }

        public LogContato(string nome, string email, string mensagem)
        {
            this.Nome = nome;
            this.Email = email;
            this.Mensagem = mensagem;
            this.DataEnvio = DateTime.Now;
        }
    }
}
