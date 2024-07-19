# Ver https://aka.ms/customizecontainer para aprender cómo personalizar tu contenedor de depuración y cómo Visual Studio usa este Dockerfile para construir tus imágenes para una depuración más rápida.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Agregar las variables de entorno
ENV D_Urls="https://localhost:7230; http://localhost:5230"
ENV DApplication_Connection_Docker="Data Source=172.17.0.3,1433;Database=TEST;User Id=sa;Password=1QazxsW212*;MultipleActiveResultSets=true;"

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ApplicationServices.csproj", "."]
RUN dotnet restore "./ApplicationServices.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./ApplicationServices.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ApplicationServices.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApplicationServices-NEW.dll"]





