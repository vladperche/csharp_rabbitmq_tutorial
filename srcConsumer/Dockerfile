# BUILD
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY ./myRabbitConsumer.sln ./
COPY ./myRabbitConsumer.Worker/myRabbitConsumer.Worker.csproj myRabbitConsumer.Worker/
COPY ./myRabbitConsumer.Entities/myRabbitConsumer.Entities.csproj myRabbitConsumer.Entities/
COPY ./myRabbitConsumer.Services/myRabbitConsumer.Services.csproj myRabbitConsumer.Services/
COPY ./myRabbitConsumer.CrossCutting/myRabbitConsumer.CrossCutting.csproj myRabbitConsumer.CrossCutting/

RUN dotnet restore myRabbitConsumer.Worker/myRabbitConsumer.Worker.csproj

COPY ./ .

RUN dotnet publish myRabbitConsumer.Worker/myRabbitConsumer.Worker.csproj -c Release -o /app/publish

# RUNTIME
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV DOTNET_ENVIRONMENT=DockerTest

RUN mkdir -p /app/logs

VOLUME ["/app/logs"]

ENTRYPOINT ["dotnet", "myRabbitConsumer.Worker.dll"]
