using System.ComponentModel.DataAnnotations;

namespace LocFarma.Models
{
    public class UF
    {
        public int Id { get; set; }
        [Display(Name = "UF")]
        public string Sigla { get; set; }

        public UF()
        {
            this.Id = 0;
            this.Sigla = string.Empty;
        }

        public UF(int id, string sigla)
        {
            this.Id = id;
            this.Sigla = sigla;
        }
    }
}
