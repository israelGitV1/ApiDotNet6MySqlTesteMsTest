# Desenvolvimento Api com DotNet 6.0 
<p>
<img height="400" src="/Api/preview/preview.gif"/>
</p>

## Live Link

* ## https://apidotnet6mysqltestemstest.onrender.com/swagger
 * Login para gerar o Token.
    * #### "email": "adm@teste.com"
    * #### "senha": "string"

# Introdução

### Este projeto consiste em uma API mínima desenvolvida com .NET 6, utilizando o Entity Framework como ORM para interagir com o banco de dados MySQL. O sistema é composto por duas tabelas principais:
 * Administrador: Responsável pelo controle de autenticação e login dos usuários administradores.
 * Veículo: Responsável pelo cadastro e gerenciamento de informações relacionadas aos veículos.

### Principais Tecnologias e Funcionalidades:

 *  MySQL: Banco de dados relacional para armazenamento das tabelas Administrador e Veículo.
 *  Entity Framework: Ferramenta ORM para mapeamento e manipulação dos dados do banco.
 *  Autenticação JWT (Bearer Token): Implementação de segurança para proteger a API, com geração e validação de tokens JWT para acesso autorizado.
 *  Swagger (OpenAPI): Interface para documentação interativa e teste dos endpoints da API.

### Endpoints Planejados:

   #### Administrador:
   * Login: Validação de credenciais e geração de token JWT.
   * Gerenciamento de usuários (opcional).

   #### Veículo:
   * Cadastro de veículos.
   * Consulta, atualização e remoção de registros de veículos.

### Objetivos:

* Fornecer uma API simples, segura e eficiente para autenticação e gerenciamento de veículos.
* Facilitar a integração com sistemas externos por meio de endpoints bem documentados no Swagger.
* Demonstrar boas práticas de desenvolvimento utilizando .NET 6, EF Core e autenticação JWT.

Se precisar de ajuda com implementação ou ajustes, é só avisar!

## Technologies used

 * .net 6 
 * EntityFrameworkCore 6
 * Autenticação com JWT Bearer 6
 * SuwggreSwashbuckle.AspNetCore 6.5
 * Banco de dados MySql
 * Testes com MsTest

## Requirements to run:

- .Net6 

- Database Mysql 
* appsetting.json "na pasta Api e Test"
```
 // changes to your connection string
 "ConnectionStrings": {
    "MySql":"Server=sql10.freesqldatabase.com;Port=3306;Database=sql10757463;User=sql10757463;Password=plpsjWwCy1;SslMode=Preferred;"
  },
```

## Running Instructions

1. Clone the project:

```
 git clone https https://github.com/israelGitV1/ApiDotNet6MySqlTesteMsTest.git
```

2. Install the dependecies:

```
 Cd Api
```
```
 dotnet restore
```

3. Run the project:

```
 Cd Api
```
```
dotnet run
```

