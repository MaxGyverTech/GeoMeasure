using GeoMeasure.Models.Db;
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
using System.Windows.Shapes;

namespace GeoMeasure.Views
{
    /// <summary>
    /// Логика взаимодействия для ProjectWindow.xaml
    /// </summary>
    public partial class ProjectWindow : Window
    {
        public ProjectWindow(Project p)
        {
            InitializeComponent();
            DataContext = p;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
