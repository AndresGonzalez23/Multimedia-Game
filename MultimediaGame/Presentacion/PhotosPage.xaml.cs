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
        public PhotosPage(MainWindow window)
        {
            parentWindow = window;
            InitializeComponent();
            this.Loaded += PhotosPage_Loaded;
        }

        private async void PhotosPage_Loaded(object sender, RoutedEventArgs e)
        {
            await CargarImagenesAsync(); // Llama al método asincrónico al cargarse la página
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
                    statusLabel.Content = $"La imagen '{imagen.Nombre}' ha sido descargada correctamente en: {rutaImagen}";
                    // Cargar la imagen en el control Image
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(rutaImagen, UriKind.RelativeOrAbsolute);
                    bitmap.EndInit();
                    imagenPregunta.Source = bitmap;
                }
                else
                {
                    MessageBox.Show($"La imagen {imagen.Nombre} no fue encontrada.");
                }
            }
        }



    }
}
