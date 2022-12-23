FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ./*.sln ./
COPY ./*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./${file%.*}/ && mv $file ./${file%.*}/; done
RUN dotnet restore
COPY . .
RUN dotnet build "R.Systems.Template.Api.Web/R.Systems.Template.Api.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "R.Systems.Template.Api.Web/R.Systems.Template.Api.Web.csproj" -c Release -o /app/publish

FROM base AS final
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "R.Systems.Template.Api.Web.dll"]