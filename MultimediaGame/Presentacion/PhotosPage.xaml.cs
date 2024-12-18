﻿using MultimediaGame.Dominio;
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
        private const int maxPreguntas = 10;
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
            if (rutasImagenesDescargadas.Count < 4)
            {
                // Mostrar un mensaje indicando que no hay suficientes preguntas
                MessageBox.Show("No hay suficientes preguntas disponibles para jugar.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);

                // Salir del método si no hay suficientes preguntas
                return;
            }
            listaRespuestas = rutasImagenesDescargadas.Select(r => r.Respuesta).ToList();
            CargarImagenAleatoriaEnControl();
            lblCarga.Visibility = Visibility.Collapsed;
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

            var recursos = recursoRepository.ObtenerRecursos();

            // Filtrar solo los recursos que son imágenes
            var imagenes = recursos.Where(r => r.Tipo == "imagen")
                           .OrderBy(_ => random.Next()) // Barajar aleatoriamente
                           .ToList();

            // Si hay menos de 4 imágenes, mostrar mensaje en lblCarga y salir
            if (imagenes.Count < 4)
            {
                lblCarga.Content = "No hay suficientes imágenes disponibles para jugar.";
                lblCarga.Visibility = Visibility.Visible;
                lblPregunta.Visibility = Visibility.Collapsed;
                btnPreg1.Visibility = Visibility.Collapsed;
                btnPreg2.Visibility = Visibility.Collapsed;
                btnPreg3.Visibility = Visibility.Collapsed;
                btnPreg4.Visibility = Visibility.Collapsed;
                parentWindow.mainFrame.Content = null;
                parentWindow.btnAudio.Visibility = Visibility.Visible;
                parentWindow.btnPhotos.Visibility = Visibility.Visible;
                parentWindow.btnQuestions.Visibility = Visibility.Visible;
                parentWindow.lblTítulo.Visibility = Visibility.Visible;
            }
            else
            {
                string assetsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets");

                foreach (var imagen in imagenes.Take(maxPreguntas))
                {
                    string rutaImagen = Path.Combine(assetsPath, $"{imagen.Nombre}.jpg");

                    if (!File.Exists(rutaImagen))
                    {
                        await imagenService.DescargarImagenDesdeDriveAsync(imagen.Url, rutaImagen);
                        if (File.Exists(rutaImagen))
                        {
                            imagen.RutaLocal = rutaImagen;
                            rutasImagenesDescargadas.Add(imagen);
                        }
                    }
                    else
                    {
                        imagen.RutaLocal = rutaImagen;
                        rutasImagenesDescargadas.Add(imagen);
                    }
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
                parentWindow.lblTítulo.Visibility = Visibility.Visible;
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
            string mensaje;
            string rutaImagen;
            if (numeroAciertos > 9)
            {
                mensaje = $"Que buen horneado. Número de aciertos: {numeroAciertos}";
                rutaImagen = "pack://application:,,,/Assets/tarta10.jpg";
            }
            else if (numeroAciertos > 6)
            {
                mensaje = $"Te ha faltado un trozo de tarta. Número de aciertos: {numeroAciertos}";
                rutaImagen = "pack://application:,,,/Assets/tarta7.jpg";
            }
            else if (numeroAciertos > 4)
            {
                mensaje = $"Al menos es un buen bizcocho. Número de aciertos: {numeroAciertos}";
                rutaImagen = "pack://application:,,,/Assets/tarta5.jpg";
            }
            else
            {
                mensaje = $"Falta hasta la levadura. Número de aciertos: {numeroAciertos}";
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
