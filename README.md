![.NET Core](https://github.com/ArmandJ77/CQRSAndMediator-Microservice/workflows/.NET%20Core/badge.svg?branch=master)

# MICROSERVICE TEMPLATE FOR CQRS AND MEDIATOR PATTERNS

## WHAT PROBLEM DOES THIS SOLVE

This origionally started as a demo project for my medium article [Why and how I implemented CQRS and Mediator patterns in a microservice](https://medium.com/@armandjordaan6/why-and-how-i-implemented-cqrs-and-mediator-patterns-in-a-microservice-b07034592b6d).

This repo focuses on providing a C# dotnet core 3.1 microservice template with all the boilerplate setup completed, which enables the developer to start implementing their domain asap without having to go through the new service setup and the teething issues that goes along with it.

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
- RabbitMQ
- Hangfire with Postgres as the backing store

## GETTING STARTED

### Docker

When Development/Testing

```
docker-compose -f docker-compose.testing.yml up -d
```

NOTE: You can change the db name in the Microservice.Db project remember to also update the connection settings in the Microservice.Api appsettings.json

## Roadmap

Features planned for next development cycle:

- Refactor RabbitMQ and Hangfire for a more streamlined implementation
- Move RabbitMQ Messagebroker implementation into a nuget package
- Move Hangfire implementation into a nuget package

Future features to include:

- Redis cache management on route level i.e cached responses
- Redis cache management for custom resources i.e custom cache managment accross multiple handlers
- Hangfire support for running background tasks using redis as a backing store
- Health checks to monitor service
- Swagger
- GraphQL (investigate trade offs)
- Extend DB layer with infrastructure to facilitate (long)LastModifiedBy,(UTCDateTime)LastModifiedDate,(long)CreatedBy,(UTCDateTime)CreatedDate on resource update and create
- Extend DB layer with infrastructure to facilitate soft deletes i.e Set IsDeleted prop on a record to true
- Extend Db layer with infrastructure to exclude IsDeleted reccords from db queries i.e add a query filter
- Hangfire support *DONE
- RabbitMQ support *DONE
