# MyRestfulApp-NET
## Instrucciones para iniciar la API localmente por primera vez

#### Ejecutar el siguiente comando para crear el contenedor de Docker con la imagen de SQL Server
```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=MyR3stfulApp_" -p 1433:1433 --name myrestfulapp-sqlserver -h myrestfulapp-sqlserver -d mcr.microsoft.com/mssql/server:2019-latest
```

#### Iniciar el contenedor de Docker con el siguiente comando
```bash
docker start {IDENTIFICADOR DEL CONTENEDOR}
```

#### Si no conocemos el identificador del contenedor, revisar el listado con el siguiente comando para obtener el dato
```bash
docker ps -a
```

### Es importante que el contenedor esté funcionando o la API no podrá conectarse con la base de datos.

#### Con el siguiente comando, comprobamos el estado de los contenedores
```bash
docker ps
```

#### Si no aparece el contenedor, verificar si está detenido con el siguiente comando
```bash
docker ps -a
```

#### Veremos algo así en la terminal
```
CONTAINER ID   IMAGE                                                        COMMAND                  CREATED         STATUS                      PORTS                                       NAMES
9c7584a99d47   mcr.microsoft.com/mssql/server:2019-latest                   "/opt/mssql/bin/perm…"   18 hours ago    Up 3 hours                  0.0.0.0:1433->1433/tcp, :::1433->1433/tcp   myrestfulapp-sqlserver
```

#### 9c7584a99d47 es el identificador del contenedor que necesitamos.

#### La API se encarga de crear el schema correspondiente al hacer el build y run de la aplicación.
```bash
dotnet run
```

### No será necesario volver a crear el contenedor de Docker a menos que lo eliminemos.
### Cada vez que queramos iniciar la API nuevamente, revisar el estado del contenedor e iniciarlo en caso de ser necesario.

### La API al iniciar en modo local (IsLocalEnvironment = true) y si la tabla de usuarios esta vacia, generara datos de prueba.
### Por lo tanto no es necesario correr ningun script para generar usuarios para probar.

# Documentación de servicios

#### Para acceder a la documentación de servicios, incorpore Swagger a la API para acceder a una documentación completa.
#### Localmente, la API funciona en el puerto 7221. Con la siguiente URL: [https://localhost:7221/index.html](https://localhost:7221/index.html) accedemos a la vista de Swagger.
#### Aquí podemos ver parámetros esperados, posibles respuestas de los servicios e incluso probar los servicios desde esta vista.

# Estructura del proyecto
## Patrón de Diseño en Capas
El patrón de diseño en capas es una técnica arquitectónica que organiza el código en distintos niveles de abstracción, cada uno representando una responsabilidad específica dentro del sistema. Esta organización facilita la separación de preocupaciones y mejora la mantenibilidad, escalabilidad y la reutilización del código.

## Estructura del proyecto

``` bash
├── MyRestfulApp_NET_API
│   ├── appsettings.Development.json
│   ├── appsettings.json
│   ├── Common
│   │   ├── CreatePasswordHashHelper.cs
│   │   ├── Exceptions
│   │   │   └── ExternalApiExeption.cs
│   │   ├── Middlewares
│   │   │   └── CustomExceptionHandlerMiddleware.cs
│   │   └── ValidationErrorResponse.cs
│   ├── Controllers
│   │   ├── BaseController.cs
│   │   ├── BusquedaController.cs
│   │   ├── CurrenciesController.cs
│   │   ├── PaisesController.cs
│   │   └── UsersController.cs
│   ├── Data
│   ├── Domain
│   │   ├── HttpClients
│   │   │   └── IMercadoLibreClient.cs
│   │   ├── Models
│   │   │   └── User.cs
│   │   ├── Repositories
│   │   │   └── IUserRepository.cs
│   │   └── Services
│   │       ├── Communication
│   │       │   ├── BasicMessageResponse.cs
│   │       │   └── UpdateUserMessageResponse.cs
│   │       ├── IBusquedaServices.cs
│   │       ├── ICurrencyService.cs
│   │       ├── IPaisesServices.cs
│   │       └── IUserService.cs
│   ├── HttpClients
│   │   ├── MercadoLibreClient.cs
│   │   └── Resources
│   │       ├── MercadoLibreCountriesResources.cs
│   │       ├── MercadoLibreCurrencyResources.cs
│   │       └── MercadoLibreSearchResources.cs
│   ├── Migrations
│   │   ├── 20240419230423_CreateUsers.cs
│   │   ├── 20240419230423_CreateUsers.Designer.cs
│   │   └── AppDbContextModelSnapshot.cs
│   ├── MyRestfulApp_NET_API.csproj
│   ├── Persistence
│   │   ├── Contexts
│   │   │   ├── AppDbContext.cs
│   │   │   └── DbInitializer.cs
│   │   └── Repositories
│   │       └── UserRepository.cs
│   ├── Program.cs
│   ├── Properties
│   │   └── launchSettings.json
│   ├── Resources
│   │   ├── CurrencyResource.cs
│   │   ├── UserResource.cs
│   │   ├── UserSaveResource.cs
│   │   └── UserUpdateResource.cs
│   ├── Services
│   │   ├── BusquedaService.cs
│   │   ├── CurrencyService.cs
│   │   ├── PaisesService.cs
│   │   └── UserService.cs
│   └── Startup.cs
├── MyRestfulApp-NET-Proyect.sln
├── MyRestfulApp_NET_TEST
│   ├── appsettings.json
│   ├── Common
│   │   └── Middlewares
│   │       └── CustomExceptionHandlerMiddlewareTest.cs
│   ├── Controllers
│   │   ├── BusquedaControllerTest.cs
│   │   ├── CurrenciesControllerTest.cs
│   │   ├── PaisesControllerTest.cs
│   │   └── UsersControllerTest.cs
│   ├── HttpClients
│   │   └── MercadoLibreClientTest.cs
│   ├── MyRestfulApp_NET_TEST.csproj
│   ├── Persistence
│   │   ├── DbContextMocker.cs
│   │   └── Repositories
│   │       └── UserRespositoryTest.cs
│   ├── Services
│   │   ├── BusquedaServiceTest.cs
│   │   ├── CurrencyServiceTest.cs
│   │   ├── PaisesServiceTest.cs
│   │   └── UserServiceTest.cs
│   └── StartupTest.cs
└── README.md
```
## Para ejecutar los tests con coverage ejecutar el siguiente comando desde la terminal

### Linux
``` bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput=./lcov.info /p:Exclude="[*]MyRestfulApp_NET_API.Migrations.*%2c[xunit.*]*%2c[*]ServiceReference1.*"
```

### Windows
``` shell
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput=lcov.info /p:Exclude="[MyRestfulApp_NET_API.Migrations.*]*%2c[xunit.*]*%2c[ServiceReference1.*]*"
```
### Ejemplo
![Captura de pantalla de 2024-04-21 17-46-58](https://github.com/JoaquinDamianSeita/MyRestfulApp-NET/assets/74945252/1dca34f9-d35d-4275-8564-6009ab06adcd)
