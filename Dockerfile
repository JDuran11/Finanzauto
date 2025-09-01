# 1. Imagen SDK para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# 2. Copiar los archivos de proyectos y restaurar dependencias
COPY *.sln ./
COPY Finanzauto.WebApi/*.csproj ./Finanzauto.WebApi/
COPY Finanzauto.Application/*.csproj ./Finanzauto.Application/
COPY Finanzauto.Persistence/*.csproj ./Finanzauto.Persistence/
COPY Finanzauto.Domain/*.csproj ./Finanzauto.Domain/
COPY Finanzauto.Tests/*.csproj ./Finanzauto.Tests/
RUN dotnet restore

# 3. Copiar todo y publicar
COPY . ./
WORKDIR /app/Finanzauto.WebApi
RUN dotnet publish -c Release -o out

# 4. Imagen final runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/Finanzauto.WebApi/out ./
EXPOSE 5000
ENTRYPOINT ["dotnet", "Finanzauto.WebApi.dll"]
