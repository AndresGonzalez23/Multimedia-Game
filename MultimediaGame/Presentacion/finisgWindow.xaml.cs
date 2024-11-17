using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MultimediaGame.Presentacion
{
    /// <summary>
    /// Lógica de interacción para finisgWindow.xaml
    /// </summary>
    public partial class finisgWindow : Window
    {
        public finisgWindow(string mensaje, string imagePath)
        {
            InitializeComponent();
            txtMessage.Text = mensaje;

            // Cargar la imagen si se proporciona una ruta
            if (!string.IsNullOrEmpty(imagePath))
            {
                imagenTarta.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
