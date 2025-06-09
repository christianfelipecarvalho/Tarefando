# Usar a imagem base do .NET SDK para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Definir o diretório de trabalho
WORKDIR /app

# Copiar e restaurar dependências (otimização de cache)
COPY *.sln ./
COPY Application/*.csproj Application/
COPY Domain/*.csproj Domain/
COPY Infrastructure/*.csproj Infrastructure/
COPY Tarefando/*.csproj Tarefando/
RUN dotnet restore

# Copiar o restante do código e compilar
COPY . .
RUN dotnet publish Tarefando/Tarefando.csproj -c Release -o /app/publish

# Usar a imagem otimizada de runtime do .NET 8
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

# Copiar arquivos publicados
COPY --from=build /app/publish .

# Expor as portas
EXPOSE 8080
EXPOSE 8081

# Definir o comando de entrada
ENTRYPOINT ["dotnet", "Tarefando.dll"]