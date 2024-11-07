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
        private List<Recurso> rutasImagenesDescargadas = new List<Recurso>();
        private Random random = new Random();

        public PhotosPage(MainWindow window)
        {
            InitializeComponent();
            parentWindow = window;
            this.Loaded += PhotosPage_Loaded;
        }

        private async void PhotosPage_Loaded(object sender, RoutedEventArgs e)
        {
            await CargarImagenesAsync(); // Llama al método asincrónico al cargarse la página
            CargarImagenAleatoriaEnControl();
            lblPregunta.Visibility = Visibility.Visible;
            btnNuevaFoto.Visibility = Visibility.Visible;
            btnNuevaFoto.IsEnabled = true;
            btnPreg2.Visibility = Visibility.Visible;
            btnPreg2.IsEnabled = true;
            btnPreg3.Visibility = Visibility.Visible;
            btnPreg3.IsEnabled = true;
            btnPreg4.Visibility = Visibility.Visible;
            btnPreg4.IsEnabled = true;
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
            List<string> imagenesDescargadas = new List<string>();

            foreach (var imagen in imagenes)
            {
                // Asumiendo que imagen.FileId es el ID del archivo en Google Drive
                string rutaImagen = Path.Combine(assetsPath, $"{imagen.Nombre}.jpg");

                // Verificar si la imagen ya ha sido descargada
                if (!File.Exists(rutaImagen))
                {
                    // Descargar la imagen desde Google Drive
                    await imagenService.DescargarImagenDesdeDriveAsync(imagen.Url, rutaImagen);

                    // Verificar si el archivo existe después de la descarga
                    if (File.Exists(rutaImagen))
                    {
                        imagen.RutaLocal = rutaImagen;
                        rutasImagenesDescargadas.Add(imagen);
                    }
                    else
                    {
                        MessageBox.Show($"La imagen {imagen.Nombre} no fue encontrada.");
                    }
                }
                else
                {
                    imagen.RutaLocal = rutaImagen;
                    rutasImagenesDescargadas.Add(imagen);
                }
            }
        }

        private void CargarImagenAleatoriaEnControl()
        {
            if (rutasImagenesDescargadas.Count > 0)
            {

                // Seleccionar un índice aleatorio de la lista
                int indiceAleatorio = random.Next(rutasImagenesDescargadas.Count);
                Recurso recursoAleatorio = rutasImagenesDescargadas[indiceAleatorio];

                string rutaImagen = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", $"{recursoAleatorio.Nombre}.jpg");

                // Verificar que el archivo existe
                if (File.Exists(rutaImagen))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(rutaImagen, UriKind.Absolute);
                    bitmap.EndInit();

                    // Asignar la imagen al control de la interfaz
                    imagenPregunta.Source = bitmap;
                    lblPregunta.Content = recursoAleatorio.Pregunta;
                    rutasImagenesDescargadas.RemoveAt(indiceAleatorio);
                }
                else
                {
                    MessageBox.Show($"La imagen en '{rutaImagen}' no fue encontrada.");
                }
            }
            else
            {
                MessageBox.Show("CakeHoot completado.");
                parentWindow.mainFrame.Content = null;
                parentWindow.btnAudio.Visibility = Visibility.Visible;
                parentWindow.btnPhotos.Visibility = Visibility.Visible;
                parentWindow.btnQuestions.Visibility = Visibility.Visible;
            }
        }

        private void btnNuevaFoto_Click_1(object sender, RoutedEventArgs e)
        {
            CargarImagenAleatoriaEnControl();
            
        }
    }
}
