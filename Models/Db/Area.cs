using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GeoMeasure.Models.Db
{
    public class Area : NotifyProperty
    {
        int id;
        string? name;
        Project project;
        ObservableCollection<Profile> profiles;
        ObservableCollection<AreaPoint> points;
        double area;
        public override string ToString()
        {
            return name is null? $"Площадь #{id}" : name;
        }
        public void Draw(VisDraw gd, Brush br)
        {
            if (points is null) return;
            gd.DrawPoly(points.Select(p => p.P).ToArray(), br, 0.5, true);
            foreach (var p in points)
                gd.DrawText($"{p.X},{p.Y}", p.X, p.Y, Brushes.Black,1.5);

        }
        [NotMapped]
        public double CalcArea
        {
            get
            {
                double area = 0;
                if (points == null || points.Count < 3) return 0;

                for (int i = 0; i < points.Count; i++)
                {
                    int j = (i + 1) % points.Count;
                    area += points[i].X * points[j].Y;
                    area -= points[j].X * points[i].Y;
                }
                area /= 2.0;
                return Math.Abs(area);
            }
            set => OnPropertyChanged(nameof(CalcArea));
        }
        public bool IsCorrect()
        {
            for (int i = 0; i < points?.Count; i++)
                for (int j = i + 1; j < points.Count; j++)
                    if (AreCrossing(points[i], points[(i + 1)%points.Count], points[j], points[(j + 1) % points.Count]))
                        return false;
            return true;
        }               

        public bool AreCrossing(AreaPoint p1, AreaPoint p2, AreaPoint p3, AreaPoint p4) 
        {
            double mult(double ax, double ay, double bx, double by) => ax * by - bx * ay;
            return ((mult(p4.X-p3.X, p4.Y-p3.Y, p1.X-p3.X, p1.Y-p3.Y) * mult(p4.X-p3.X, p4.Y-p3.Y, p2.X-p3.X, p2.Y-p3.Y)) < 0 && 
                    (mult(p2.X-p1.X, p2.Y-p1.Y, p3.X-p1.X, p3.Y-p1.Y) * mult(p2.X-p1.X, p2.Y-p1.Y, p4.X-p1.X, p4.Y-p1.Y)) < 0);
        }
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
        public string? Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public Project Project
        {
            get { return project; }
            set
            {
                project = value;
                OnPropertyChanged(nameof(Project));
            }
        }

        public ObservableCollection<Profile> Profiles
        {
            get { return profiles; }
            set
            {
                profiles = value;
                OnPropertyChanged(nameof(Profiles));
            }
        }
        public ObservableCollection<AreaPoint> Points
        {
            get { return points; }
            set
            {
                points = value;
                OnPropertyChanged(nameof(Points));
                OnPropertyChanged(nameof(CalcArea));
            }
        }

    }

}
