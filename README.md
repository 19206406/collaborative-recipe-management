# collaborative-recipe-management-api 

Se trata de una API REST que se encarga de gestionar recetas colaborativas entre usuarios autenticados. Permite registro de multiples usuarios y la administracción de recetas este proyecto esta basado en una arquitectura orientada a servicios. 

## Objectivo 

Permite a los usuarios crear y compartir recetas, recibir ratings y recomendaciones personalizadas, y recibir notificaciones sobre actividad en sus recetas.

## Arquitectura 

El proyecto sigue una arquitectura basada en servicios conectados entre si mediante httpClient y a traves de un broker de mensajeria como lo es RabbitMQ además en cada uno de los servicios, siguen una arquitectura de vertical Slice Arquitecture la cual le proporciona a cada uno de los servicios una flexibilidad para la escalibilidad para nuevas funcionalidades sin embargo aunque cada uno de los proyectos sigue esta arquitectura tambien cuentan con ciertas estracciones de arquitecturas como clean architecture sobre la persistencia de los datos. Sobre la persistencia de los datos en este proyecto cada uno de los servicios cuenta con su propia base de datos. Además de utilizar patrones de diseño como CQRS y Repository Pattern. 

## Tecnologias 
- .NET 10 
- ASP.NET Core 
- Entity Framework Core 
- Postgres 
- Redis 
- RabbitMQ 
- JWT Authentication 
- Swagger multi-servicio 


## Instalación 

Se debe de contar previamente instalado docker-compose para ejecutar la aplicación. Y luego aplicar los siguiente comandos: 

```bash 
git clone https://github.com/19206406/collaborative-recipe-management.git 

docker-compose up -d
```

Tambien se puede ejecutar el proyecto desde un IDE clonando y luego correr el docker-compose esto se puede hacer en IDES como Visual Studio o Rider. 

## Documentación de la API 
http://localhost:6005/swagger 

## Desiciones Tecnicas 

## Problemas Resueltos 

## Mejoras futuras 

## Autor 
Sebastian Urrego Graciano 
- GitHub: https://github.com/19206406 
- LinkedIn: https://www.linkedin.com/feed/ 

