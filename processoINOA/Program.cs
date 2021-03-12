using System;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using YahooFinanceApi;
using System.Threading.Tasks;

namespace processoINOA
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string stockName = args[1] + ".SA";
            decimal redLine = Convert.ToDecimal(args[2]), blueLine = Convert.ToDecimal(args[3]);
            int previusStateOfShare = 0; // -1 is bellow red line, 0 is in the middle and 1 is above blue line
            while (true)
            {
                try
                {
                    var securities = await Yahoo.Symbols(stockName).Fields(Field.Symbol, Field.RegularMarketPrice, Field.FiftyTwoWeekHigh).QueryAsync();
                    var stock = securities[stockName];
                    decimal price = Convert.ToDecimal(stock.RegularMarketPrice);
                    if (price <= redLine && previusStateOfShare != -1)
                    {
                        previusStateOfShare = -1;
                        SendEmail(true, stockName);
                    }
                    else if (price >= blueLine && previusStateOfShare != 1)
                    {
                        previusStateOfShare = 1;
                        SendEmail(false, stockName);
                    }
                    else if (price > redLine && price < blueLine)
                    {
                        previusStateOfShare = 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                System.Threading.Thread.Sleep(1000);
            }
        }
        static void SendEmail(bool buyStock, string stockName)
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
                smtpClient.Send(emailDeEnvio, emailRecipiente, "Alerta de preço da ação", (buyStock ? "Compre a ação " : "Venda a ação ") + stockName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

}
