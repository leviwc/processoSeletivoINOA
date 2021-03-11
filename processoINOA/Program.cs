using System;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Collections.Specialized;
namespace processoINOA
{
    class Program
    {
        static void Main(string[] args)
        {

        }
        static void SendEmail(bool buyStock)
        {
            try
            {
                string emailDeEnvio = ConfigurationManager.AppSettings.Get("emailDeEnvio");
                string senhaDeEnvio = ConfigurationManager.AppSettings.Get("senhaDeEnvio");
                string emailRecipiente = ConfigurationManager.AppSettings.Get("emailRecipiente");
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    UseDefaultCredentials = false,
                    Port = 587,
                    Credentials = new NetworkCredential(emailDeEnvio, senhaDeEnvio),
                    EnableSsl = true,
                };
                smtpClient.Send(emailDeEnvio, emailRecipiente, "Alerta de preço da ação", (buyStock ? "Compre a ação" : "Venda a ação"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

}
