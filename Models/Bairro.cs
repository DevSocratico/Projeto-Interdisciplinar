using System.ComponentModel.DataAnnotations;

namespace LocFarma.Models
{
    public class Bairro
    {
        public int Id { get; set; }
        [Display(Name = "Bairro")]
        public string Nome { get; set; }
        public Cidade Cidade { get; set; }

        public Bairro()
        {
            this.Id = 0;
            this.Nome = string.Empty;
            this.Cidade = new Cidade();
        }

        public Bairro(int id, string nome, Cidade cidade)
        {
            this.Id = id;
            this.Nome = nome;
            this.Cidade = cidade;
        }
    }
}
