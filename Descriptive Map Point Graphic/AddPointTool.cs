using System.Threading.Tasks;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;

namespace Descriptive_Map_Point_Graphic
{

    internal class AddPointTool : MapTool
    {
        public AddPointTool()
        {
            IsSketchTool = true;
            SketchType = SketchGeometryType.Point;
            SketchOutputMode = SketchOutputMode.Map;
        }

        protected override Task OnToolActivateAsync(bool active)
        {
            return base.OnToolActivateAsync(active);
        }

        protected override async Task<bool> OnSketchCompleteAsync(Geometry geometry)
        {
            if (geometry == null)
                return false;

            await QueuedTask.Run(() =>
            {
                System.Diagnostics.Debug.WriteLine("Sketch complete triggered");
            });

            if (geometry is MapPoint mapPoint)
            {
                var dockpane = FrameworkApplication.DockPaneManager.Find(
                    DescriptiveMapPointDockPaneViewModel._dockPaneID) as DescriptiveMapPointDockPaneViewModel;

                if (dockpane != null)
                {
                    await dockpane.AddOrUpdatePoint(mapPoint);
                }
            }
            return true;
        }
    }
}