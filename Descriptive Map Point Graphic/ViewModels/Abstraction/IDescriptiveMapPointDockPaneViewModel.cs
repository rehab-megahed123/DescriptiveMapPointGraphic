using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Core.Geometry;
using System.Windows.Input;
using Descriptive_Map_Point_Graphic.Models;

namespace Descriptive_Map_Point_Graphic.ViewModels.Abstraction
{
    public interface IDescriptiveMapPointDockPaneViewModel
    {
        ObservableCollection<DescriptivePoint> Points { get; set; }
        string DescriptionText { get; set; }

        ICommand AddPointCommand { get; }
        ICommand EditPointCommand { get; }
        ICommand ShowCountCommand { get; }

        Task AddOrUpdatePoint(MapPoint mapPoint);
    }
}
