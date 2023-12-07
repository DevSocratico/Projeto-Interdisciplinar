using System.ComponentModel.DataAnnotations;

namespace LocFarma.Models
{
    public class Cidade
    {
        public int Id { get; set; }
        [Display(Name = "Cidade")]
        public string Nome { get; set; }
        public UF UF { get; set; }

        public Cidade()
        {
            this.Id = 0;
            this.Nome = string.Empty;
            this.UF = new UF();
        }

        public Cidade(int id, string nome, UF uf)
        {
            this.Id = id;
            this.Nome = nome;
            this.UF = uf;
        }
    }
}
