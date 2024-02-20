using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoMeasure.Models.Db
{
    public class Operator : NotifyProperty
    {
        int id;
        string name;
        string surname;
        ObservableCollection<Profile> profiles;
        public override string ToString()
        {
            return $"{name} {surname}";
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
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Surname
        {
            get { return surname; }
            set
            {
                surname = value;
                OnPropertyChanged(nameof(Surname));
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
    }
}
