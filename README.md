# Teste Backend



## Descrição da Solução

Este projeto é uma solução de backend desenvolvida em C# e segue a arquitetura CQRS (Command Query Responsibility Segregation). E foi utilizado o seguinte repositorio como base: https://github.com/omid-ahmadpour/CleanArchitecture-Template/blob/main/README.md

Ele utiliza o Entity Framework (Code-First, as migrations sempre serão aplicadas toda vez que o projeto foi iniciado) para interagir com o banco de dados SqlServer e o MongoDB.Driver para interagir com MongoDB. Foi utilizado também Redis apenas para demonstração de uso de cache.

Para importação dos dados via os arquivos de filmes, pokemons e "meus pokemons" foi utilizado um BackgroundService, para não bloquear a aplicação principal. Foi criado um endpoint para pegar o STATUS da importação, esse controle foi implementado utilizando o MongoDB também.

Foi criado duas funções de importação dos dados dentro do BackgroundSerivce (ProcessExcelFileBackgroundService):
- **ProcessFileMovies**: Faz a importação simples dos filmes com o excel fornecido, nao faz tratamento das colunas como identificadores.
- **ProcessFileMoviesV2**: Faz a importação melhorando a importação e fazendo tratamento das colunas como identificadores.

Para demonstração de filtros, foi criado um endpoint na parte dos filmes onde, ele faz uma busca generica de texto em todos os campos, no caso do MongoDB.
Para demonstração de filtros com SqlServer foi utilizando o conceito de Specification Pattern, onde é possível criar filtros complexos e reutilizáveis.

Para controle de importação dos "Meus Pokemons" foi utilizado o login (JWT) para identificar o usuario. A mesma logica quando ele busca os "Meus Pokemons"

## Tecnologias Envolvidas

- **C#**: A linguagem de programação principal usada para desenvolver a solução.
- **Entity Framework**: Um ORM (Object-Relational Mapper) que permite interagir com o banco de dados usando objetos C#.
- **.NET 8.0**: A plataforma de desenvolvimento usada para construir a aplicação.
- **CQRS**: A arquitetura utilizada para separar as operações de leitura e gravação.

## Banco de Dados

SQL Server para Pokemons
MongoDB para gerencias a importação de arquivos e para todos os filmes.
Redis para demonstração de uso de cache

## Como Executar o Projeto

1. Clone o repositório para a sua máquina local.
2. Foi criado um docker-compose para subir o projeto e suas respectivas dependencias (SqlServer, Redis e MongoDB)
3. Caso queira rodar a aplicação sem utilizar docker, precisa ajustar os aqruivos de appsettings dentro do projeto da API
4. docker-compose up 

