# processoSeletivoINOA
Esse projeto foi feito para o desafio de estagio da Inoa.

## Introdução
O objetivo desse projeto é fazer uma aplicação de console capaz de monitorar uma ação da BOVESPA e mandar um email quando o preço dela ficar abaixo de um limite informado aconselhando a compra, e acima de um limite informado, aconselhando a venda. Como feature extra, essa aplicação pode monitorar multiplas ações.

## Instruções de instalação

### Requisitos necessarios:
Para que o programa rode, será necessario criar um arquivo na pasta processoINOA config.env, com as informações descritas no config.env.example. Após isso coloque-o na pasta onde está o .exe

```
git clone https://github.com/leviwc/processoSeletivoINOA.git
cd processoSeletivoINOA/processoINOA
```
 para gerar um arquivo .exe use:


```
dotnet publish -c Debug -r <RID do seu sistema operacional>
```
mais informações sobre RIDS suportadas em https://docs.microsoft.com/en-us/dotnet/core/rid-catalog

### Como usar:
Para rodar o .exe entre na pasta processoSeletivoINOA/processoINOA/bin/Debug/net5.0/win10-x64/publish e rode o programa no console com as ações que deseja monitorar, exemplo:

```
./processoINOA.exe BBAS3 40.32 20.30 PETR4 22.67 22.59 BBDC3 30.14 20
```






