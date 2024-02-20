using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GeoMeasure.Models.Db
{
    public class AreaPoint : NotifyProperty
    {
        int id;
        double x;
        double y;
        Area area;
        public override string ToString()
        {
            return $"{X},{Y}";
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
        public Area Area
        {
            get { return area; }
            set
            {
                area = value;
                OnPropertyChanged(nameof(Area));
            }
        }
        [NotMapped]
        public Point AsPoint => new Point(x, y);
    }
}
