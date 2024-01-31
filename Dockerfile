#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

EXPOSE 5000
EXPOSE 5001

ENV ASPNETCORE_URLS=http://+:5000

ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Common/TesteBackend.Common.csproj", "src/Common/"]
COPY ["src/Web/Api/TesteBackend.Api.csproj", "src/Web/Api/"]
COPY ["src/Web/ApiFramework/TesteBackend.ApiFramework.csproj", "src/Web/ApiFramework/"]
COPY ["src/Infrastructure/Persistance/TesteBackend.Persistence.csproj", "src/Infrastructure/Persistance/"]
COPY ["src/Core/Domain/TesteBackend.Domain.csproj", "src/Core/Domain/"]
COPY ["src/Core/Application/TesteBackend.Application.csproj", "src/Core/Application/"]
RUN dotnet restore "src/Web/Api/TesteBackend.Api.csproj"
COPY . .
WORKDIR "/src/src/Web/Api/"
RUN dotnet build "TesteBackend.Api.csproj" -c Release -o /app/build

# Adicione esta linha para executar os testes
RUN dotnet test --no-restore --verbosity normal

FROM build AS publish
RUN dotnet publish "TesteBackend.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TesteBackend.Api.dll"]