using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using Point = System.Windows.Point;

namespace GeoMeasure.Models.Db
{
    public class Profile : NotifyProperty
    {
        int id;
        Area area;
        Operator? _operator;
        ObservableCollection<Picket> pickets;
        ObservableCollection<ProfilePoint> points;
        public List<(Picket pic, Point proj)> OrderPickets()
        {
            if (points is null || pickets is null || points.Count < 2) return new();
            var temp = new List<(int idx, double dis, Point pr,Picket pi)>();
            foreach (var pik in pickets)
            {
                int min=0;
                double minVal = double.MaxValue;
                for(int i = 0; i < points.Count-1; i++)
                {
                    double d = pik.Distance(points[i].AsPoint, points[i+1].AsPoint);
                    if (d < minVal) { min = i; minVal = d; }
                }
                var proj = pik.Projection(points[min].AsPoint, points[min + 1].AsPoint);
                temp.Add((min, Distance(points[min].AsPoint, proj), proj,pik));
            }
            return temp.OrderBy(o => o.idx).OrderBy(o => o.dis).Select(t => (t.pi, t.pr)).ToList();
        }
        static double Distance(Point point1, Point point2)
        {
            double dx = point2.X - point1.X;
            double dy = point2.Y - point1.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
        public void Draw(VisDraw vd, Brush br)
        {
            if (points is null) return;
            vd.DrawPoly(points.Select(p => p.AsPoint).ToArray(), br, 0.4, false);
            foreach (var p in points)
                vd.DrawText($"{p.X},{p.Y}", p.X, p.Y, Brushes.Black, 1.3);
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

        public Area Area
        {
            get { return area; }
            set
            {
                area = value;
                OnPropertyChanged(nameof(Area));
            }
        }
        public Operator? Operator
        {
            get { return _operator; }
            set
            {
                _operator = value;
                OnPropertyChanged(nameof(Operator));
            }
        }
        public ObservableCollection<Picket> Pickets
        {
            get { return pickets; }
            set
            {
                pickets = value;
                OnPropertyChanged(nameof(Pickets));
            }
        }
        public ObservableCollection<ProfilePoint> Points
        {
            get { return points; }
            set
            {
                points = value;
                OnPropertyChanged(nameof(Points));
            }
        }
    }

}
