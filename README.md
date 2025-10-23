# FormacaoCSharp.Bookstore

## 📘 Descrição

API REST construída em **.NET 8** para gerenciar livros de uma livraria, armazenando dados em memória. Possui CRUD completo, validações, herança entre classes de domínio e documentação via **Swagger**.

---

## Funcionalidades

* Criar um livro (`POST /api/books`)
* Listar todos os livros (`GET /api/books`)
* Buscar um livro pelo ID (`GET /api/books/{id}`)
* Atualizar um livro (`PUT /api/books/{id}`)
* Excluir um livro (`DELETE /api/books/{id}`)

---

## Tecnologias utilizadas

* .NET 8.0 (ASP.NET Core Web API)
* Swagger (Swashbuckle.AspNetCore)
* Injeção de dependência (DI)
* Validações com DataAnnotations

---

## Estrutura do projeto

```
FormacaoCSharp.Bookstore/
├─ Communication/
|  └─Requests/
│    ├─ RequestCreateBookJson.cs
│    └─ RequestUpdateBookJson.cs
├─ Controllers/
│  └─ BooksController.cs
├─ Models/
│  ├─ Book.cs
│  └─ EntityBase.cs
├─ Repositories/
│  ├─ IBookRepository.cs
│  └─ InMemoryBookRepository.cs
├─ appsettings.json
├─ Program.cs
└─ README.md
```

---

## Como rodar o projeto

### Pré-requisitos

* Instalar o **.NET 8 SDK**: [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)

Verifique a instalação:

```bash
dotnet --version
```

### Passos para executar

1. Clone o repositório ou copie o código para uma pasta local:

```bash
git clone https://github.com/alessandroibes/FormacaoCSharp.Bookstore.git
cd FormacaoCSharp.Bookstore
```

2. Restaure as dependências:

```bash
dotnet restore
```

3. Execute a aplicação:

```bash
dotnet run
```

4. Acesse o Swagger UI para testar:

```
https://localhost:7169/swagger
```

> Se estiver usando HTTP, acesse `http://localhost:7169/swagger`

---

## Validações e Regras de Negócio

* **title** e **author**: obrigatórios (2–120 caracteres) e combinação única.
* **genre**: obrigatório, deve estar na lista de gêneros válidos (`ficção`, `romance`, `mistério`, `fantasia`, `biografia`, `tecnologia`).
* **price** e **stock**: obrigatórios e ≥ 0.
* **id**: gerado automaticamente (GUID).
* **CreatedAt** e **UpdatedAt**: preenchidos automaticamente.

---

## Códigos de status HTTP

| Código | Descrição                              |
| ------ | -------------------------------------- |
| 200    | Sucesso em consultas e atualizações    |
| 201    | Livro criado com sucesso               |
| 204    | Exclusão bem-sucedida (sem retorno)    |
| 400    | Requisição inválida / dados incorretos |
| 404    | Livro não encontrado                   |
| 409    | Conflito (título e autor duplicados)   |
| 500    | Erro interno do servidor               |

---

## Exemplos de uso (via `curl`)

### Criar um livro

```bash
curl -X POST "https://localhost:5001/api/books" \
     -H "Content-Type: application/json" \
     -d '{"title":"O Senhor dos Livros","author":"Fulano","genre":"fantasia","price":39.9,"stock":10}' -k
```

### Listar todos os livros

```bash
curl https://localhost:5001/api/books -k
```

### Buscar por ID

```bash
curl https://localhost:5001/api/books/{id} -k
```

### Atualizar

```bash
curl -X PUT "https://localhost:5001/api/books/{id}" \
     -H "Content-Type: application/json" \
     -d '{"title":"Novo Título","author":"Fulano","genre":"ficção","price":49.9,"stock":15}' -k
```

### Excluir

```bash
curl -X DELETE https://localhost:5001/api/books/{id} -k
```

---

## Observações

* O repositório é **in-memory** — os dados são perdidos ao encerrar o app.
* Para persistir, basta criar uma implementação de `IBookRepository` com Entity Framework Core e registrar no `Program.cs`.

---

## Licença

Este projeto é de uso livre para fins de aprendizado e portfólio.
