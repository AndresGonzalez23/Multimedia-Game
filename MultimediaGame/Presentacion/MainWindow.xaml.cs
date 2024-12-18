﻿using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Page[] pages;

        public MainWindow()
        {
            InitializeComponent();
            pages = new Page[] { new AudioPage(this), new PhotosPage(this), new QuestionsPage(this) };
        }
        private void btnQuestions_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = pages[2];
            btnAudio.Visibility = Visibility.Collapsed;
            btnPhotos.Visibility = Visibility.Collapsed;
            btnQuestions.Visibility = Visibility.Collapsed;
            lblTítulo.Visibility = Visibility.Collapsed;
        }

        private void btnPhotos_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = pages[1];
            btnAudio.Visibility = Visibility.Collapsed;
            btnPhotos.Visibility = Visibility.Collapsed;
            btnQuestions.Visibility = Visibility.Collapsed;
            lblTítulo.Visibility = Visibility.Collapsed;
        }

        private void btnAudio_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = pages[0];
            btnAudio.Visibility = Visibility.Collapsed;
            btnPhotos.Visibility = Visibility.Collapsed;
            btnQuestions.Visibility = Visibility.Collapsed;
            lblTítulo.Visibility = Visibility.Collapsed;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                // Cambiar a la página específica (por ejemplo, HomePage)
                mainFrame.Content = null;
                btnAudio.Visibility = Visibility.Visible;
                btnPhotos.Visibility = Visibility.Visible;
                btnQuestions.Visibility = Visibility.Visible;
                lblTítulo.Visibility = Visibility.Visible;
            }
        }
    }
}