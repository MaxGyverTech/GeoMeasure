using GeoMeasure.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GeoMeasure
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            // some code

            //OffsetX=...
            //OffsetY=...
            //Zoom = ...
        }
    }
}