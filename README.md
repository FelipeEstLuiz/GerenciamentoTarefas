# Tecnologias utilizadas
* Dot Net 9
* Sql Server 2022
* Dapper
* React para o front
* Testes com xUnit
* NSubstitute para Mock
* MediatR
* FluentValidation
* Versionamento de rotas no netcore
* Swagger

## Para rodar a aplicacao basca inserir no appsettings o servidor do banco, o Database, o usuario e a senha
 > ao subir em ambiente de desenvolvimento, ele ira criar o database se nao existir e irá criar todas as tabelas

Passos para rodar o projeto

* Configurar o banco de dados no appsettings.json do Application.Api e appsettings.Test.json do projeto Tests
* Executar o comando dotnet run dentro do projeto Application.Api para iniciar a aplicacao, ira abrir no endereço https://localhost:7053/swagger/index.html
* Executar o comando npm run dev dentro do projeto gerenciamento-tarefas-react para iniciar a aplicacao front end, irá abrir provavelmente no endereco http://localhost:5173/
* Para executar os testes, rodar o comando dotnet test dentro do projeto Tests

> Os scripts do banco rodam automatico, porem eles se encontram no diretorio GerenciamentoTarefas\src\Infrastructure\Scripts