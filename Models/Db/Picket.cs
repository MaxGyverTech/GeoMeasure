using System.Windows;

namespace GeoMeasure.Models.Db
{
    public class Picket : NotifyProperty
    {
        int id;
        Profile profile;
        double x, y, ra, th, k;
        public Point Projection(Point p1, Point p2)
        {
            double dx = p2.X - p1.X;
            double dy = p2.Y - p1.Y;
            double t = ((x - p1.X) * dx + (y - p1.Y) * dy) / (dx * dx + dy * dy);
            t = Math.Max(0, Math.Min(1, t));
            return new Point(p1.X + t * dx, p1.Y + t * dy);
        }

        public double Distance(Point p1, Point p2)
        {
            double dx = p2.X - p1.X;
            double dy = p2.Y - p1.Y;
            var v = ((x - p1.X) * dx + (y - p1.Y) * dy) / (dx * dx + dy * dy);
            v = Math.Max(0, Math.Min(1, v));
            return Math.Sqrt(Math.Pow(p1.X - x + dx * v, 2) + Math.Pow(p1.Y - y + dx*v, 2));
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

        public Profile Profile
        {
            get { return profile; }
            set
            {
                profile = value;
                OnPropertyChanged(nameof(Profile));
            }
        }

        public double X
        {
            get { return x; }
            set
            {
                x = value;
                OnPropertyChanged(nameof(X));
            }
        }

        public double Y
        {
            get { return y; }
            set
            {
                y = value;
                OnPropertyChanged(nameof(Y));
            }
        }

        public double Ra
        {
            get { return ra; }
            set
            {
                this.ra = value;
                OnPropertyChanged(nameof(Ra));
            }
        }
        public double Th
        {
            get { return th; }
            set
            {
                this.th = value;
                OnPropertyChanged(nameof(Th));
            }
        }
        public double K
        {
            get { return k; }
            set
            {
                this.k = value;
                OnPropertyChanged(nameof(K));
            }
        }
    }
}