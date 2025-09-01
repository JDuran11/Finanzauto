# Proyecto Finanzauto

## Descripción general
El proyecto Finanzauto es una aplicación empresarial modular y escalable diseñada para gestionar operaciones críticas del negocio como la gestión de productos, manejo de clientes, procesamiento de pedidos y administración financiera. Está construido siguiendo los principios de Arquitectura Basada en Capas combinada con Clean Architecture, garantizando una clara separación de responsabilidades, alta mantenibilidad, escalabilidad y facilidad de pruebas.

## Arquitectura de la solución
La solución implementa una arquitectura de 4 capas bien definidas, donde cada capa tiene responsabilidades específicas y mantiene dependencias unidireccionales hacia las capas inferiores:

### 1. **Finanzauto.Domain**
   - **Propósito**: Representa el núcleo del dominio de la aplicación, definiendo las entidades, DTOs (Objetos de Transferencia de Datos) y lógica compartida.
   - **Componentes clave**:
     - **Entidades**: Modelos que representan las tablas de la base de datos y encapsulan la lógica de negocio principal. Ejemplos:
       - `Category.cs`: Representa las categorías de productos.
       - `Customer.cs`: Modela los clientes de la aplicación.
       - `Order.cs`: Define los pedidos realizados por los clientes.
       - `Product.cs`: Representa los productos disponibles en el sistema.
     - **DTOS (Data Transfer Objects)**: Objetos diseñados para transferir datos entre capas de la aplicación. Ejemplos:
       - `AuthRequest.cs` y `AuthResponse.cs`: Manejan la autenticación de usuarios.
       - `CategoryDTO.cs`: Transfiere datos relacionados con categorías.
       - `OrderDTO.cs` y `OrderCreateDTO.cs`: Gestionan la transferencia de datos de pedidos.
       - `ProductDTO.cs` y `ProductCreateDTO.cs`: Manejan la transferencia de datos de productos.
     - **Common/BaseEntity.cs**: Clase base que proporciona propiedades comunes para todas las entidades, como `Id`, `CreatedDate`, y `UpdatedDate`.
   - **Estructura de carpetas**:
     - **Entities/**: Contiene las entidades principales del dominio.
     - **DTOS/**: Contiene los DTOs organizados por funcionalidad (por ejemplo, `Auth`, `Category`, `Order`, etc.).
     - **Common/**: Contiene clases compartidas como `BaseEntity.cs`.
   - **Función**: Esta capa define el modelo de dominio y asegura que la lógica de negocio esté centralizada y desacoplada de otras capas. Es fundamental para garantizar la consistencia de los datos y la integridad


### 2. **Finanzauto.Persistence**
   - **Propósito**: Maneja el acceso y la persistencia de datos, proporcionando una capa de abstracción entre la base de datos y las demás capas de la aplicación.
   - **Componentes clave**:
     - **Data/AppDbContext.cs**: 
       - Clase principal que actúa como puente entre las entidades del dominio y la base de datos.
       - Configura las tablas y relaciones utilizando Entity Framework Core.
     - **TableConfiguration/**:
       - Contiene configuraciones específicas para cada entidad, asegurando que las propiedades y relaciones se mapeen correctamente en la base de datos.
       - Ejemplos:
         - `CategoryConfiguration.cs`: Configura la tabla de categorías.
         - `OrderConfiguration.cs`: Define las relaciones y restricciones de la tabla de pedidos.
     - **Interfaces/**:
       - Define contratos para los repositorios, asegurando que las operaciones de acceso a datos sean consistentes y reutilizables.
       - Ejemplos:
         - `ICategoryRepository.cs`: Contrato para operaciones relacionadas con categorías.
         - `IOrderRepository.cs`: Contrato para manejar datos de pedidos.
     - **Repositories/**:
       - Implementaciones de los contratos definidos en `Interfaces/`.
       - Ejemplos:
         - `CategoryRepository.cs`: Implementa las operaciones de acceso a datos para categorías.
         - `OrderRepository.cs`: Gestiona las operaciones relacionadas con pedidos.
     - **Migrations/**:
       - Contiene las migraciones generadas por Entity Framework Core para gestionar los cambios en el esquema de la base de datos.
       - Ejemplo:
         - `20250830194013_Init.cs`: Migración inicial que crea las tablas base.
       - `AppDbContextModelSnapshot.cs`: Representa el estado actual del modelo de datos.
     - **DependencyInjection.cs**:
       - Configura la inyección de dependencias para los repositorios y el `AppDbContext`, permitiendo que otras capas accedan a los servicios de persistencia.
   - **Estructura de carpetas**:
     - **Data/**: Contiene el `AppDbContext` y configuraciones de tablas.
     - **Interfaces/**: Define los contratos para los repositorios.
     - **Repositories/**: Implementaciones de los contratos de acceso a datos.
     - **Migrations/**: Migraciones de la base de datos generadas por Entity Framework Core.
   - **Función**: Esta capa abstrae la lógica de acceso a datos, permitiendo que las demás capas interactúen con la base de datos de manera desacoplada. Además, gestiona la configuración del esquema de la base de datos y asegura la consistencia de los datos.

### 3. **Finanzauto.Application**
   - **Propósito**: Contiene la lógica de la aplicación y define los servicios que implementan las reglas de negocio. Esta capa actúa como intermediaria entre la capa de persistencia y la capa de presentación (WebApi), asegurando que las operaciones de negocio se ejecuten correctamente.
   - **Componentes clave**:
     - **Interfaces/**:
       - Define contratos para los servicios de la aplicación, asegurando que las implementaciones sean consistentes y reutilizables.
       - Ejemplos:
         - `IAuthService.cs`: Contrato para la autenticación de usuarios.
         - `IProductService.cs`: Contrato para la gestión de productos.
         - `IOrderService.cs`: Contrato para el manejo de pedidos.
     - **Services/**:
       - Implementaciones de las interfaces definidas en la carpeta `Interfaces/`.
       - Ejemplos:
         - `AuthService.cs`: Implementa la lógica de autenticación.
         - `ProductService.cs`: Gestiona las operaciones relacionadas con productos.
         - `OrderService.cs`: Implementa la lógica de negocio para pedidos.
     - **Common/ProductGenerator.cs**:
       - Clase auxiliar que genera datos de prueba o realiza operaciones comunes relacionadas con productos.
     - **DependencyInjection.cs**:
       - Configura la inyección de dependencias para los servicios de la aplicación, permitiendo que otras capas accedan a ellos de manera desacoplada.
   - **Estructura de carpetas**:
     - **Interfaces/**: Define los contratos para los servicios.
     - **Services/**: Contiene las implementaciones de los servicios.
     - **Common/**: Contiene clases auxiliares o utilitarias.
   - **Función**: Esta capa encapsula la lógica de negocio y asegura que las reglas de la aplicación se implementen correctamente. También proporciona servicios reutilizables que pueden ser consumidos por la capa de presentación (WebApi) o cualquier otra capa que lo requiera.

### 4. **Finanzauto.WebApi**
   - **Propósito**: Actúa como el punto de entrada de la aplicación, exponiendo una API RESTful para interactuar con las capas internas del sistema. Esta capa se encarga de recibir solicitudes HTTP, procesarlas y devolver respuestas adecuadas.
   - **Componentes clave**:
     - **Controladores**:
       - Manejan las solicitudes HTTP y delegan la lógica de negocio a los servicios de la capa de aplicación.
       - Ejemplos:
         - `AuthController.cs`: Gestiona las operaciones de autenticación y autorización.
         - `ProductController.cs`: Maneja las operaciones relacionadas con productos.
         - `OrderController.cs`: Procesa las solicitudes relacionadas con pedidos.
     - **DTOs/**:
       - Contiene objetos diseñados para recibir datos de entrada desde las solicitudes HTTP.
       - Ejemplos:
         - `CategoryCreateRequest.cs`: DTO para crear nuevas categorías.
         - `EmployeeCreateRequest.cs`: DTO para registrar nuevos empleados.
     - **Archivos de configuración**:
       - `appsettings.json`: Contiene configuraciones generales de la aplicación, como cadenas de conexión y claves de API.
       - `appsettings.Development.json`: Configuraciones específicas para el entorno de desarrollo.
       - `launchSettings.json`: Configura los perfiles de inicio para ejecutar la aplicación en diferentes entornos.
     - **Program.cs**:
       - Configura los servicios y middleware necesarios para la aplicación, como la autenticación, el manejo de excepciones y el enrutamiento.
   - **Dependencias externas**:
     - **Microsoft.AspNetCore.Authentication.JwtBearer**: Maneja la autenticación basada en JWT (JSON Web Tokens).
     - **CloudinaryDotNet**: Proporciona integración con Cloudinary para la gestión de archivos multimedia.
     - **BCrypt.Net-Next**: Permite el hashing seguro de contraseñas.
     - **Bogus**: Generador de datos ficticios para pruebas y desarrollo.
     - **Humanizer**: Facilita la manipulación y visualización de datos como fechas y números.
   - **Estructura de carpetas**:
     - **Controllers/**: Contiene los controladores que manejan las solicitudes HTTP.
     - **DTOs/**: Contiene los DTOs para las solicitudes HTTP.
     - **Properties/**: Contiene configuraciones específicas del proyecto.
   - **Función**: Esta capa expone los servicios de la aplicación a través de endpoints HTTP, permitiendo que los clientes (como aplicaciones web o móviles) interactúen con el sistema. También gestiona la autenticación, autorización y validación de datos de entrada.

### 5. **Finanzauto.Tests**
   - **Propósito**: Este proyecto contiene las pruebas unitarias e integrales para garantizar la calidad y el correcto funcionamiento de las diferentes capas de la aplicación. Las pruebas están diseñadas para validar la lógica de negocio, la interacción entre componentes y la integración con servicios externos.

   - **Componentes clave**:
     - **Unit/**:
       - Contiene las pruebas unitarias que validan la lógica de negocio de los servicios de la capa `Finanzauto.Application`.
       - Ejemplos:
         - `CategoryServiceTests.cs`: Pruebas unitarias para el servicio de categorías.
         - `ProductServiceTests.cs`: Pruebas unitarias para el servicio de productos.
     - **Integration/**:
       - Contiene las pruebas de integración que validan la interacción entre múltiples capas o servicios.
       - Ejemplo:
         - `ProductServiceIntegrationTest.cs`: Pruebas de integración para el servicio de productos, verificando su interacción con la capa de persistencia.
     - **Dependencias externas**:
       - **xUnit**: Framework de pruebas utilizado para escribir y ejecutar las pruebas.
       - **Moq**: Biblioteca para crear mocks y simular dependencias en las pruebas unitarias.
       - **DotNet.Testcontainers**: Biblioteca para ejecutar contenedores Docker en pruebas de integración, útil para simular bases de datos o servicios externos.
       - **Entity Framework Core InMemory**: Proveedor de base de datos en memoria utilizado para pruebas unitarias, evitando la necesidad de una base de datos real.
       - **Bogus**: Generador de datos ficticios para pruebas.
       - **BCrypt.Net-Next**: Utilizado para validar el hashing de contraseñas en las pruebas.

   - **Estructura de carpetas**:
     - **Unit/**: Contiene las pruebas unitarias.
     - **Integration/**: Contiene las pruebas de integración.

   - **Ejecutar las pruebas**:
     - Para ejecutar todas las pruebas, abre una terminal en la raíz del proyecto y ejecuta el siguiente comando:
       ```bash
       dotnet test
       ```
       - Este comando ejecutará todas las pruebas definidas en el proyecto `Finanzauto.Tests` y mostrará un resumen de los resultados.


## Características clave
- **Diseño modular**: Cada capa es independiente y se enfoca en una responsabilidad específica.
- **Inyección de dependencias**: Garantiza bajo acoplamiento entre componentes.
- **Diseño orientado a entidades**: Las entidades de dominio representan la lógica de negocio principal.
- **Escalabilidad**: La arquitectura soporta futuras mejoras y escalado.

## Estructura de carpetas
- **Finanzauto.Application**: Lógica de aplicación e interfaces de servicios.
- **Finanzauto.Domain**: Modelos de dominio y DTOs.
- **Finanzauto.Persistence**: Acceso a datos y migraciones de base de datos.
- **Finanzauto.Infrastructure**: Servicios de infraestructura.
- **Finanzauto.WebApi**: Capa API y punto de entrada.

## Tecnologías utilizadas
- **.NET Core**: Framework para construir la aplicación.
- **Entity Framework Core**: ORM para acceso a datos.
- **Inyección de dependencias**: Contenedor DI integrado en .NET Core.

## Ejecución local

A continuación, se detalla el paso a paso para clonar, construir y ejecutar el proyecto Finanzauto localmente. Este tutorial incluye la configuración de la base de datos utilizando **Entity Framework Core** con el enfoque **Code First**.

### 1. Clonar el repositorio
1. Abre una terminal en tu equipo.
2. Clona el repositorio ejecutando el siguiente comando:
   ```bash
   git clone <URL_DEL_REPOSITORIO>
   ```
3. Navega al directorio del proyecto clonado:
   ```bash
   cd Finanzauto
   ```

### 2. Restaurar paquetes NuGet
   - Abre la solución en Visual Studio.
   - Ve a la consola del Administrador de Paquetes (Tools > NuGet Package Manager > Package Manager Console).
   - Ejecuta el siguiente comando para restaurar los paquetes:
     ```bash
     Update-Package -reinstall
     ```

### 3. Configurar la cadena de conexión
   - Abre el archivo `appsettings.json` en el proyecto `Finanzauto.WebApi`.
   - Actualiza la cadena de conexión `DefaultConnection` con los detalles de tu base de datos postgreSQL:
     ```json
       "ConnectionStrings": {
            "DefaultConnection": "Host=tuservidor;Port=5432;Database=Db_Finanzauto;Username=usuario;Password=contraseña"
        },
     ```

### 4. Crear y actualizar la base de datos
   - Abre la consola en la carpeta raíz del proyecto.
   - Ejecuta el siguiente comando para crear o actualizar la base de datos utilizando Entity Framework:
     ```bash
     dotnet ef database update --project Finanzauto.Persistence --startup-project Finanzauto.WebApi
     ```

### 5. Ejecutar el proyecto
   - Establece `Finanzauto.WebApi` como el proyecto de inicio.
   - Presiona `F5` o haz clic en "Iniciar depuración" para ejecutar el proyecto.
   - La API debería estar disponible en `https://localhost:7210` (o el puerto configurado).

### 6. Probar la API
Puedes probar los endpoints de varias formas:
   #### 1. Desde Swagger
   - Abre en tu navegador: `https://localhost:7210/swagger/index.html`
   - Allí verás todos los endpoints documentados y podrás ejecutarlos directamente desde la interfaz.
   #### 2. Desde Postman
   - Crea una nueva solicitud HTTP y usa la URL de tu endpoint, por ejemplo:
     ```http
     GET https://localhost:7210/api/Auth/login
     ```
 Asegúrate de que la API responda correctamente y devuelva los datos esperados.

## Ejecución en Docker

Para ejecutar el proyecto en un entorno Docker, sigue los pasos a continuación:

### 1. Configurar la cadena de conexión
1. Abre el archivo `appsettings.json` ubicado en el proyecto `Finanzauto.WebApi`.
2. Cambia la cadena de conexión `DefaultConnection` por la siguiente:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=db;Port=5432;Database=Db_Finanzauto;Username=postgres;Password=1234"
   }
   ```

### 2. Crear imágenes de Docker
   - Abre una terminal en la raíz del proyecto.
   - Ejecuta el siguiente comando para construir las imágenes de Docker
     ```bash
     docker-compose build
     ```

### 3. Ejecutar contenedores
   - Una vez que las imágenes se hayan construido correctamente, ejecuta el siguiente comando para iniciar los contenedores:
     ```bash
     docker-compose up
     ```
   - Esto levantará todos los servicios definidos en el archivo `docker-compose.yml`, incluyendo la base de datos y la API.

### 4. Probar la API en Docker
   - Al igual que en la ejecución local, puedes probar los endpoints de la API utilizando Swagger o Postman.
   - La diferencia es que ahora la API estará corriendo dentro de un contenedor Docker, y la base de datos estará en otro contenedor (según lo definido en `docker-compose.yml`).

## Escalabilidad horizontal en un entorno cloud

El diseño modular y basado en capas del proyecto Finanzauto permite escalar horizontalmente de manera eficiente en un entorno cloud. A continuación, detallo cómo se puede lograr esto:

1. **Despliegue en contenedores**:
   - La solución puede contenerizarse utilizando **Docker**, donde cada capa (WebApi, Application, etc.) se empaqueta en un contenedor independiente.
   - Estos contenedores pueden ser orquestados mediante **Kubernetes** o servicios equivalentes en la nube, como **Azure Kubernetes Service (AKS)** o **Amazon Elastic Kubernetes Service (EKS)**.

2. **Balanceo de carga**:
   - Se puede implementar un balanceador de carga (por ejemplo, **Azure Load Balancer** o **AWS Elastic Load Balancer**) para distribuir el tráfico entre múltiples instancias de la capa `Finanzauto.WebApi`.
   - Esto asegura que las solicitudes se distribuyan equitativamente, evitando la sobrecarga de una sola instancia.

3. **Autoescalado**:
   - Configurar políticas de autoescalado en el proveedor de nube (como **Azure Autoscale** o **AWS Auto Scaling Groups**) para aumentar o disminuir automáticamente el número de instancias según la carga de trabajo.

4. **Base de datos escalable**:
   - Utilizar una base de datos en la nube que soporte escalabilidad horizontal, como **Azure SQL Database**, **Amazon RDS**, o **Google Cloud SQL**.
   - Configurar réplicas de lectura para manejar grandes volúmenes de consultas de solo lectura, mientras que las escrituras se dirigen al nodo principal.

5. **Caché distribuido**:
   - Implementar un sistema de caché distribuido como **Redis** o **Memcached** para reducir la carga en la base de datos y mejorar el rendimiento.
   - Almacenar en caché datos frecuentemente consultados, como productos, categorías o configuraciones.

Con estas estrategias, el proyecto puede manejar un aumento en la carga de trabajo mientras mantiene un rendimiento óptimo y garantiza la disponibilidad del sistema.

## Ejecución de la API: Autenticación y uso de endpoints

### 1. Obtener un token de autenticación
Para consumir los endpoints protegidos de la API, primero es necesario obtener un token de autenticación. Esto se puede hacer utilizando el endpoint de inicio de sesión (`/api/Auth/login`) con las credenciales de prueba.

Ejecuta el siguiente comando en tu terminal para obtener el token:

```bash
curl -X 'POST' \
  'http://localhost:5000/api/Auth/login' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "username": "admin",
  "password": "admin"
}'
```
