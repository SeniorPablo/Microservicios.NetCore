using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using TiendaServicios.Mensajeria.Email.SendGridLibrary.Interface;

namespace TiendaServicios.Mensajeria.Email.SendGridLibrary.Implement
{
    public class SendGridData : ISendGridData
    {
        public async Task<(bool result, string errorMessage)> SendEmail(Models.SendGridData data)
        {
            try
            {
                var sendGridClient = new SendGridClient(data.SendGridApiKey);
                var receiver = new EmailAddress(data.EmailReceiver, data.EmailReceiver);
                var emailTitle = data.Title;
                var sender = new EmailAddress("juangonzalez251088@correo.itm.edu.co", "Juan González");
                var content = data.Content;

                var objMessage = MailHelper.CreateSingleEmail(sender, receiver, emailTitle, content, content);
                await sendGridClient.SendEmailAsync(objMessage);

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }

        }
    }
}
