using System;
using System.Threading;
using YahooFinanceApi;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;
namespace processoINOA {
  class Program {
    private static readonly CancellationTokenSource canToken = new CancellationTokenSource();
    static async Task Main(string[] args) {
      DotNetEnv.Env.Load("config.env");
      List<Stock> stockList = ParseArgs(args);
      while (!canToken.IsCancellationRequested) {
        List<Task> taskList = new List<Task>();
        foreach (var stock in stockList) {
          taskList.Add(Task.Run(async () => {
            var emailSenderInTask = new EmailSender(Variables.emailDeEnvio, Variables.senhaDeEnvio, Variables.emailRecipiente);
            var securities = await Yahoo.Symbols(stock.name).Fields(Field.Symbol, Field.RegularMarketPrice, Field.FiftyTwoWeekHigh).QueryAsync();
            var apiResult = securities[stock.name];
            decimal price = Convert.ToDecimal(apiResult.RegularMarketPrice);
            if (price <= stock.redLine && stock.currentState != Stock.State.buy) {
              await emailSenderInTask.SendEmail(true, stock.name);
              Console.WriteLine("paralelizou");
              stock.currentState = Stock.State.buy;
            } else if (price >= stock.blueLine && stock.currentState != Stock.State.sell) {
              await emailSenderInTask.SendEmail(false, stock.name);
              stock.currentState = Stock.State.sell;
            } else if (price > stock.redLine && price < stock.blueLine) {
              stock.currentState = Stock.State.nothing;
            }
          }));
          if (taskList.Count == Variables.bucketListMaxSize) {
            try {
              await Task.WhenAll(taskList);
            } catch { }
            foreach (var task in taskList) {
              if (task.IsFaulted) {
                foreach (var e in task.Exception.InnerExceptions) {
                  Console.WriteLine(e.Message);
                }
              }
            }
            taskList.Clear();
          }
        }
        if (taskList.Count != 0) {
          try {
            await Task.WhenAll(taskList);
          } catch { }
          foreach (var task in taskList) {
            if (task.IsFaulted) {
              foreach (var e in task.Exception.InnerExceptions) {
                Console.WriteLine(e.Message);
              }
            }
          }
          taskList.Clear();
        }
        await Task.Delay(2000);
      }
    }
    static List<Stock> ParseArgs(string[] args) {
      var stockList = new List<Stock>();
      for (int i = 0, j = 0; i + 2 < args.Length; i += 3, j++) {
        Stock stock = new Stock();
        stock.currentState = Stock.State.nothing;
        stock.name = args[i];
        try {
          stock.blueLine = Convert.ToDecimal(args[i + 1], new CultureInfo("en-GB"));
          stock.redLine = Convert.ToDecimal(args[i + 2], new CultureInfo("en-GB"));
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

