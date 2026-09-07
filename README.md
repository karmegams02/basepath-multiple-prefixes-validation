# Base Path Multiple Prefixes Validation

## Run the Applications

### WASM Application

```powershell
dotnet run --project .\InteractiveWebAssemblyApp\BasePathWebAssemblyValidation\BasePathWebAssemblyValidation\BasePathWebAssemblyValidation.csproj -c Release --urls http://localhost:5081
```

```powershell
dotnet run --project .\InteractiveWebAssemblyApp\PrefixProxy\PrefixProxy.csproj -c Release --urls http://localhost:5080
```

### SSR Application

```powershell
dotnet run --project .\BasePathSSRValidation\BasePathSSRValidation\BasePathSSRValidation.csproj -c Release --urls http://localhost:5081
```

```powershell
dotnet run --project .\BasePathSSRValidation\PrefixProxy\PrefixProxy.csproj -c Release --urls http://localhost:5080
```

### Interactive Server Application

```powershell
dotnet run --project .\InteractiveServerApp\BasePathValidation\TestApp\TestApp.csproj -c Release --urls http://localhost:5081
```

```powershell
dotnet run --project .\InteractiveServerApp\BasePathValidation\PrefixProxy\PrefixProxy.csproj -c Release --urls http://localhost:5080
```

## Published Output Test Steps

### WASM Applications

```powershell
dotnet publish .\InteractiveWebAssemblyApp\PrefixProxy\PrefixProxy.csproj -c Release -o artifacts\PrefixProxy
```

```powershell
Set-Location .\artifacts\PrefixProxy
dotnet .\PrefixProxy.dll --urls http://localhost:5080
Set-Location ..\..
```

```powershell
dotnet publish .\InteractiveWebAssemblyApp\BasePathWebAssemblyValidation\BasePathWebAssemblyValidation\BasePathWebAssemblyValidation.csproj -c Release -o artifacts\BasePathWebAssemblyValidation
```

```powershell
Set-Location .\artifacts\BasePathWebAssemblyValidation
dotnet .\BasePathWebAssemblyValidation.dll --urls http://localhost:5081
Set-Location ..\..
```

### SSR Applications

```powershell
dotnet publish .\BasePathSSRValidation\PrefixProxy\PrefixProxy.csproj -c Release -o artifacts\PrefixProxy
```

```powershell
Set-Location .\artifacts\PrefixProxy
dotnet .\PrefixProxy.dll --urls http://localhost:5080
Set-Location ..\..
```

```powershell
dotnet publish .\BasePathSSRValidation\BasePathSSRValidation\BasePathSSRValidation.csproj -c Release -o artifacts\BasePathSSRValidation
```

```powershell
Set-Location .\artifacts\BasePathSSRValidation
dotnet .\BasePathSSRValidation.dll --urls http://localhost:5081
Set-Location ..\..
```

### Interactive Server Applications

```powershell
dotnet publish .\InteractiveServerApp\BasePathValidation\PrefixProxy\PrefixProxy.csproj -c Release -o artifacts\PrefixProxy
```

```powershell
Set-Location .\artifacts\PrefixProxy
dotnet .\PrefixProxy.dll --urls http://localhost:5080
Set-Location ..\..
```

```powershell
dotnet publish .\InteractiveServerApp\BasePathValidation\TestApp\TestApp.csproj -c Release -o artifacts\TestApp
```

```powershell
Set-Location .\artifacts\TestApp
dotnet .\TestApp.dll --urls http://localhost:5081
Set-Location ..\..
```

## Docker Container Test

### WASM Commands for Container Test

```powershell
docker network create basepath-network

docker build -f .\ContainerAppTest\BasePathWasmContainerApp\BasePathWasmContainerApp\BasePathWasmContainerApp\Dockerfile -t basepath-containerapp .

docker run --rm -d --name wasm-server --network basepath-network -p 5081:8080 basepath-containerapp:latest

docker build -f .\ContainerAppTest\BasePathWasmContainerApp\PrefixProxy\Dockerfile -t basepath-proxy .

docker run --rm -d --name prefixproxy --network basepath-network -p 5080:8080 --env "ReverseProxy__Clusters__test-app__Destinations__destination1__Address=http://wasm-server:8080/" basepath-proxy:latest
```

### SSR Commands for Container Test

```powershell
docker network create basepath-network-ssr

docker build -f .\ContainerAppTest\BasePathSSRContainerApp\BasePathSSRContainerApp\Dockerfile -t basepath-ssrapp .

docker run --rm -d --name wasm-server --network basepath-network-ssr -p 5081:8080 basepath-ssrapp:latest

docker build -f .\ContainerAppTest\BasePathSSRContainerApp\PrefixProxy\Dockerfile -t basepath-proxy .

docker run --rm -d --name prefixproxy --network basepath-network-ssr -p 5080:8080 --env "ReverseProxy__Clusters__test-app__Destinations__destination1__Address=http://wasm-server:8080/" basepath-proxy:latest
```

### Interactive Server Commands for Container Test

```powershell
docker network create basepath-network-server

docker build -f .\ContainerAppTest\BasePathInteractiveServerContainerApp\BasePathInteractiveServerContainerApp\Dockerfile -t basepath-serverapp .

docker run --rm -d --name wasm-server --network basepath-network-server -p 5081:8080 basepath-serverapp:latest

docker build -f .\ContainerAppTest\BasePathInteractiveServerContainerApp\PrefixProxy\Dockerfile -t basepath-proxy .

docker run --rm -d --name prefixproxy --network basepath-network-server -p 5080:8080 --env "ReverseProxy__Clusters__test-app__Destinations__destination1__Address=http://wasm-server:8080/" basepath-proxy:latest
```
