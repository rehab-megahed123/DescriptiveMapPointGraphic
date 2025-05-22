using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ArcGIS.Core.Geometry;
using ArcGIS.Core.Internal.Geometry;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;

namespace Descriptive_Map_Point_Graphic
{
    public class DescriptiveMapPointDockPaneViewModel : DockPane
    {
        public DescriptiveMapPointDockPaneViewModel()
        {
            _points = new ObservableCollection<DescriptivePoint>();

            _points.Add(new DescriptivePoint
            {
                Description = "Test Point 1",
                Location = MapPointBuilder.CreateMapPoint(30.05, 31.23)
            });
        }
        public const string _dockPaneID = "Descriptive_Map_Point_Graphic_DescriptiveMapPointDockPane";

        private string _descriptionText;
        private string _heading = "Descriptive Points";
        private ObservableCollection<DescriptivePoint> _points = new ObservableCollection<DescriptivePoint>();
        private bool _isEditing;
        private DescriptivePoint _currentEditPoint;
        private RelayCommand _addPointCommand;
       
        public string DescriptionText
        {
            get => _descriptionText;
            set
            {
                SetProperty(ref _descriptionText, value);
                
            }
        }

        public string Heading
        {
            get => _heading;
            set => SetProperty(ref _heading, value);
        }

        //private ObservableCollection<DescriptivePoint> _points = new ObservableCollection<DescriptivePoint>();

        public ObservableCollection<DescriptivePoint> Points
        {
            get => _points;
            set => SetProperty(ref _points, value);
        }

        public ICommand AddPointCommand => _addPointCommand ?? (_addPointCommand = new RelayCommand(OnAddPoint, CanExecuteAddPoint));
        public ICommand EditPointCommand => new RelayCommand(() => OnEditPoint(_currentEditPoint), () => _currentEditPoint != null);
        public ICommand ShowCountCommand => new RelayCommand(() =>
        {
            MessageBox.Show($"Total points: {_points.Count}");
        });

        private bool CanExecuteAddPoint()
        {
            return !string.IsNullOrWhiteSpace(DescriptionText);
        }

        private async void OnAddPoint()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(DescriptionText))
                {
                    MessageBox.Show("Please enter a description for the point.",
                                    "Input Required",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                    return;
                }

                _isEditing = false;
                _currentEditPoint = null;

                await FrameworkApplication.SetCurrentToolAsync("Descriptive_Map_Point_Graphic_AddPointTool");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error activating tool: {ex.Message}");
            }
        }

        private void OnEditPoint(DescriptivePoint point)
        {
            if (point == null) return;

            _isEditing = true;
            _currentEditPoint = point;
            DescriptionText = point.Description;
        }

        public async Task AddOrUpdatePoint(MapPoint mapPoint)
        {
            // Store these in temp variables to use inside the background thread
            string description = DescriptionText;
            bool isEdit = _isEditing;
            var editPoint = _currentEditPoint;

            // Do the map-related or heavy logic inside QueuedTask
            await QueuedTask.Run(() =>
            {
                if (isEdit && editPoint != null)
                {
                    editPoint.Description = description;
                    editPoint.Location = mapPoint;
                }
                else
                {
                    var newPoint = new DescriptivePoint
                    {
                        Description = description,
                        Location = mapPoint
                    };

                    // Set EditCommand outside QueuedTask to avoid threading issues
                    // But we temporarily store it here
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        newPoint.EditCommand = new RelayCommand(() => OnEditPoint(newPoint));
                        _points.Add(newPoint);
                        Points = _points;
                        NotifyPropertyChanged(nameof(Points));
                    });
                }
            });

            // Reset fields on UI thread
            Application.Current.Dispatcher.Invoke(() =>
            {
                DescriptionText = string.Empty;
                _isEditing = false;
                _currentEditPoint = null;
                
                NotifyPropertyChanged(nameof(Points));
            });
        }
        internal static void Show()
        {
            DockPane pane = FrameworkApplication.DockPaneManager.Find(_dockPaneID);
            pane?.Activate();
        }
    }
}