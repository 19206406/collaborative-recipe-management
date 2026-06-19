# collaborative-recipe-management-api

Se trata de una API REST que se encarga de gestionar recetas colaborativas entre usuarios autenticados. Permite el registro de múltiples usuarios y la administración de recetas. Este proyecto está basado en una arquitectura orientada a servicios.

## Objetivo

Permite a los usuarios crear y compartir recetas, recibir calificaciones y recomendaciones personalizadas, y recibir notificaciones sobre la actividad en sus recetas.

## Arquitectura

El proyecto sigue una arquitectura basada en servicios conectados entre sí mediante HttpClient y a través de un broker de mensajería como lo es RabbitMQ. Además, cada uno de los servicios sigue una arquitectura de Vertical Slice, la cual le proporciona flexibilidad y escalabilidad para nuevas funcionalidades. Sin embargo, aunque cada servicio sigue esta arquitectura, también cuentan con ciertas abstracciones propias de Clean Architecture, principalmente sobre la capa de persistencia de datos. Cada servicio cuenta con su propia base de datos y hace uso de patrones de diseño como CQRS y Repository Pattern.

## Tecnologías

- .NET 10
- ASP.NET Core
- Entity Framework Core
- PostgreSQL
- Redis
- RabbitMQ
- JWT Authentication
- Swagger multi-servicio

## Variables de entorno

Todas las variables de entorno del proyecto se encuentran en el `docker-compose.yml`, el `docker-compose.override.yml` y en los `appsettings.json` de cada servicio, dado que este es un proyecto de aprendizaje y portafolio, no de uso comercial. Si desea instalarlo y modificar estas variables, puede hacerlo teniendo en cuenta la consistencia de dichos cambios a lo largo del proyecto.

## Instalación

Es necesario tener Docker y Docker Compose instalados previamente para ejecutar la aplicación. Luego, aplicar los siguientes comandos:

```bash
git clone https://github.com/19206406/collaborative-recipe-management.git

docker-compose up -d
```

También se puede ejecutar el proyecto desde un IDE, clonando el repositorio y corriendo el Docker Compose desde herramientas como Visual Studio o Rider.

## Enlaces de interés

### Documentación de la API
http://localhost:6005/swagger

### Health Check
http://localhost:6005/health-ui

### UI RabbitMQ
http://localhost:15672

## Decisiones técnicas

- **CQRS**: Se utilizó este patrón para separar las acciones de lectura y escritura, logrando una mayor claridad y separación de responsabilidades.
- **Vertical Slice Architecture**: Se aplicó en cada servicio para agilizar el desarrollo y facilitar la escalabilidad ante nuevas funcionalidades.
- **RabbitMQ**: Se eligió como broker de mensajería por su facilidad de uso, configuración y alta compatibilidad con proyectos de microservicios en ASP.NET Core.
- **ASP.NET Core (.NET 10)**: Se eligió por su estabilidad, solidez para construir soluciones robustas y su amplio ecosistema de herramientas para el desarrollo backend profesional.
- **PostgreSQL**: Se utilizó en todos los servicios por su eficiencia, su excelente integración con Entity Framework Core y su bajo consumo de recursos en entornos contenerizados.
- **YARP**: Se eligió como API Gateway por su fácil configuración, confiabilidad y por ser la opción moderna estándar dentro del ecosistema .NET.
- **Redis**: Se utiliza para cachear respuestas de algunas operaciones con alto consumo de recursos a nivel de base de datos.

## Retos encontrados

Al desarrollar este proyecto me encontré con varios retos, principalmente porque anteriormente nunca había desarrollado una solución basada en microservicios, ni había trabajado con tecnologías necesarias para este tipo de arquitecturas, como RabbitMQ para la comunicación asíncrona entre servicios o HttpClient para mantener la consistencia de datos de forma sincrónica.

Otro reto importante fue la administración de la configuración en Docker Compose, que resultó bastante compleja al inicio, especialmente al momento de realizar pruebas en un entorno que intenta simular condiciones productivas. En cuanto al testing, fue particularmente difícil validar acciones que dependían de la conexión con otros servicios, ya que en ocasiones era complejo detectar en qué punto exacto se producía el error: si en el API Gateway, en el servicio destino o en algún servicio llamado internamente mediante HttpClient. A pesar de estas dificultades, a lo largo del desarrollo fui encontrando soluciones dentro de las herramientas y habilidades que tenía disponibles.

## Mejoras futuras

El proyecto actualmente es funcional en todos sus servicios y cuenta con health checks para monitorear su estado, rate limiting en las acciones, doble autenticación mediante JWT tanto en el API Gateway como en cada servicio, entre otras características. Sin embargo, hay varios puntos de mejora identificados, como la implementación de gRPC para la comunicación entre servicios en lugar de HttpClient, mejoras en el diseño arquitectónico de algunos servicios y la corrección de posibles bugs no detectados aún. Espero aplicar estas mejoras no solo en este proyecto, sino también en futuros proyectos donde pueda seguir poniendo en práctica los conocimientos adquiridos aquí.

## Autor

Sebastian Urrego Graciano
- GitHub: https://github.com/19206406
- LinkedIn: https://www.linkedin.com/feed/