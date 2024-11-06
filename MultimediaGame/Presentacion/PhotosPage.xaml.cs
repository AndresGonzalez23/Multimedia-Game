using MultimediaGame.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Net.Http;


namespace MultimediaGame.Presentacion
{
    /// <summary>
    /// Lógica de interacción para PhotosPage.xaml
    /// </summary>
    public partial class PhotosPage : Page
    {
        private MainWindow parentWindow;
        private List<string> rutasImagenesDescargadas = new List<string>();
        private Random random = new Random();

        public PhotosPage(MainWindow window)
        {
            parentWindow = window;
            InitializeComponent();
            this.Loaded += PhotosPage_Loaded;
        }

        private async void PhotosPage_Loaded(object sender, RoutedEventArgs e)
        {
            await CargarImagenesAsync(); // Llama al método asincrónico al cargarse la página
            CargarImagenAleatoriaEnControl();
            btnNuevaFoto.IsEnabled = true;
        }

        private async Task CargarImagenesAsync()
        {
            var recursoRepository = new RecursoRepository();
            var imagenService = new ImagenService();

            // Obtener todos los recursos desde el archivo JSON
            var recursos = recursoRepository.ObtenerRecursos();

            // Filtrar solo los recursos que son imágenes
            var imagenes = recursos.Where(r => r.Tipo == "imagen").ToList();

            string assetsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets");

            foreach (var imagen in imagenes)
            {
                // Asumiendo que imagen.FileId es el ID del archivo en Google Drive
                string rutaImagen = Path.Combine(assetsPath, $"{imagen.Nombre}.jpg");

                // Descargar la imagen desde Google Drive
                await imagenService.DescargarImagenDesdeDriveAsync(imagen.Url, rutaImagen);

                // Verificar si el archivo existe después de la descarga
                if (File.Exists(rutaImagen))
                {
                    rutasImagenesDescargadas.Add(rutaImagen); 
                }
                else
                {
                    MessageBox.Show($"La imagen {imagen.Nombre} no fue encontrada.");
                }
            }
        }

        private void CargarImagenAleatoriaEnControl()
        {
            if (rutasImagenesDescargadas.Count > 0)
            {

                // Seleccionar un índice aleatorio de la lista
                int indiceAleatorio = random.Next(rutasImagenesDescargadas.Count);
                string rutaImagenAleatoria = rutasImagenesDescargadas[indiceAleatorio];

                // Verificar que el archivo existe
                if (File.Exists(rutaImagenAleatoria))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(rutaImagenAleatoria, UriKind.Absolute);
                    bitmap.EndInit();

                    // Asignar la imagen al control de la interfaz
                    imagenPregunta.Source = bitmap;
                    statusLabel.Content = $"Imagen aleatoria cargada desde: {rutaImagenAleatoria}"; // Mensaje de estado opcional
                    rutasImagenesDescargadas.RemoveAt(indiceAleatorio);
                }
                else
                {
                    MessageBox.Show($"La imagen en '{rutaImagenAleatoria}' no fue encontrada.");
                }
            }
            else
            {
                MessageBox.Show("No hay imágenes descargadas para mostrar.");
            }
        }

        private void btnNuevaFoto_Click(object sender, RoutedEventArgs e)
        {
            CargarImagenAleatoriaEnControl();
        }
    }
}
