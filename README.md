# processoSeletivoINOA
Esse projeto foi feito para o desafio de estagio da Inoa.

## Introdução
O objetivo desse projeto é fazer uma aplicação de console capaz de monitorar uma ação da BOVESPA e mandar um email quando o preço dela ficar abaixo de um limite informado aconselhando a compra, e acima de um limite informado, aconselhando a venda. Como feature extra, essa aplicação pode monitorar multiplas ações.

## Instruções de instalação

### Requisitos necessarios:
Clone o projeto em uma pasta com:
```
git clone https://github.com/leviwc/processoSeletivoINOA.git
```
Para que o programa rode, será necessario criar um arquivo config.env que está na pasta processoSeletivoINOA/processoINOA, com as informações descritas no config.env.example ,sendo elas sem '<' e  '>'.
```
EMAIL_DE_ENVIO = "<email de login/envio>"
SENHA_DE_ENVIO = "<senha do login/envio>"
EMAIL_RECIPIENTE = "<email de destino>"
SMTP_URL = "<url do smtp server>"
SMTP_PORT = "<port do smtp>"
BUCKET_SIZE = "3" #valor da quantiade limite de chamadas ao mesmo tempo na api yahoo
```

### Como usar:
Sera necessario colocar o arquivo config.env na mesma pasta do .exe

Se seu sistema operacional for windows 10 podera rodar o .exe entrando na pasta processoSeletivoINOA/processoINOA/bin/Release/net5.0/win10-x64/publish com
```
cd processoSeletivoINOA/processoINOA/bin/Release/net5.0/win10-x64/publish
```

e rodando o programa no console com as ações que deseja monitorar, exemplo:

```
./processoINOA.exe BBAS3 40.32 20.30 PETR4 22.67 22.59 BBDC3 30.14 20
```

Para os passos abaixo, será necessario instalar o .NET SDK 5.0.201, mais informações em https://dotnet.microsoft.com/download/dotnet/5.0

Também é possivel gerar um novo arquivo .exe para outros sistemas operacionais com:

```
dotnet publish -c Release -r <RID do seu sistema operacional>
```
mais informações sobre RIDS suportadas em https://docs.microsoft.com/en-us/dotnet/core/rid-catalog

Também é possivel rodar a aplicação na pasta processoSeletivoINOA/processoINOA com dotnet run, mas a pasta config.env tera que ficar em processoSeletivoINOA/processoINOA  ex:
```
dotnet run BBAS3 40.32 20.30 PETR4 22.67 22.59 BBDC3 30.14 20
```

## Dados utilizados
Os dados das ações da B3 foram buscados da api YahooFinance com a ajuda da biblioteca YahooFinanceApi versão 2.1.2








