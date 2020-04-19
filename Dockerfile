FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app
COPY ./NuGet.Config ./Microservice.sln ./
COPY ./Microservice.Api/*.csproj ./Microservice.Api/
COPY ./Microservice.Db/*.csproj ./Microservice.Db/
COPY ./Microservice.Logic/*.csproj ./Microservice.Logic/
COPY ./Microservice.HangfireBackgroundJobServer/*.csproj ./Microservice.HangfireBackgroundJobServer/
COPY ./Microservice.RabbitMessageBroker/*.csproj ./Microservice.RabbitMessageBroker/
COPY ./Microservice.RabbitMessageBroker.Integration.Tests/*.csproj ./Microservice.RabbitMessageBroker.Integration.Tests/
COPY ./Microservice.Api.Integration.Tests/*.csproj ./Microservice.Api.Integration.Tests/
RUN dotnet restore

COPY . ./
RUN dotnet build -c Release
ENV ASPNETCORE_ENVIRONMENT IntegrationTesting
RUN dotnet test -c Release --no-build
ENV ASPNETCORE_ENVIRONMENT Development
RUN dotnet publish -c Release -o published --no-restore --no-build ./Microservice.Api

# Runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 as runtime
EXPOSE 80
EXPOSE 443
WORKDIR /app
COPY --from=build /app/published . 
CMD ["dotnet", "Microservice.Api.dll"]

ARG VERSION=0.0.1
ENV BUILDNUMBER=${VERSION}
