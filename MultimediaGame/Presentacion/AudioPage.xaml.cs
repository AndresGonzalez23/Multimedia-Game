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
    /// Lógica de interacción para AudioPage.xaml
    /// </summary>
    public partial class AudioPage : Page
    {
        private MainWindow parentWindow;
        
        public AudioPage(MainWindow window)
        {
            parentWindow = window;
            InitializeComponent();
        }
    }
}
