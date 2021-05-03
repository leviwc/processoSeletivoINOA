using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace processoINOA {
  public class EmailSender {
    public SmtpClient smtpClient { get; set; }
    public string emailRecipiente { get; set; }
    public string emailDeEnvio { get; set; }
    public EmailSender(string emailDeEnvio, string senhaDeEnvio, string emailRecipiente) {
      this.smtpClient = new SmtpClient(Variables.smtpUrl) {
        UseDefaultCredentials = false,
        Port = Variables.smtpPort,
        Credentials = new NetworkCredential(emailDeEnvio, senhaDeEnvio),
        EnableSsl = true,
      };
      this.emailRecipiente = emailRecipiente;
      this.emailDeEnvio = emailDeEnvio;
    }
    public async Task SendEmail(bool buyStock, string stockName) {
      await this.smtpClient.SendMailAsync(emailDeEnvio, emailRecipiente, "Alerta de ações", (buyStock ? "Compre a ação " : "Venda a ação ") + stockName);
    }
  }
}