![.NET Core](https://github.com/ArmandJ77/CQRSAndMediator-Microservice/workflows/.NET%20Core/badge.svg?branch=master)

# CRUD-CQRS-MEDIATOR-MICROSERVICE

This origionally started as a demo project for my medium article [Why and how I implemented CQRS and Mediator patterns in a microservice](https://medium.com/@armandjordaan6/why-and-how-i-implemented-cqrs-and-mediator-patterns-in-a-microservice-b07034592b6d). 

## PATTERNS 

- CQRS
- Mediator
- Layered architecture

## TECH STACK

- Dotnet core 3.1
- EF Core
- Postgres
- NUnit
- Docker
- Docker-Compose
- RabbitMQ (up next)
- Redis (on roadmap)
- Hangfire (on roadmap)
- Swagger

## Roadmap

Features planned for next development cycle:
- RabbitMQ support (Investigate if I can implement the infrastructure as an optional package) 

Future feature to include:
- Redis cache management on route level i.e cached responses
- Redis cache management for custom resources i.e custom cache managment accross multiple handlers
- Hangfire support for running background tasks using redis as a backing store
- Health checks to monitor service
- Swagger
- GraphQL (investigate trade offs)
