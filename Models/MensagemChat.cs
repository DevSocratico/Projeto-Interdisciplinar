namespace LocFarma.Models
{
    public class MensagemChat
    {
        public int Id { get; set; }
        public Usuario Remetente { get; set; }
        public Usuario Destinatario { get; set; }
        public string Mensagem { get; set; }
        public DateTime DataEnvio { get; set; }

        public MensagemChat(Usuario remetente, Usuario destinatario, string mensagem)
        {
            this.Remetente = remetente;
            this.Destinatario = destinatario;
            this.Mensagem = mensagem;
            this.DataEnvio = DateTime.Now;
        }
    }
}
