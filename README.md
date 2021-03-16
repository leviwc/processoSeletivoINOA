# processoSeletivoINOA
Esse projeto foi feito para o desafio de estagio da Inoa.

## Introdução
O objetivo desse projeto é fazer uma aplicação de console capaz de monitorar uma ação da BOVESPA e mandar um email quando o preço dela ficar abaixo de um limite informado aconselhando a compra, e acima de um limite informado, aconselhando a venda. Como feature extra, essa aplicação pode monitorar multiplas ações.

## Instruções de instalação

### Requisitos necessarios:
Para que o programa rode, será necessario criar um arquivo na pasta processoINOA config.env, com as informações descritas no config.env.example. Após isso coloque-o na pasta onde está o .exe

```
git clone https://github.com/leviwc/processoSeletivoINOA.git
cd inoa
cd processoINOA
dotnet restore
```
e para gerar um arquivo .exe use:


```
dotnet publish -c Debug -r win10-x64(ou seu codigo do seu sistema operacional)
```

### Como usar:
Para rodar o .exe entre na pasta inoa/processoINOA/bin/Debug/net5.0/win10-x64 e rode o programa no console com as ações que deseja monitorar, exemplo:

```
./processoINOA.exe BBAS3 40.32 20.30 PETR4 22.67 22.59 BBDC3 22 30.14
```






