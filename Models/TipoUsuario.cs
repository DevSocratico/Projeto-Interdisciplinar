namespace LocFarma.Models
{
    public class TipoUsuario
    {
        public int Id { get; set; }
        public string Tipo { get; set; }

        public TipoUsuario()
        {
            this.Id = 0;
            this.Tipo = string.Empty;
        }

        public TipoUsuario(int id, string tipo)
        {
            this.Id = id;
            this.Tipo = tipo;
        }
    }
}
