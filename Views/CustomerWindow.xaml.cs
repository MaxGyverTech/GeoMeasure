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
    /// Логика взаимодействия для CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        public CustomerWindow(Customer c)
        {
            InitializeComponent();
            DataContext = c;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
