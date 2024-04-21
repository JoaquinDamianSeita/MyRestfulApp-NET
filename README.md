# MyRestfulApp-NET
Desafío Nubimetrics para el área de BackEnd

**1 - Introducción**

En el área de backend de [Nubimetrics](https://www.nubimetrics.com/) tenemos múltiples desafíos. Los principales son crear APIs para que distintas áreas puedan consumir esa información.

**Para poder cubrir las responsabilidades de un perfil junior pensamos que deberías poder tener un entendimiento básico sobre el uso de patrones de diseños, testing y buenas prácticas de desarrollo.**

Te queremos proponer un trabajo sobre la creación y consumo de diferentes endpoints para así poder entender tu nivel de conocimiento de .Net como así también tu compromiso en el desarrollo de una solución con un **código claro, limpio y ordenado**. Lo que es una de nuestras prioridades. Así también nos gustaría encontrar en tu solución una correcta **separación de responsabilidades a** la hora de proponer una solución.

**Forma de Entrega:**

- Link a [Github](https://github.com/) con el código fuente.
- Script para generar la base de datos y luego popular en el caso que sea necesario.
- Todos los desafíos deben estar en el mismo repositorio.

**Framework permitidos:** Justificar porqué

Debido a que estamos tratando de comprender tus conocimientos en .Net te pedimos por favor que **NO** uses el [software development kit (SDK)](https://github.com/mercadolibre/dotnet-sdk) para .NET de MercadoLibre.

**Bonus:**

- Crear [unit test](https://en.wikipedia.org/wiki/Unit_testing)
- Implementación de patrones de diseño y explicación, en la documentación, de por qué es relevante ese patrón para la solución propuesta.

**2 - Desafíos**

**Desafío 1:** Crear endpoint "países"

Crear una aplicación del tipo REST Web Api y exponer el endpoint que llamaremos "Países"

**Este endpoint deberá ser llamado los parámetros del país. Por Ejemplo:**
- [http://localhost:8080/MyRestfulApp/Paises/PAIS](https://localhost:8080/MyRestfulApp/Paises/PAIS)

**El País (PAIS) debería ser: "AR", "BR", "CO". La totalidad de los sites se pueden encontrar en la siguiente documentación de mercadolibre:** [API classified Locations de Mercado Libre](https://api.mercadolibre.com/classified_locations)

**Consideraciones:**
- Para los parámetros "BR" y "CO" hacer que el endpoint de una respuesta que sea **"error 401 unauthorized de http"**
- Para el parámetro "AR" el endpoint deberá consumir la información del país desde el servicio externo: [API classified Locations de Mercado Libre - Countries: AR](https://api.mercadolibre.com/classified_locations/countries/AR)

**Desafío 2:** Crear endpoint "búsqueda":

Crear una aplicación del tipo REST Web Api exponer el endpoint que llamaremos "búsqueda"

**El endpoint deberá ser llamado por un parámetro de tipo string. Por Ejemplo**
- [http://localhost:8080/MyRestfulApp/busqueda/TERMINO](https://localhost:8080/MyRestfulApp/busqueda/TERMINO)

**El término (TERMINO), por ejemplo Iphone, deberá ser lo que se debe buscar. El endpoint deberá consumir la información del siguiente servicio externo:** [Resultado de la api search para el término 'Iphone' en Mercado Libre](https://api.mercadolibre.com/sites/MLA/search?q=iphone)

**Consideraciones:**
- Este endpoint deberá devolver el objeto como lo devuelve el servicio externo.
- En el array "results" solo incluir los Fields:
  - id
  - site_id
  - title
  - price
  - seller.id
  - permalink

**Desafío 3:** Crear endpoint "usuarios":

Crear una aplicación del tipo REST Web Api exponer el siguiente endpoint:

**Crear un endpoint "usuarios" que devuelva todos los usuarios de una tabla "User" con los campos, id, nombre, apellido, email, password. Para ello crear la base de datos, la tabla y popularla con un script.**

Crear los endpoints necesarios en la misma api rest para el ABM de usuarios.

**Desafío 4:** Consumir datos de endpoints

Al ser ejecutado el proyecto se debe poder consumir datos de los siguientes endpoints:

- Api Currencies de Mercado Libre: [Link a la API](https://api.mercadolibre.com/currencies)
- Api Currency Conversion de Mercado Libre: [Link a la API](https://api.mercadolibre.com/currency_conversions)

**El objetivo es que se logre almacenar en disco un json con la estructura que devuelve el endpoint [Api Currencies de Mercado Libre](https://api.mercadolibre.com/currencies). Adicionalmente se debe incluir una nueva property "todolar" con el resultado del endpoint [Api Currency Conversion de Mercado Libre](https://api.mercadolibre.com/currency_conversions). Tené en cuenta que: El endpoint [Currency Conversion](https://api.mercadolibre.com/currency_conversions/toars) toma como parámetro en "from" el id de moneda correspondiente a un país. Id que es devuelto por el endpoint [Currencies](https://api.mercadolibre.com/currencies) - Para más información podés consultar la documentación de [Mercado Libre](https://developers.mercadolibre.com.ar/es_ar/currencies-and-conversions).**

**Adicionalmente la misma aplicación tiene que almacenar en disco un archivo csv con cada uno de los resultados obtenidos de "currency_conversions", es decir debe almacenar sólo los resultados obtenidos de la property "ratio" (Ej: 0.0147275,0.013651,0.727565).**


# Instrucciones para iniciar la API localmente por primera vez

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
