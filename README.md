# CakeHoot

Proyecto realizado para la asignatura de Multimedia de la rama de Tecnologías de la Información en la Escuela Superior de Informática de Ciudad Real. Se trata de una aplicación multimedia de escritorio
gamificada al estilo Kahoot, Trivial, etc. En este caso la temática estará centrada en libros y series varias, aunque es posible modificarlo añadiendo imágenes, audios o preguntas propias. 

Hay trés modos de juego donde las preguntas se centrarán en un elemento multimedia diferente:
- Preguntas: Apartedo centrado en el texto.
- Fotos: Apartado centrado en la relación de imágenes.
- Audios: Apartado centrado en la relación de audios.

En dicha aplicación se aprovecha la posibilidad de descarga de archivos de Google Drive, para que sea lo más ligera posible.

## 🧍: Creador

- Andrés González Varela

## ⚙️ Tecnologías Usadas

- C#
- WPF
- Paquetes NuGet

## 📖 Modificación de las preguntas

Si quieres modificar el juego para hacer diferentes preguntas o enfocarlo a otras tematicas solo debes entrar en el .json y modificarlo usando la siguiente plantilla:

    {
      "id": 1,
      "nombre": "gandalf",
      "tipo": "audio",
      "url": "https://drive.google.com/file/d/1ICSqS6Vo1Lc4yve94Zkv1OxXralnGJ5w/view?usp=drive_link",
      "pregunta": "¿En qué saga de películas o serie sale este fragmento de audio?",
      "respuesta": "El Señor de los Anillos"
    }

Lo más importante a la hora de modificar el JSON es:
- Que el nombre coincida con el que tiene el archivo subido en Google Drive.
- Que el archivo este accesible para todo el mundo que tenga el enlace.
- Cambiar la pregunta y la respuesta.
    
⚠️¡Aviso!
- El apartado de preguntas también requiere un enlace, pero este no se usará por lo que podríamos usar un enlace por defecto.

## 🛠️ Ejecución

Hay dos opciones para ejecutar el proyecto, usar un ejecutable o ejecutarlo desde Visual Studio 2022.

- Ejecutable:
    Debe descargarse el archivo comprimido en el siguiente [enlace](https://drive.google.com/file/d/1mHEctW3jXBM_AP0XVVaUQrZVNqoH3fvQ/view?usp=sharing), descomprimirlo en una carpeta y ejecutar el .exe y la aplicación se empezará automaticamente, sin necesidad de      instalar ningun programa o necesitar instalar .NET, ya que viene preparado dentro del propio ejecutable.
  
  ⚠️¡Aviso!
    Al no ser una aplicación que esté publicada en la tienda de Windows, el antivirus te preguntará si deseas ejecutarla aunque no sea de una fuente fiable, al darle a Ejecutar de todos modos funcionará sin problemas.
  
- Visual Studio 2022:
    Simplemente debes copiar el enlace del repositorio para clonarlo en el propio Visual Studio 2022, una vez clonado solo hará falta darle a ejecutar.
  
