using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Geometry;
using ArcGIS.Core.Internal.Geometry;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using DocumentFormat.OpenXml.Drawing;

namespace Descriptive_Map_Point_Graphic
{
    public class DescriptiveMapPointDockPaneViewModel : DockPane
    {
        public const string _dockPaneID = "Descriptive_Map_Point_Graphic_DescriptiveMapPointDockPane";

        private string _descriptionText;
        private string _heading = "Descriptive Points";
        private ObservableCollection<DescriptivePoint> _points = new ObservableCollection<DescriptivePoint>();
        private bool _isEditing;
        private DescriptivePoint _currentEditPoint;
        private RelayCommand _addPointCommand;
        private CIMSymbolReference _tempSymbol;
        private Graphic _editGraphic;
        private IDisposable _editOverlayGraphic;

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

            QueuedTask.Run(() =>
            {
                MapView.Active?.Map?.SetSelection(null);
                MapView.Active.ZoomTo(point.Location, TimeSpan.FromSeconds(1));

                // امسح أي overlay قديم قبل إضافة جديد
                _editOverlayGraphic?.Dispose();

                var graphic = new CIMSymbolReference()
                {
                    Symbol = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.BlueRGB, 10)
                };
                _editOverlayGraphic = MapView.Active.AddOverlay(point.Location, graphic);
            });
        }

        public async Task AddOrUpdatePoint(MapPoint mapPoint)
        {
            string description = DescriptionText;
            bool isEdit = _isEditing;
            var editPoint = _currentEditPoint;

            await QueuedTask.Run(() =>
            {
                if (isEdit && editPoint != null)
                {
                    // نافذة التأكيد على الـ UI thread
                    bool? confirm = null;
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        var result = MessageBox.Show("Do you want to save the changes to this point?",
                                                     "Confirm Edit",
                                                     MessageBoxButton.YesNo,
                                                     MessageBoxImage.Question);
                        confirm = result == MessageBoxResult.Yes;
                    });

                    if (confirm != true)
                    {
                        // المستخدم لغى الحفظ
                        return;
                    }

                    // تحديث البيانات
                    editPoint.Description = description;
                    editPoint.Location = mapPoint;

                    // امسح overlay القديم وأضف الجديد بالموقع الجديد
                    _editOverlayGraphic?.Dispose();
                    var graphic = new CIMSymbolReference()
                    {
                        Symbol = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.BlueRGB, 10)
                    };
                    _editOverlayGraphic = MapView.Active.AddOverlay(mapPoint, graphic);
                }
                else
                {
                    var newPoint = new DescriptivePoint
                    {
                        Description = description,
                        Location = mapPoint
                    };

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        newPoint.EditCommand = new RelayCommand(() => OnEditPoint(newPoint));
                        _points.Add(newPoint);
                        Points = _points;
                        NotifyPropertyChanged(nameof(Points));
                    });
                }
            });

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
