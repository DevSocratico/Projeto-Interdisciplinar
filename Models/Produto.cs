using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocFarma.Models
{
    public class Produto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório!")]
        [Display(Name = "Produto")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O preço é obrigatório!")]
        [Display(Name = "Preço")]
        public decimal Preco { get; set; }
        [Required(ErrorMessage = "A quantidade é obrigatória!")]
        public int Quantidade { get; set; }
        [Display(Name = "Descrição")]
        [StringLength(500)]
        public string? Descricao { get; set; }
        public IFormFile? Foto { get; set; }
        public string? NomeFoto { get; set; }

        public Produto()
        {
            this.Id = 0;
            this.Nome = string.Empty;
            this.Preco = 0;
            this.Descricao = null;
            this.Foto = null;
            this.NomeFoto = null;
        }

        public Produto(string nome, decimal preco, string? descricao, IFormFile? foto, string? nomeFoto)
        {
            this.Nome = nome;
            this.Preco = preco;
            this.Descricao = descricao;
            this.Foto = foto;
            this.NomeFoto = nomeFoto;
        }
    }
}
