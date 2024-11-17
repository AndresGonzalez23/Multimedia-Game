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
        private List<string> listaRespuestas = new List<string>();
        private string respuestaCorrecta;
        private Random random = new Random();
        private int numeroAciertos = 0;

        
        public PhotosPage(MainWindow window)
        {
            InitializeComponent();
            parentWindow = window;
            this.Loaded += PhotosPage_Loaded;
        }

        private async void PhotosPage_Loaded(object sender, RoutedEventArgs e)
        {
            await CargarImagenesAsync(); // Llama al método asincrónico al cargarse la página
            listaRespuestas = rutasImagenesDescargadas.Select(r => r.Respuesta).ToList();
            CargarImagenAleatoriaEnControl();
            lblPregunta.Visibility = Visibility.Visible;
            btnPreg1.Visibility = Visibility.Visible;
            btnPreg1.IsEnabled = true;
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
                respuestaCorrecta = recursoAleatorio.Respuesta;

                // Verificar que el archivo existe
                if (File.Exists(rutaImagen))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(rutaImagen, UriKind.Absolute);
                    bitmap.EndInit();

                    // Asignar la imagen al control de la interfaz
                    imagenPregunta.Source = bitmap;
                    List<string> opciones = crearRespuestas(respuestaCorrecta);
                    RestaurarColoresBotones(opciones);
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
                MostrarMensajeConImagen();
                numeroAciertos = 0;
                parentWindow.mainFrame.Content = null;
                parentWindow.btnAudio.Visibility = Visibility.Visible;
                parentWindow.btnPhotos.Visibility = Visibility.Visible;
                parentWindow.btnQuestions.Visibility = Visibility.Visible;
            }
        }

        private void RestaurarColoresBotones(List<string> opciones)
        {
            btnPreg1.Content = opciones[0];
            btnPreg2.Content = opciones[1];
            btnPreg3.Content = opciones[2];
            btnPreg4.Content = opciones[3];
            btnPreg1.Background = new SolidColorBrush(Colors.White); // o el color que prefieras como base
            btnPreg2.Background = new SolidColorBrush(Colors.White);
            btnPreg3.Background = new SolidColorBrush(Colors.White);
            btnPreg4.Background = new SolidColorBrush(Colors.White);
        }

        private void MostrarMensajeConImagen()
        {
            string mensaje = $"CakeHoot completado. Número de aciertos: {numeroAciertos}";
            string rutaImagen;
            if (numeroAciertos > 9)
            {
                rutaImagen = "pack://application:,,,/Assets/tarta10.jpg";
            }
            else if (numeroAciertos > 6)
            {
                rutaImagen = "pack://application:,,,/Assets/tarta7.jpg";
            }
            else if (numeroAciertos > 4)
            {
                rutaImagen = "pack://application:,,,/Assets/tarta5.jpg";
            }
            else
            {
                rutaImagen = "pack://application:,,,/Assets/tarta3.jpg";
            }


            var messageBox = new finisgWindow(mensaje, rutaImagen);
            messageBox.ShowDialog();
        }

        private List<string> crearRespuestas(string respuesta)
        {
            var posiblesOpciones = new List<string> { respuesta };
            while (posiblesOpciones.Count < 4)
            {
                string opcionAleatoria = listaRespuestas[random.Next(listaRespuestas.Count)];
                if (!posiblesOpciones.Contains(opcionAleatoria))
                {
                    posiblesOpciones.Add(opcionAleatoria);
                }
            }
            return posiblesOpciones.OrderBy(_ => random.Next()).ToList();
        }

        private void VerificarRespuesta(object sender, RoutedEventArgs e)
        {
            Button botonSeleccionado = sender as Button;

            if (botonSeleccionado.Content.ToString() == respuestaCorrecta)
            {
                numeroAciertos++;
                botonSeleccionado.Background = new SolidColorBrush(Colors.Green);
                MessageBox.Show("¡Respuesta correcta!");
            }
            else
            {
                botonSeleccionado.Background = new SolidColorBrush(Colors.Red);
                MessageBox.Show("Respuesta incorrecta");
            }
            CargarImagenAleatoriaEnControl();
        }
    }
}
