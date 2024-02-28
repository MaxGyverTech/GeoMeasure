using GeoMeasure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using GeoMeasure.Models.Db;
using GeoMeasure.Views;
using System.Globalization;
using System.Windows.Input;
using System.Windows.Controls;

namespace GeoMeasure.ViewModels
{
    public class MainViewModel : NotifyProperty
    {
        Database db = Database.getInstance();
        DrawingImage image;

        Customer selectedCustomer;
        Project selectedProject;
        Area selectedArea;
        public ObservableCollection<Customer> Customers { get=>db.Customers.Local.ToObservableCollection(); }
        public MainViewModel()
        {
            AddCustomerCommand = new(AddCustomer);
            DeleteCustomerCommand = new(DeleteCustomer, (o)=>SelectedCustomer!=null);
            AddProjectCommand = new(AddProject, (o) => SelectedCustomer != null);
            DeleteProjectCommand = new(DeleteProject, (o) => SelectedProject != null);
            AddAreaCommand = new(AddArea, (o) => SelectedProject != null);
            DeleteAreaCommand = new(DeleteArea, (o) => SelectedArea != null);
            OpenAreaCommand = new(OpenArea);
            ZoomCommand = new(Zoom);
        }
        public RelayCommand AddCustomerCommand { get; set; }
        public RelayCommand DeleteCustomerCommand { get; set; }
        public RelayCommand AddProjectCommand { get; set; }
        public RelayCommand DeleteProjectCommand { get; set; }
        public RelayCommand AddAreaCommand { get; set; }
        public RelayCommand DeleteAreaCommand { get; set; }
        public RelayCommand OpenAreaCommand { get; set; }
        public RelayCommand ZoomCommand { get; set; }

        void AddCustomer(object obj)
        {
            var c = new Customer() { Name="", Phone="" };
            if (new CustomerWindow(c).ShowDialog() == false) return;
            db.Customers.Add(c); 
            db.SaveChanges();
            SelectedCustomer = c;
        }
        void DeleteCustomer(object obj)
        {
            if (MessageBox.Show("Вы уверены что хотите удалить заказчика?", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.No) return;
            db.Customers.Remove(SelectedCustomer);
            db.SaveChanges();
        }
        void AddProject(object obj)
        {
            var p = new Project() { Name = "", Address = "" , Customer = SelectedCustomer };
            if (new ProjectWindow(p).ShowDialog() == false) return;
            db.Projects.Add(p);
            db.SaveChanges();
            SelectedProject = p;
        }
        void DeleteProject(object obj)
        {
            if (MessageBox.Show("Вы уверены что хотите удалить проект?", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.No) return;
            db.Projects.Remove(SelectedProject);
            db.SaveChanges();
        }
        void AddArea(object obj)
        {
            var a = new Area() { Project=SelectedProject };
            db.Areas.Add(a);
            db.SaveChanges();
            a.Name = $"Площадь {a.Id}";
            db.SaveChanges();
            OnPropertyChanged(nameof(SelectedProject));
            SelectedArea = a;
        }
        void DeleteArea(object obj)
        {
            if (MessageBox.Show("Вы уверены что хотите удалить площадь?", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.No) return;
            db.Areas.Remove(SelectedArea);
            db.SaveChanges();
            OnPropertyChanged(nameof(SelectedProject));
            db.Areas.Load();
        }
        void OpenArea(object obj)
        {
            new AreaWindow() { 
                    DataContext=new AreaViewModel((Area)obj) 
                }.ShowDialog();
            OnPropertyChanged(nameof(SelectedProject.Areas));
            OnPropertyChanged(nameof(obj));
            Redraw();
        }
        void Zoom(object obj)
        {
            var e = (MouseWheelEventArgs)obj;
            var image = (Image)e.Source;

            double delta = e.Delta > 0 ? 0.1 : -0.1;
            double scaleX = image.RenderTransform.Value.M11 + delta;
            double scaleY = image.RenderTransform.Value.M22 + delta;

            if (scaleX < 1 || scaleY < 1) return;

            image.RenderTransform = new ScaleTransform(scaleX, scaleY);
            var vp = e.MouseDevice.GetPosition(image);
            image.RenderTransformOrigin = new Point(vp.X/image.ActualWidth, vp.Y/image.ActualHeight);
        }
        void Redraw()
        {
            var vis = new VisDraw();
            foreach (var area in SelectedProject?.Areas ?? new())
            {
                area.Draw(vis, area == SelectedArea ? Brushes.Yellow : (area.IsCorrect() ? Brushes.Green : Brushes.Red));
                foreach (var profile in area.Profiles ?? new())
                    profile.Draw(vis, area == SelectedArea ? Brushes.Yellow : (profile.IsCorrect() ? Brushes.Green : Brushes.Red));
            }
            Image = vis.Render();
        }
        public DrawingImage Image
        {
            get { return image; }
            set
            {
                image = value;
                OnPropertyChanged(nameof(Image));
            }
        }
        public Customer SelectedCustomer
        {
            get => selectedCustomer;
            set
            {
                selectedCustomer = value;
                OnPropertyChanged(nameof(SelectedCustomer));
            }
        }
        public Project SelectedProject
        {
            get => selectedProject;
            set
            {
                selectedProject = value;
                OnPropertyChanged(nameof(SelectedProject));
                Redraw();
            }
        }
        public Area SelectedArea
        {
            get => selectedArea;
            set
            {
                selectedArea = value;
                OnPropertyChanged(nameof(SelectedArea));
                Redraw();
            }
        }
        
    }
}
