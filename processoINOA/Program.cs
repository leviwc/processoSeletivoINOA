using System;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using YahooFinanceApi;
using System.Threading.Tasks;

namespace processoINOA {
  class Program {
    static async Task Main(string[] args) {
      int quantityOfStocks = (args.Length - 1) / 3;
      Stock[] stocks = new Stock[quantityOfStocks];
      for (int i = 0; i < quantityOfStocks; i++) stocks[i] = new Stock();
      int[] previusStateOfShares = new int[quantityOfStocks];// -1 is bellow red line, 0 is in the middle and 1 is above blue line
      for (int i = 1, j = 0; i < args.Length; i += 3, j++) {
        previusStateOfShares[j] = 0;
        stocks[j].stockName = args[i] + ".SA";
        stocks[j].redLine = Convert.ToDecimal(args[i + 1]);
        stocks[j].blueLine = Convert.ToDecimal(args[i + 2]);
        if (stocks[j].redLine >= stocks[j].blueLine) {
          Console.Write("Invalid input, the redLine is above or equal the blueLine of stock " + stocks[j].stockName + '\n');
          return;
        }
      }
      while (true) {
        try {
          for (int i = 0; i < quantityOfStocks; i++) {
            var securities = await Yahoo.Symbols(stocks[i].stockName).Fields(Field.Symbol, Field.RegularMarketPrice, Field.FiftyTwoWeekHigh).QueryAsync();
            var stock = securities[stocks[i].stockName];
            decimal price = Convert.ToDecimal(stock.RegularMarketPrice);
            if (price <= stocks[i].redLine && previusStateOfShares[i] != -1) {
              previusStateOfShares[i] = -1;
              SendEmail(true, stocks[i].stockName);
            } else if (price >= stocks[i].blueLine && previusStateOfShares[i] != 1) {
              previusStateOfShares[i] = 1;
              SendEmail(false, stocks[i].stockName);
            } else if (price > stocks[i].redLine && price < stocks[i].blueLine) {
              previusStateOfShares[i] = 0;
            }
          }

        } catch (Exception ex) {
          Console.WriteLine(ex.Message);
        }
        System.Threading.Thread.Sleep(2000);
      }
    }
    static void SendEmail(bool buyStock, string stockName) {
      try {
        string emailDeEnvio = ConfigurationManager.AppSettings.Get("emailDeEnvio");
        string senhaDeEnvio = ConfigurationManager.AppSettings.Get("senhaDeEnvio");
        string emailRecipiente = ConfigurationManager.AppSettings.Get("emailRecipiente");
        var smtpClient = new SmtpClient("smtp.gmail.com") {
          UseDefaultCredentials = false,
          Port = 587,
          Credentials = new NetworkCredential(emailDeEnvio, senhaDeEnvio),
          EnableSsl = true,
        };
        smtpClient.Send(emailDeEnvio, emailRecipiente, "Alerta de preço da ação", (buyStock ? "Compre a ação " : "Venda a ação ") + stockName);
      } catch (Exception ex) {
        Console.WriteLine(ex.Message);
      }
    }
  }
  public class Stock {
    public string stockName;
    public decimal redLine;
    public decimal blueLine;
    public Stock() { }
  }
}
