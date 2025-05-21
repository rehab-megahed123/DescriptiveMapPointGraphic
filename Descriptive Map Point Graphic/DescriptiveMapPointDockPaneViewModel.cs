﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Extensions;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.KnowledgeGraph;

namespace Descriptive_Map_Point_Graphic
{
    public class DescriptiveMapPointDockPaneViewModel : DockPane
    {   
      private const string _dockPaneID = "Descriptive_Map_Point_Graphic_DescriptiveMapPointDockPane";
 
      public DescriptiveMapPointDockPaneViewModel() { }  

      /// <summary>
      /// Show the DockPane.
      /// </summary>
      internal static void Show()
      {        
        DockPane pane = FrameworkApplication.DockPaneManager.Find(_dockPaneID);
        if (pane == null)
          return;

        pane.Activate();
      }

      /// <summary>
      /// Text shown near the top of the DockPane.
      /// </summary>
		  private string _heading = "My DockPane";
      public string Heading
      {
        get => _heading; 
        set => SetProperty(ref _heading, value);
      }
    }

  /// <summary>
  /// Button implementation to show the DockPane.
  /// </summary>
	internal class DescriptiveMapPointDockPane_ShowButton : Button
	{
		protected override void OnClick()
		{
			DescriptiveMapPointDockPaneViewModel.Show();
		}
  }	
}
