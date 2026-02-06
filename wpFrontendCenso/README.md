# Relación entre proyectos

- Backend API: https://github.com/ricardoescamilla787/api-rest-dotnet-csharp-frontend
- Frontend Web: https://github.com/ricardoescamilla787/api-rest-dotnet-csharp-backend

# Frontend Web – Sistema de Censo y Gestión de Usuarios (ASP.NET Web Forms)

Aplicación web desarrollada en **ASP.NET Web Forms (C#)** que funciona como interfaz gráfica del sistema de censo,
gestión de usuarios y control de acceso.

Este frontend consume una **API REST desarrollada en ASP.NET Web API**, realizando operaciones de autenticación,
consulta y administración de usuarios mediante peticiones HTTP (GET, POST, PUT, DELETE).

---

## Tecnologías utilizadas

- **ASP.NET Web Forms (C#)**
- **.NET Framework 4.7.2**
- **Bootstrap**
- **JavaScript**
- **Newtonsoft.Json**
- **HttpClient**
- **Visual Studio Community**

---

## Arquitectura del proyecto

El proyecto sigue una arquitectura **cliente-servidor**, donde este frontend actúa como cliente que consume la API REST.

wpFrontendCenso/
│
├── App_Start/
│ ├── BundleConfig.cs
│ └── RouteConfig.cs
│
├── Models/
│ └── clsApiStatus.cs
│
├── bootstrap/
├── css/
├── imagenes/
│
├── wpAcceso.aspx
│ └── wpAcceso.aspx.cs
├── wpUsuarios.aspx
│ └── wpUsuarios.aspx.cs
│
├── Web.config
│ ├── Web.Debug.config
│ └── Web.Release.config
|
└── README.md

## Configuración del consumo de la API


La URL base del backend se configura en el archivo `Web.config`:

<appSettings>
  <add key="ApiBaseUrl" value="http://localhost:44322/" />
</appSettings>

## Funcionalidades principales

### Autenticación de usuarios

1. Pantalla de inicio de sesión (wpAcceso.aspx)
2. Validación de credenciales mediante el endpoint:
3. POST /full/usuario/spvalidaracceso
4. Manejo de sesión mediante Session

### Gestión de usuarios (CRUD)

Listar usuarios
Buscar usuarios por filtro
Registrar nuevos usuarios
Editar usuarios existentes
Eliminar usuarios

Todas las operaciones se realizan consumiendo la API REST mediante HttpClient.

### Consumo de la API REST

HttpClient client = new HttpClient();
HttpContent contenido = new StringContent(json, Encoding.UTF8, "application/json");
HttpResponseMessage response = await client.PostAsync(urlApi, contenido);

### Ejecución del proyecto

1. Clonar el repositorio
2. Abrir la solución en Visual Studio Community
3. Verificar la URL del backend en Web.config
4. Ejecutar el proyecto
5. Acceder desde el navegador
6. Iniciar sesión con un usuario válido registrado en la base de datos

## Notas adicionales

El frontend depende completamente de la disponibilidad de la API REST.
Diseñado para ejecutarse inicialmente en entorno local, con posibilidad de despliegue en IIS o servidor en la nube.
Puede ser desplegado en IIS en conjunto con el backend.

## Autor

Frontend / Backend Developer Jr
Ricardo Escamilla Mendoza