using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoMeasure.Models.Db
{
    public class Customer : NotifyProperty
    {
        int id;
        string name;
        string phone;
        ObservableCollection<Project> projects;
        public override string ToString()
        {
            return  $"{name}, {phone}";
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

        public string Phone
        {
            get { return phone; }
            set
            {
                phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }

        public ObservableCollection<Project> Projects
        {
            get { return projects; }
            set
            {
                projects = value;
                OnPropertyChanged(nameof(Projects));
            }
        }
    }
}
