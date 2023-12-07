using System.ComponentModel.DataAnnotations;

namespace LocFarma.Models
{
    public class Endereco
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O nome do local é obrigatório!")]
        [Display(Name = "Local")]
        public string NomeLocal { get; set; }
        [Required(ErrorMessage = "O CEP é obrigatório!")]
        public string CEP { get; set; }
        public Logradouro Logradouro { get; set; }
        [Required(ErrorMessage = "O número é obrigatório!")]
        [Display(Name = "Número")]
        public string Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }

        public Endereco()
        {
            this.NomeLocal = string.Empty;
            this.CEP = string.Empty;
            this.Logradouro = new Logradouro();
            this.Numero = string.Empty;
            this.Complemento = null;
            this.Latitude = null;
            this.Longitude = null;
        }

        public Endereco(string cep, Logradouro logradouro, string numero, string? complemento, string? latitude, string? longitude)
        {
            this.NomeLocal = string.Empty;
            this.CEP = cep;
            this.Logradouro = logradouro;
            this.Numero = numero;
            this.Complemento = complemento;
            this.Latitude = latitude;
            this.Longitude = longitude;
        }
    }
}
