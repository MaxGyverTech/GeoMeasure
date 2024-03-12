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
                    double d = pik.DistanceToLine(points[i].P, points[i+1].P);
                    if (d < minVal) { min = i; minVal = d; }
                }
                var proj = pik.Projection(points[min].P, points[min + 1].P);
                temp.Add((min, Distance(points[min].P, proj), proj,pik));
            }
            return temp.OrderBy(o => o.idx).OrderBy(o => o.dis).Select(t => (t.pi, t.pr)).ToList();
        }
        public bool IsCorrect()
        {
            for (int i = 0; i < points?.Count - 1; i++)
                for (int j = 0; j < Area.Points.Count; j++)
                    if (AreCrossing(points[i].P, points[i + 1].P, Area.Points[j].P, Area.Points[(j + 1) % Area.Points.Count].P))
                        return false;
            foreach (var pr in Area.Profiles)
                for (int i = 0; i < pr.Points?.Count - 1; i++)
                    for (int j = 0; j < points?.Count - 1; j++)
                        if (AreCrossing(pr.Points[i].P, pr.Points[i + 1].P, points[j].P, points[j + 1].P, colideSegments:pr==this?Math.Abs(i-j)>1:true))
                            return false;
            return true;
        }

        public bool AreCrossing(Point a1, Point a2, Point b1, Point b2, bool colideSegments = true)
        {
            double mult(double ax, double ay, double bx, double by) => ax * by - bx * ay;
            if ((mult(b2.X - b1.X, b2.Y - b1.Y, a1.X - b1.X, a1.Y - b1.Y) * mult(b2.X - b1.X, b2.Y - b1.Y, a2.X - b1.X, a2.Y - b1.Y)) < 0 &&
                (mult(a2.X - a1.X, a2.Y - a1.Y, b1.X - a1.X, b1.Y - a1.Y) * mult(a2.X - a1.X, a2.Y - a1.Y, b2.X - a1.X, b2.Y - a1.Y)) < 0) return true;
            if ((IsPointOnSegment(a1, b1, b2, colideSegments) || IsPointOnSegment(a2, b1, b2, colideSegments) ||
                 IsPointOnSegment(b1, a1, a2, colideSegments) || IsPointOnSegment(b2, a1, a2, colideSegments))) return true;
            return false;
        }

        static double Distance(Point point1, Point point2)
        {
            double dx = point2.X - point1.X;
            double dy = point2.Y - point1.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
        static bool IsPointOnSegment(Point p, Point l1, Point l2, bool colideSegments)
        {
            if (p.X == 5)
                p = p;
            if (!colideSegments && (p==l1 || p == l2)) return false;
            if (p.X >= Math.Min(l1.X, l2.X) && p.X <= Math.Max(l1.X, l2.X) &&
                p.Y >= Math.Min(l1.Y, l2.Y) && p.Y <= Math.Max(l1.Y, l2.Y))
            {
                //var v = Math.Abs((p.X - l1.X) * (l2.X - l1.X) - (l2.Y - l1.Y) * (p.Y - l1.Y));
                var v = Math.Abs((l1.X - p.X) * (l2.Y - p.Y) - (l1.Y - p.Y) * (l2.X - p.X));
                return !(v > 0.001);
            }
            return false;
        }
        public void Draw(VisDraw vd, Brush br)
        {
            if (points is null) return;
            vd.DrawPoly(points.Select(p => p.P).ToArray(), br, 0.4, false);
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
