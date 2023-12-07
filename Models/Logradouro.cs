using System.ComponentModel.DataAnnotations;

namespace LocFarma.Models
{
    public class Logradouro
    {
        public int Id { get; set; }
        [Display(Name = "Logradouro")]
        public string Nome { get; set; }
        public Bairro Bairro { get; set; }

        public Logradouro()
        {
            this.Id = 0;
            this.Nome = string.Empty;
            this.Bairro = new Bairro();
        }

        public Logradouro(int id, string nome, Bairro bairro)
        {
            this.Id = id;
            this.Nome = nome;
            this.Bairro = bairro;
        }
    }
}
