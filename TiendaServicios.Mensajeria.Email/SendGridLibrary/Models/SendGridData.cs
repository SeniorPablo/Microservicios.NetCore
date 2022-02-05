namespace TiendaServicios.Mensajeria.Email.SendGridLibrary.Models
{
    public class SendGridData
    {
        public string SendGridApiKey { get; set; }
        public string EmailReceiver { get; set; }
        public string ReceiverName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
