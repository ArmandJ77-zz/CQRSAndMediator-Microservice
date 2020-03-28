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
- Swagger (at some point)

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
- Extend DB layer with infrastructure to facilitate (long)LastModifiedBy,(UTCDateTime)LastModifiedDate,(long)CreatedBy,(UTCDateTime)CreatedDate on resource update and create
- Extend DB layer with infrastructure to facilitate soft deletes i.e Set IsDeleted prop on a record to true
- Extend Db layer with infrastructure to exclude IsDeleted reccords from db queries i.e add a query filter
