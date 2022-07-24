# R.Systems.Template

ASP.NET Core 6 Web API template project, created as a base for other projects.

## Docker

### Build image

```powershell
docker build -t r-systems-template -f .\R.Systems.Template.WebApi\Dockerfile .
```

### Run container

```powershell
docker run -it --rm -p 8080:80 r-systems-template /bin/bash
```
