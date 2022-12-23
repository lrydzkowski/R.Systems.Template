# R.Systems.Template

ASP.NET Core 7 Web API template project, created as a base for other projects.

## .NET CLI Template

## Install

### From directory

```powershell
dotnet new --install C:\repo\Private\R.Systems\R.Systems.Template
```

### From NuGet

```powershell
dotnet new -i R.Systems.Template
```

## Uninstall

### From directory

```powershell
dotnet new --uninstall C:\repo\Private\R.Systems\R.Systems.Template
```

### From NuGet

```powershell
dotnet new --uninstall R.Systems.Template
```

## Show list of templates

```powershell
dotnet new --list
```

## Create a new project from the template

```powershell
dotnet new r-systems-clean-architecture --name R.Systems.Lexica --projectNameKebabCase r-systems-lexica
```

## NuGet package

### Create

```powershell
nuget pack .\.nuspec -NoDefaultExcludes
```

### Publish

```powershell
nuget setApiKey <your_API_key>
nuget push R.Systems.Template.1.0.0.nupkg -Source https://api.nuget.org/v3/index.json
```

## Docker

### Build image

```powershell
docker build -t r-systems-template -f .\R.Systems.Template.Api.Web\Dockerfile .
```

### Run container

```powershell
docker run -it --rm -p 8080:80 r-systems-template /bin/bash
```
