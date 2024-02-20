using GeoMeasure.Models.Db;
using GeoMeasure.Models;
using GeoMeasure.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using System.Windows.Controls;

namespace GeoMeasure.ViewModels
{
    class ProfileViewModel : NotifyProperty
    {
        Database db = Database.getInstance();
        DrawingImage image;
        DrawingImage graphImage;

        Picket selectedPicket;
        ProfilePoint selectedPoint;
        public Profile Profile { get; set; }
        public ObservableCollection<Operator> Operators { get => db.Operators.Local.ToObservableCollection(); }
        public ProfileViewModel() : this(null) { }
        public ProfileViewModel(Profile prof)
        {
            Profile = prof;
            AddOperatorCommand = new(AddOperator);
            DeleteOperatorCommand = new(DeleteOperator, (o) => SelectedOperator != null);
            AddPointCommand = new(AddPoint);
            DeletePointCommand = new(DeletePoint, (o) => SelectedPoint != null);
            AddPicketCommand = new(AddPicket);
            DeletePicketCommand = new(DeletePicket, (o) => SelectedPicket != null);
            SavePicketCommand = new(SavePicket);
            SavePointCommand = new(SavePoint);
            ZoomCommand = new(Zoom);
            Redraw();
        }
        public RelayCommand AddOperatorCommand { get; set; }
        public RelayCommand DeleteOperatorCommand { get; set; }
        public RelayCommand AddPointCommand { get; set; }
        public RelayCommand DeletePointCommand { get; set; }
        public RelayCommand AddPicketCommand { get; set; }
        public RelayCommand DeletePicketCommand { get; set; }
        public RelayCommand SavePicketCommand { get; set; }
        public RelayCommand SavePointCommand { get; set; }
        public RelayCommand ZoomCommand { get; set; }
        void AddOperator(object obj)
        {
            var c = new Operator() { Name = "", Surname = "" };
            if (new OperatorWindow(c).ShowDialog() == false) return;
            db.Operators.Add(c);
            OnPropertyChanged(nameof(Operators));
            db.SaveChanges();
            SelectedOperator = c;
        }
        
        void DeleteOperator(object obj)
        {
            if (MessageBox.Show("Вы уверены что хотите удалить оператора?", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.No) return;
            db.Operators.Remove(SelectedOperator!);
            SelectedOperator = null!;
            db.SaveChanges();
        }
        void AddPoint(object obj)
        {
            var p = new ProfilePoint() { X = 0, Y = 0, Profile = Profile };
            db.ProfilePoints.Add(p);
            db.SaveChanges();
            SelectedPoint = p;
            OnPropertyChanged(nameof(Profile));
            Redraw();
        }
        void DeletePoint(object obj)
        {
            db.ProfilePoints.Remove(SelectedPoint);
            db.SaveChanges();
            Redraw();
        }
        void AddPicket(object obj)
        {
            var p = new Picket() { Profile = Profile };
            db.Pickets.Add(p);
            db.SaveChanges();
            SelectedPicket = p;
            OnPropertyChanged(nameof(Profile));
            Redraw();
        }
        void DeletePicket(object obj)
        {
            db.Pickets.Remove(SelectedPicket);
            db.SaveChanges();
            OnPropertyChanged(nameof(Profile));
            Redraw();
        }
        void SavePicket(object obj)
        {
            if (obj is Picket)
            {
                db.Entry((Picket)obj).State = EntityState.Modified;
                db.SaveChanges();
                Redraw();
            }
        }
        void SavePoint(object obj)
        {
            if (obj is ProfilePoint)
            {
                db.Entry((ProfilePoint)obj).State = EntityState.Modified;
                db.SaveChanges();
                Redraw();
            }
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
            image.RenderTransformOrigin = new Point(vp.X / image.ActualWidth, vp.Y / image.ActualHeight);
        }
        void Redraw()
        {
            var pickets = Profile.OrderPickets();

            var vd = new VisDraw();
            foreach (var p in pickets)
                vd.DrawLine(p.proj.X, p.proj.Y, p.pic.X, p.pic.Y, Brushes.Orange, 0.2);
            Profile.Draw(vd, Brushes.Green);
            foreach (var p in Profile.Points ?? new())
                vd.DrawCircle(p.X, p.Y, 0.5, SelectedPoint == p ? Brushes.Yellow : Brushes.Green);
            foreach (var p in Profile.Pickets ?? new())
                vd.DrawCircle(p.X, p.Y, 0.5, p == SelectedPicket ? Brushes.Yellow : Brushes.Orange);
            Image = vd.Render();


            var graph = new VisDraw();
            graph.DrawPoly(pickets.Select((v, i) => new Point(i * 10, v.pic.Ra)).ToList(), Brushes.Orange, 0.3, false);
            graph.DrawPoly(pickets.Select((v, i) => new Point(i * 10, v.pic.Th)).ToList(), Brushes.Green, 0.3, false);
            graph.DrawPoly(pickets.Select((v, i) => new Point(i * 10, v.pic.K)).ToList(), Brushes.Blue, 0.3, false);
            if (SelectedPicket != null)
                for (int i= 0; i < pickets.Count; i++)
                    if (pickets[i].pic == SelectedPicket)
                    {
                        graph.DrawCircle(i*10, pickets[i].pic.Ra, 0.5, Brushes.Yellow);
                        graph.DrawCircle(i*10, pickets[i].pic.Th, 0.5, Brushes.Yellow);
                        graph.DrawCircle(i*10, pickets[i].pic.K, 0.5, Brushes.Yellow);
                    }
            GraphImage = graph.Render(drawAxies: true);
        }
        public Operator? SelectedOperator
        {
            get=> Profile.Operator;
            set
            {
                Profile.Operator = value;
                db.Entry(Profile).State = EntityState.Modified;
                db.SaveChanges();
            }
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
        public DrawingImage GraphImage
        {
            get { return graphImage; }
            set
            {
                graphImage = value;
                OnPropertyChanged(nameof(GraphImage));
            }
        }
        public Picket SelectedPicket
        {
            get => selectedPicket;
            set
            {
                selectedPicket = value;
                OnPropertyChanged(nameof(SelectedPicket));
                Redraw();
            }
        }
        public ProfilePoint SelectedPoint
        {
            get => selectedPoint;
            set
            {
                selectedPoint = value;
                OnPropertyChanged(nameof(SelectedPoint));
                Redraw();
            }
        }
    }
}
