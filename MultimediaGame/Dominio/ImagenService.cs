using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MultimediaGame.Dominio
{
    internal class ImagenService
    {
        private readonly HttpClient httpClient = new HttpClient();

        // Descarga la imagen desde la URL pública
        public async Task DescargarImagenAsync(Recurso recurso, string rutaDestino)
        {
            if (recurso.Tipo == "imagen")
            {
                var response = await httpClient.GetAsync(recurso.Url);
                response.EnsureSuccessStatusCode();

                using (var fileStream = new FileStream(rutaDestino, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await response.Content.CopyToAsync(fileStream);
                }

                Console.WriteLine($"Imagen '{recurso.Nombre}' descargada en {rutaDestino}");
            }
        }

        // Carga y muestra la imagen desde la URL pública
        public async Task<BitmapImage> CargarImagenAsync(Recurso recurso)
        {
            if (recurso.Tipo == "imagen")
            {
                var response = await httpClient.GetStreamAsync(recurso.Url);
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = response;
                bitmap.CacheOption = BitmapCacheOption.OnLoad; // Cargar la imagen en memoria
                bitmap.EndInit();
                bitmap.Freeze(); // Hace que la imagen sea seguro para el acceso desde varios hilos

                Console.WriteLine($"Imagen '{recurso.Nombre}' cargada desde URL pública.");
                return bitmap;
            }
            return null;
        }

        // Método para descargar una imagen desde Google Drive
        public async Task DescargarImagenDesdeDriveAsync(string fileId, string destinoPath)
        {
            var url = fileId;

            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            using (var fileStream = new FileStream(destinoPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await response.Content.CopyToAsync(fileStream);
            }

            Console.WriteLine($"Imagen descargada desde Google Drive en {destinoPath}");
        }
    }
}
