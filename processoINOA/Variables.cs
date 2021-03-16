using System;
namespace processoINOA {
  public static class Variables {
    public static string emailDeEnvio = Environment.GetEnvironmentVariable("EMAIL_DE_ENVIO");
    public static string senhaDeEnvio = Environment.GetEnvironmentVariable("SENHA_DE_ENVIO");
    public static string emailRecipiente = Environment.GetEnvironmentVariable("EMAIL_RECIPIENTE");
    public static string smtpUrl = Environment.GetEnvironmentVariable("SMTP_URL");
    public static int smtpPort = Convert.ToInt16(Environment.GetEnvironmentVariable("SMTP_PORT"));
    public static int bucketListMaxSize = Convert.ToInt16(Environment.GetEnvironmentVariable("BUCKET_SIZE"));

  }
}