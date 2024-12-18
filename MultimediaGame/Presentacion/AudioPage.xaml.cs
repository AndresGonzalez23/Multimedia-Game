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
using Microsoft.VisualBasic.Devices;

namespace MultimediaGame.Presentacion
{
    /// <summary>
    /// Lógica de interacción para AudioPage.xaml
    /// </summary>
    public partial class AudioPage : Page
    {
        private MainWindow parentWindow;
        private AudioService audioService = new AudioService();
        private List<Recurso> rutasAudiosDescargados = new List<Recurso>();
        private List<string> listaRespuestas = new List<string>();
        private string respuestaCorrecta;
        private string assetsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets");
        private Random random = new Random();
        private int numeroAciertos = 0;
        private const int maxPreguntas = 10;

        public AudioPage(MainWindow window)
        {
            parentWindow = window;
            InitializeComponent();
            this.Loaded += AudioPage_Loaded;
        }

        private async void AudioPage_Loaded(object sender, RoutedEventArgs e)
        {
            await CargarAudiosAsync();
            // Aquí puedes continuar con la lógica de la página de audio
            if (rutasAudiosDescargados.Count < 4)
            {
                // Mostrar un mensaje indicando que no hay suficientes preguntas
                MessageBox.Show("No hay suficientes preguntas disponibles para jugar.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);

                // Salir del método si no hay suficientes preguntas
                return;
            }
            listaRespuestas = rutasAudiosDescargados.Select(r => r.Respuesta).ToList();
            CargarAudioAleatorioEnMediaElement();
            lblCarga.Visibility = Visibility.Collapsed;
            btnReproducir.Visibility = Visibility.Visible;
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

        private async Task CargarAudiosAsync()
        {
            var recursoRepository = new RecursoRepository();
            var recursos = recursoRepository.ObtenerRecursos();

            // Filtrar los recursos que son de tipo "audio"
            var audios = recursos.Where(r => r.Tipo == "audio").ToList();

            // Verificar si hay menos de 4 recursos
            if (audios.Count < 4)
            {
                lblCarga.Content = "No hay suficientes recursos de audio para jugar. Se necesitan al menos 4.";
                lblCarga.Visibility = Visibility.Visible;

                // Ocultar o desactivar elementos de la interfaz
                btnReproducir.Visibility = Visibility.Collapsed;
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
                // Continuar con la lógica habitual si hay suficientes recursos
                audios = audios.OrderBy(_ => random.Next()) // Barajar aleatoriamente
                               .Take(maxPreguntas)          // Tomar máximo `maxPreguntas`
                               .ToList();

                foreach (var audio in audios)
                {
                    string rutaAudio = Path.Combine(assetsPath, $"{audio.Nombre}.mp3");

                    if (!File.Exists(rutaAudio))
                    {
                        await audioService.DescargarAudioAsync(audio, rutaAudio);
                        if (File.Exists(rutaAudio))
                        {
                            audio.RutaLocal = rutaAudio;
                            rutasAudiosDescargados.Add(audio);
                        }
                        else
                        {
                            MessageBox.Show($"El audio {audio.Nombre} no fue encontrado.");
                        }
                    }
                    else
                    {
                        audio.RutaLocal = rutaAudio;
                        rutasAudiosDescargados.Add(audio);
                    }
                }
            }
        }


        private void CargarAudioAleatorioEnMediaElement()
        {
            if (rutasAudiosDescargados.Count > 0)
            {
                // Seleccionar un audio aleatorio
                int indiceAleatorio = random.Next(rutasAudiosDescargados.Count);
                Recurso audioAleatorio = rutasAudiosDescargados[indiceAleatorio];

                string rutaAudio = Path.Combine(assetsPath, $"{audioAleatorio.Nombre}.mp3");
                respuestaCorrecta = audioAleatorio.Respuesta;

                if (File.Exists(rutaAudio))
                {
                    elementoAudio.Source = new Uri(rutaAudio, UriKind.Absolute);
                    //descomentar esta linea
                    List<string> opciones = crearRespuestas(respuestaCorrecta);
                    elementoAudio.LoadedBehavior = MediaState.Manual; // Configurar el MediaElement para control manual
                    elementoAudio.UnloadedBehavior = MediaState.Manual;
                    elementoAudio.Pause();
                    RestaurarColoresBotones(opciones);
                    // Cargar la ruta del audio en el MediaElement
                    lblPregunta.Content = audioAleatorio.Pregunta;
                    rutasAudiosDescargados.RemoveAt(indiceAleatorio);
                    btnReproducir.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show($"El audio en '{rutaAudio}' no fue encontrado.");
                }
            }
            else
            {
                elementoAudio.Pause();
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
            btnPreg1.Background = new SolidColorBrush(Colors.White);
            btnPreg2.Background = new SolidColorBrush(Colors.White);
            btnPreg3.Background = new SolidColorBrush(Colors.White);
            btnPreg4.Background = new SolidColorBrush(Colors.White);
        }

        private void btnReproducir_Click(object sender, RoutedEventArgs e)
        {
            elementoAudio.Position = TimeSpan.Zero;
            elementoAudio.Play(); // Reproducir el audio solo cuando el botón es 
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
            btnReproducir.IsEnabled = false;

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
            CargarAudioAleatorioEnMediaElement();
        }

        private void elementoAudio_MediaEnded(object sender, RoutedEventArgs e)
        {
            btnPreg1.IsEnabled = true;
            btnPreg2.IsEnabled = true;
            btnPreg3.IsEnabled = true;
            btnPreg4.IsEnabled = true;
        }
    }
}
