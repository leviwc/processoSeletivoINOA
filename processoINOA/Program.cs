using System;
using System.Configuration;
using YahooFinanceApi;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace processoINOA {
  class Program {
    static async Task Main(string[] args) {
      List<Stock> stocks = ParseArgs(args);
      EmailSender emailSender = new EmailSender(ConfigurationManager.AppSettings.Get("emailDeEnvio"), ConfigurationManager.AppSettings.Get("senhaDeEnvio"), ConfigurationManager.AppSettings.Get("emailRecipiente"));
      while (true) {
        try {
          foreach (var stock in stocks) {
            var securities = await Yahoo.Symbols(stock.name).Fields(Field.Symbol, Field.RegularMarketPrice, Field.FiftyTwoWeekHigh).QueryAsync();
            var apiResult = securities[stock.name];
            decimal price = Convert.ToDecimal(apiResult.RegularMarketPrice);
            if (price <= stock.redLine && stock.state != -1) {
              stock.state = -1;
              emailSender.SendEmail(true, stock.name);
            } else if (price >= stock.blueLine && stock.state != 1) {
              stock.state = 1;
              emailSender.SendEmail(false, stock.name);
            } else if (price > stock.redLine && price < stock.blueLine) {
              stock.state = 0;
            }
          }

        } catch (Exception ex) {
          Console.WriteLine(ex.Message);
        }
        System.Threading.Thread.Sleep(2000);
      }
    }
    static List<Stock> ParseArgs(string[] args) {
      var stocks = new List<Stock>();
      for (int i = 1, j = 0; i < args.Length; i += 3, j++) {
        Stock auxiliar = new Stock();
        auxiliar.state = 0;
        auxiliar.name = args[i];
        auxiliar.redLine = Convert.ToDecimal(args[i + 1]);
        auxiliar.blueLine = Convert.ToDecimal(args[i + 2]);
        auxiliar.Format();
        if (!auxiliar.Validate()) {
          Console.WriteLine("Erro na entrada das ações");
          Environment.Exit(1);
        }
        stocks.Add(auxiliar);
      }
      return stocks;
    }
  }

}
