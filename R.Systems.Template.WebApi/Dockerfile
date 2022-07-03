#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["R.Systems.Template.Core/R.Systems.Template.Core.csproj", "R.Systems.Template.Core/"]
RUN dotnet restore "R.Systems.Template.Core/R.Systems.Template.Core.csproj"
COPY ["R.Systems.Template.WebApi/R.Systems.Template.WebApi.csproj", "R.Systems.Template.WebApi/"]
RUN dotnet restore "R.Systems.Template.WebApi/R.Systems.Template.WebApi.csproj"
COPY . .
WORKDIR "/src/R.Systems.Template.WebApi"
RUN dotnet build "R.Systems.Template.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "R.Systems.Template.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "R.Systems.Template.WebApi.dll"]