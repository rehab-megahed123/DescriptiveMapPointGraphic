using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ArcGIS.Core.Geometry;

namespace Descriptive_Map_Point_Graphic.Models
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using ArcGIS.Core.Geometry;
    using System.Windows.Input;

    public class DescriptivePoint : INotifyPropertyChanged
    {
        private string _description;
        private MapPoint _location;

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        public string LocationText => $"X: {_location?.X:F2}, Y: {_location?.Y:F2}";

        public MapPoint Location
        {
            get => _location;
            set
            {
                if (_location != value)
                {
                    _location = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(LocationText));
                }
            }
        }

        public ICommand EditCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

}
