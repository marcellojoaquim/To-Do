# To-Do
API REST de gerenciamento do tarefas.

## Tecnologias

- DotNet 8
- Entity Framework
- SQLite

## Como Executar o projeto

- Clone o projeto
- Navegue até o diretório /ToDoList
- Execute o comando dotnet run

<p>A API está deisponível em http://localhost:5128 </p>
<p>O Swagger estará deisponível em http://localhost:5128/swagger/index.html </p>

## Como executar os testes

- Com o pejeto clonado
- Navegue até o diretório /ToDoList.Tests
- Execute o comando dotnet test

## Testes via Postman

- Com o projeto em execução
- Acesse a collection no diretório /Util
- Importe a collection no postman

## API

### API Tasks
- URL Base: /api/tasks

### API Usuarios
- URL Base: /api/usuarios

## Regras de negócio

- Uma vez que a tarefa for concluída não poderá mais ser modificada. Retorna 409, mais coerênte em minha opnião.
- PageSize no máximo 50.
- Prioridade das tarefas devem está entre 1 e 3.
- Título deve conter de 3 a 80 caracteres.
- Descricao deve conter de 0 a 400 caracteres.
- Criar um nova tarefa deve enviar um usuário existente.