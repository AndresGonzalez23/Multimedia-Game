using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace MultimediaGame.Dominio
{
    internal class AudioService
    {
        private readonly HttpClient httpClient = new HttpClient();

        public async Task DescargarAudioAsync(Recurso recurso, string rutaDestino)
        {
            if (recurso.Tipo == "audio")
            {
                var response = await httpClient.GetAsync(recurso.Url);
                response.EnsureSuccessStatusCode();

                using (var fileStream = new FileStream(rutaDestino, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await response.Content.CopyToAsync(fileStream);
                }

                Console.WriteLine($"Audio '{recurso.Nombre}' descargado en {rutaDestino}");
            }
        }

        public async Task ReproducirAudioAsync(Recurso recurso)
        {
            if (recurso.Tipo == "audio")
            {
                var response = await httpClient.GetStreamAsync(recurso.Url);

                using (var audioFile = new WaveFileReader(response))
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Play();
                    Console.WriteLine($"Reproduciendo '{recurso.Nombre}' desde URL pública...");
                    await Task.Delay(audioFile.TotalTime);  // Espera hasta que el audio termine de reproducirse
                }
            }
        }
    }
}
