using System;
using System.Net;
using System.Net.Mail;
using System.Configuration;
namespace processoINOA
{
    class Program
    {
        static void Main(string[] args)
        {

        }
        public void SendEmail(bool buyStock)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    UseDefaultCredentials = false,
                    Port = 587,
                    Credentials = new NetworkCredential("inoaprocessoseletivo@gmail.com", "inoa12345"),
                    EnableSsl = true,
                };
                smtpClient.Send("inoaprocessoseletivo@gmail.com", "leviwc3@hotmail.com", "Alerta de preço da ação", (buyStock ? "Compre a ação" : "Venda a ação"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

}
