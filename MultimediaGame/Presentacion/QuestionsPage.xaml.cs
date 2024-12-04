using MultimediaGame.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MultimediaGame.Presentacion
{
    /// <summary>
    /// Lógica de interacción para QuestionsPage.xaml
    /// </summary>
    public partial class QuestionsPage : Page
    {
        private MainWindow parentWindow;
        private List<Recurso> preguntasDescargadas = new List<Recurso>();
        private List<string> listaRespuestas = new List<string>();
        private string respuestaCorrecta;
        private Random random = new Random();
        private int numeroAciertos = 0;

        public QuestionsPage(MainWindow window)
        {
            parentWindow = window;
            InitializeComponent();
            this.Loaded += QuestionPage_Loaded;
        }

        private async void QuestionPage_Loaded(object sender, RoutedEventArgs e)
        {
            await CargarPreguntasAsync(); // Llama al método asincrónico al cargarse la página
            listaRespuestas = preguntasDescargadas.Select(r => r.Respuesta).ToList();
            CargarPreguntasAleatoriasEnControl();
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

        private void CargarPreguntasAleatoriasEnControl()
        {
            if (preguntasDescargadas.Count > 0)
            {
                int indiceAleatorio = random.Next(preguntasDescargadas.Count);
                Recurso recursoAleatorio = preguntasDescargadas[indiceAleatorio];

                respuestaCorrecta = recursoAleatorio.Respuesta;
                List<string> opciones = crearRespuestas(respuestaCorrecta);
                RestaurarColoresBotones(opciones);
                lblPregunta.Content = recursoAleatorio.Pregunta;
                preguntasDescargadas.RemoveAt(indiceAleatorio);
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

        private async Task CargarPreguntasAsync()
        {
            var recursoRepository = new RecursoRepository();

            var recursos = recursoRepository.ObtenerRecursos();
            var preguntas = recursos.Where(r => r.Tipo == "pregunta").ToList();

            foreach (var pregunta in preguntas)
            {
                preguntasDescargadas.Add(pregunta);
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
            CargarPreguntasAleatoriasEnControl();
        }
    }
}
