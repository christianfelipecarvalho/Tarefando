# Usar a imagem base do .NET SDK para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Definir o diretório de trabalho
WORKDIR /app

# Copiar solution e arquivos de projeto (.csproj) — otimiza cache
COPY *.sln ./
COPY Application/*.csproj Application/
COPY Domain/*.csproj Domain/
COPY Infrastructure/*.csproj Infrastructure/
COPY Tarefando/*.csproj Tarefando/
COPY Tarefando.Tests/*.csproj Tarefando.Tests/

# Restaurar dependências
RUN dotnet restore

# Copiar o restante do código
COPY . .

# Compilar e publicar em Release
RUN dotnet publish Tarefando/Tarefando.csproj -c Release -o /app/publish

# Usar imagem de runtime do .NET 8
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Definir diretório de trabalho
WORKDIR /app

# Copiar arquivos publicados do build
COPY --from=build /app/publish .

# Expor as portas
EXPOSE 8080
EXPOSE 8081

# Definir o comando de entrada
ENTRYPOINT ["dotnet", "Tarefando.dll"]
