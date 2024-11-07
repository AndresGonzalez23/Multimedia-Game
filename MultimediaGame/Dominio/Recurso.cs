using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaGame.Dominio
{
    public class Recurso
    {
        public string Nombre { get; set; }
        public string Url { get; set; } // URL de visualización de Google Drive
        public string Tipo { get; set; } // "imagen", "audio", etc.
        public string Pregunta { get; set; }
        public string RutaLocal { get; set; }

        // Método para extraer el ID desde el enlace de visualización de Google Drive
        public string ExtraerIdDesdeEnlace()
        {
            if (string.IsNullOrEmpty(Url))
                throw new ArgumentException("El enlace no puede estar vacío.", nameof(Url));

            // Buscar el patrón del ID en el enlace
            var idIndex = Url.IndexOf("/d/") + 3; // 3 es la longitud de "/d/"
            var endIndex = Url.IndexOf("/", idIndex);
            if (endIndex == -1) endIndex = Url.IndexOf("?", idIndex); // También considerar el '?' como fin

            if (idIndex == -1 || endIndex == -1)
                throw new FormatException("El enlace no es válido para extraer el ID.");

            return Url.Substring(idIndex, endIndex - idIndex);
        }

        // Método para crear la URL de descarga
        public string CrearUrlDescarga()
        {
            string id = ExtraerIdDesdeEnlace();
            return $"https://drive.google.com/uc?export=download&id={id}";
        }
    }
}