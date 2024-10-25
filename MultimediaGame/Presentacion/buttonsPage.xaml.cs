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
    /// Lógica de interacción para buttonsPage.xaml
    /// </summary>
    public partial class buttonsPage : Page
    {
        public Page[] pages;

        public buttonsPage()
        {
            //pages = new Page[] { new AudioPage(this), new PhotosPage(this), new QuestionsPage(this) };
            InitializeComponent();
        }
        private void btnQuestions_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = pages[2];
        }

        private void btnPhotos_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = pages[1];
        }

        private void btnAudio_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = pages[0];
        }
    }
}
