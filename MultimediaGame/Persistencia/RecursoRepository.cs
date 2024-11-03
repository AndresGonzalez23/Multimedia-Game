using MultimediaGame.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;


public class RecursoRepository
{
    private const string RutaArchivo = "/Assets/recursos.json";
    public List<Recurso> ObtenerRecursos()
    {
        // Supongamos que este método carga recursos desde un archivo JSON
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "recursos.json");
        var recursosJson = File.ReadAllText(path);
        var recursos = JsonConvert.DeserializeObject<List<Recurso>>(recursosJson);

        foreach (var recurso in recursos)
        {
            // Crear la URL de descarga
            recurso.Url = recurso.CrearUrlDescarga();
        }

        return recursos;
    }
}