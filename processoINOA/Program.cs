using System;
using System.Configuration;
using YahooFinanceApi;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace processoINOA {
  class Program {
    static async Task Main(string[] args) {
      List<Stock> stockList = ParseArgs(args);
      EmailSender emailSender = new EmailSender(ConfigurationManager.AppSettings.Get("emailDeEnvio"), ConfigurationManager.AppSettings.Get("senhaDeEnvio"), ConfigurationManager.AppSettings.Get("emailRecipiente"));
      while (true) {
        List<Task> taskList = new List<Task>();
        int bucketListMaxSize = 3;
        foreach (var stock in stockList) {
          taskList.Add(Task.Run(async () => {
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
          }));
          if (taskList.Count == bucketListMaxSize) {
            try {
              await Task.WhenAll(taskList);
            } catch {
              Console.WriteLine("Problem reaching the api");
            }
            taskList.Clear();
          }
          if (taskList.Count != 0) {
            try {
              await Task.WhenAll(taskList);
            } catch {
              Console.WriteLine("Problem reaching the api");
            }
            taskList.Clear();
          }
          System.Threading.Thread.Sleep(2000);
        }
      }
      static List<Stock> ParseArgs(string[] args) {
        var stockList = new List<Stock>();
        for (int i = 1, j = 0; i + 2 < args.Length; i += 3, j++) {
          Stock stock = new Stock();
          stock.state = 0;
          stock.name = args[i];
          try {
            stock.redLine = Convert.ToDecimal(args[i + 1]);
            stock.blueLine = Convert.ToDecimal(args[i + 2]);
          } catch {
            Console.WriteLine("Formatação errada das ações");
            Environment.Exit(1);
          }
          stock.Format();
          if (!stock.Validate()) {
            Console.WriteLine("Erro na entrada da ação " + stock.name);
            Environment.Exit(1);
          }
          stockList.Add(stock);
        }
        return stockList;
      }
    }
  }
}
