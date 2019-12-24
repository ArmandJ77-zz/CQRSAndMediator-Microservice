FROM microsoft/dotnet:sdk AS build
WORKDIR /app
COPY ./NuGet.Config ./Microservice.sln ./
COPY ./Microservice.Api/*.csproj ./Microservice.Api/
RUN dotnet restore

COPY . ./
RUN dotnet build -c Release
RUN dotnet test -c Release --no-build
RUN dotnet publish -c Release -o published --no-restore --no-build ./Microservice.Api

FROM microsoft/dotnet:aspnetcore-runtime as runtime
WORKDIR /app
COPY --from=build /app/Microservice.Api/published . 
CMD ["dotnet", "Microservice.Api.dll"]

ARG VERSION=0.0.1
ENV BUILDNUMBER=${VERSION}
