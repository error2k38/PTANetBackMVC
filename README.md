# Prueba Técnica para candidatos

## Descripción

Este repositorio contiene una prueba técnica para candidatos que deseen unirse a nuestro equipo de desarrollo backend y frontend. El objetivo de la prueba es evaluar las habilidades de los candidatos en el desarrollo de aplicaciones utilizando tecnologías como .NET, C#, SQL Server, MVC...

## Instrucciones

1. Realizar un programa en .NET - C# que cumpla con los siguientes requisitos:
    - Haz un fork de este proyecto
    - Consumir la siguiente API: [https://api.opendata.esett.com/](https://api.opendata.esett.com/). Escoge sólo 1 servicio cualquiera de los proporcionados por la API.
    - Almacenar la información obtenida en la base de datos. (usa SQL Server en contenedor de docker para esto)
    - Implementar un controlador que permita filtrar por Primary Key en la base de datos.
    - Construir una API REST con Swagger que permita visualizar los datos almacenados en la base de datos.
    - Usar contenedores Docker para DDBB y la propia App
    - Usa arquitectura MVC (sólo API imagina que existe un segundo proyecto con el frontend, por tanto las vistas serán DTOs)
    - Haz un pull request con tu nombre completo y comenta lo que creas necesario al evaluador técnico.
    - Elige entre implementar CRUD o CQRS

### Criterios de evaluación:

Se valorará positivamente (pero no es obligatorio cumplir con todos estos puntos):

1. El uso de código limpio y buenas prácticas de programación tanto en el frontend como en el backend.
2. Utilizar código generado a mano en lugar de depender excesivamente de herramientas de generación automática.
3. Hacer commits frecuentes y bien explicados durante el desarrollo.
4. Demostrar conocimientos en patrones de diseño, tanto en el frontend como en el backend.
5. Gestion correcta de los secretos como cadenas de conexión, usuarios, passwords...
6. Uso del inglés en código y comentarios
7. Uso de elementos de monitoreo y observabilidad como ILogger
8. Uso de Eventos
9. Manejo de excepciones con patron monad
10. Pruebas de test

## Tecnologías utilizadas

- .NET - C#
- SQL Server
- MVC

## Estructura del repositorio

No hay restricciones específicas sobre la estructura del repositorio. Los candidatos son libres de organizar su código de la manera que consideren más apropiada. Sin embargo, se recomienda seguir las convenciones de nomenclatura y estructura de proyecto estándar.

¡Buena suerte!

## Respuesta a la problemática planteada:

1. Capa de Dominio: Se define la capa de dominio. Aquí es donde se modela el dominio del negocio, definiendo todas las entidades, los objetos de valor, los eventos de dominio, las interfaces, los tipos y las excepciones que son específicas del dominio del negocio.
2. Capa de Repositorio: Una vez definido el dominio, se implementan los repositorios. Los repositorios son los encargados de manejar la persistencia de las entidades del dominio. En esta capa, se hace uso del patrón de diseño de repositorio para abstraer las operaciones de la base de datos. Además, se configuran los valores y características de las propiedades en el Fluent de esta capa para una mejor organización y limpieza.
3. Capa de Mapeo (Mapper): A continuación, se implementa la capa de mapeo. Esta capa es responsable de convertir los objetos de dominio en DTOs (Data Transfer Objects) y viceversa. Los DTOs son necesarios para transferir datos entre procesos, lo que ayuda a reducir el número de llamadas al servidor y mejora el rendimiento de la aplicación.
4. Capa de Servicios de Aplicación: Finalmente, se implementa la capa de servicios de aplicación. Los servicios de aplicación son los encargados de coordinar las operaciones de alto nivel que implican a varias entidades del dominio.

## Manejo de errores

1. Manejo de Excepciones: Se evita lanzar excepciones excesivas en los endpoints. En su lugar, se hace uso de un middleware que se encarga de manejar las excepciones que no son manejadas por el usuario. Este middleware captura cualquier excepción no manejada y devuelve una respuesta HTTP adecuada. Esto mejora la robustez de la aplicación y proporciona una mejor experiencia al usuario. Las excepciones se utilizan de manera estratégica y se reservan para situaciones críticas y específicas. El uso descontrolado del manejo y lanzamiento de excepciones puede llevar a problemas de rendimiento y dificultades en la depuración.
2. Uso de ILogger: Para tener un control de los errores producidos, se hace uso de ILogger, una interfaz proporcionada por ASP.NET Core. ILogger permite registrar mensajes en una variedad de niveles de gravedad (Información, Advertencia, Error, etc.) y puede ser configurado para registrar mensajes a diferentes destinos (como la consola, un archivo, etc.). Esto es especialmente útil para el seguimiento de errores y la depuración.

## Nota Importante

La solución propuesta es puramente ilustrativa, diseñada para demostrar las capacidades técnicas y prácticas de desarrollo en un proyecto .NET. Se han incluido ejemplos prácticos para mostrar cómo se abordan típicamente los desafíos en este tipo de proyectos.

Es importante destacar que esta solución no toma en cuenta los requisitos específicos de entidades desconocidas. En un escenario real, se realizaría un análisis exhaustivo de los requisitos del cliente y se diseñaría la solución en función de esos requisitos. Sin embargo, en este caso, el objetivo principal es demostrar las habilidades técnicas y las mejores prácticas de desarrollo, más que satisfacer los requisitos de un cliente específico.
