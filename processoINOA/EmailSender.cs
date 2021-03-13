using System.Net;
using System.Net.Mail;

namespace processoINOA {
  public class EmailSender {
    public SmtpClient smtpClient;
    string emailRecipiente;
    string emailDeEnvio;
    public EmailSender(string emailDeEnvio, string senhaDeEnvio, string emailRecipiente) {
      this.smtpClient = new SmtpClient("smtp.gmail.com") {
        UseDefaultCredentials = false,
        Port = 587,
        Credentials = new NetworkCredential(emailDeEnvio, senhaDeEnvio),
        EnableSsl = true,
      };
      this.emailRecipiente = emailRecipiente;
      this.emailDeEnvio = emailDeEnvio;
    }
    public void SendEmail(bool buyStock, string stockName) {
      this.smtpClient.Send(emailDeEnvio, emailRecipiente, "Alerta de ações", (buyStock ? "Buy the stock " : "Sell the stock ") + stockName);
    }
  }
}