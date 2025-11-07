namespace WSPPolska_Tools
{
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using System;
    using System.IO;
    using System.Reflection;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Plugin's main entry point
    /// </summary>
    /// <seealso cref="Autodesk.Revit.UI.IExternalApplication"/>
    public class Main : IExternalApplication
    {
        /// <summary>
        /// Calleds when startup
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        
        
        //Handlers For Transactions 
        //DeleteHandler
        public static ExternalEvent deleteEvent { get; set; }
        public static DeleteElementsHandler deleteHandler { get; set; }
        
        //Change Active Location
        public static ExternalEvent changeLocEvent { get; set; }
        public static ChangeLocationHandler changeLocHandler { get; set; }

        //Update Locations
        public static ExternalEvent updateLocEvent { get; set; }
        public static UpdateLocationHandler updateLocHandler { get; set; }

        //Update Locations
        public static ExternalEvent exportLocEvent { get; set; }
        public static ExportLocationsHandler exportLocHandler { get; set; }

        //Create Spheres
        public static ExternalEvent createSpheresEvent { get; set; }
        public static CreateSphereHandler createSpheresHandler { get; set; }

        //Create Spheres
        public static ExternalEvent createWorksetEvent { get; set; }
        public static CreateWorksetHandler createWorksetHandler { get; set; }

        public Result OnStartup(UIControlledApplication application)
        {
            string tabName = "WSP Polska Tools";
            application.CreateRibbonTab(tabName);
            string addinFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            //Create panel for discipline params
            string paramPanelName = "WSP Params";
            var paramPanel = application.CreateRibbonPanel(tabName, paramPanelName);
            //New buttons in Discipline panels
            var VentParametersData = new PushButtonData("Vent Params", "Vent Params", Assembly.GetExecutingAssembly().Location, "WSPPolska_Tools.VentParametersCommand")
            
            { 
                ToolTipImage = new BitmapImage(new Uri(Path.Combine(addinFolder, "res", "WSP355x355.png"))),
                ToolTip = "WSP Ventilation parameters script"
            };
            
            var VParameters = paramPanel.AddItem(VentParametersData) as PushButton;
            VParameters.LargeImage = new BitmapImage(new Uri(Path.Combine(addinFolder, "res", "fan32x32.png")));

            var HCParametersData = new PushButtonData("HC Params", "HC Params", Assembly.GetExecutingAssembly().Location, "WSPPolska_Tools.HCParametersCommand")
            {
                ToolTipImage = new BitmapImage(new Uri(Path.Combine(addinFolder, "res", "WSP355x355.png"))),
                ToolTip = "WSP Heating and Cooling parameters script"
            };
            
            var HParameters = paramPanel.AddItem(HCParametersData) as PushButton;
            HParameters.LargeImage = new BitmapImage(new Uri(Path.Combine(addinFolder, "res", "heater32x32.png")));

            var PlumbParametersData = new PushButtonData("Plumbing Params", "Plumbing Params", Assembly.GetExecutingAssembly().Location, "WSPPolska_Tools.PlumbParametersCommand")
            {
                ToolTipImage = new BitmapImage(new Uri(Path.Combine(addinFolder, "res", "WSP355x355.png"))),
                ToolTip = "WSP Plumbing parameters script"
            };


            var PlumbParameters = paramPanel.AddItem(PlumbParametersData) as PushButton;
            PlumbParameters.LargeImage = new BitmapImage(new Uri(Path.Combine(addinFolder, "res", "plumbing32x32.png")));

            //Second panel - other tools
            string otherPanelName = "WSP Other Tools";
            var otherPanel = application.CreateRibbonPanel(tabName, otherPanelName);
            var SplitData = new PushButtonData("Split Elements", "Split Elements", Assembly.GetExecutingAssembly().Location, "WSPPolska_Tools.SplitElements")
            {
                ToolTipImage = new BitmapImage(new Uri(Path.Combine(addinFolder, "res", "WSP355x355.png"))),
                ToolTip = "Splitting elements by levels with Building Story"
            };

            var SplitButton = otherPanel.AddItem(SplitData) as PushButton;
            SplitButton.LargeImage = new BitmapImage(new Uri(Path.Combine(addinFolder, "res", "split32x32.png")));
            
            var StructuralCostData = new PushButtonData("Structural Cost", "Structural Cost", Assembly.GetExecutingAssembly().Location, "WSPPolska_Tools.StructureCostFormCommand")
            {
                ToolTipImage = new BitmapImage(new Uri(Path.Combine(addinFolder, "res", "WSP355x355.png"))),
                ToolTip = "Analysis of the cost of selected Elements"
            };

            var StructuralCostButton = otherPanel.AddItem(StructuralCostData) as PushButton;
            StructuralCostButton.LargeImage = new BitmapImage(new Uri(Path.Combine(addinFolder, "res", "strucCost32x32.png")));

            //Geodata Panel inport
            string geoLocationPanelName = "Geolocation Data";
            RibbonPanel geoLocationPanel = application.CreateRibbonPanel(tabName, geoLocationPanelName);
            var GeoLocationData = new PushButtonData("Geo Manipulation", "Get and Set Geolocation", Assembly.GetExecutingAssembly().Location, "WSPPolska_Tools.GeolocationFormCommand")
            {
                ToolTipImage = new BitmapImage(new Uri(Path.Combine(addinFolder, "res", "WSP355x355.png"))),
                ToolTip = "Exporting and Importing Geolocation data to/from Excel"
            };
            PushButton GeoLocationButton = geoLocationPanel.AddItem(GeoLocationData) as PushButton;
            GeoLocationButton.LargeImage = new BitmapImage(new Uri(Path.Combine(addinFolder, "res", "currentLocation32x32.png")));

            var ExportLocationsData = new PushButtonData("Export Locations", "Export Locations", Assembly.GetExecutingAssembly().Location, "WSPPolska_Tools.ExportLocationsFormCommand")
            {
                ToolTipImage = new BitmapImage(new Uri(Path.Combine(addinFolder, "res", "WSP355x355.png"))),
                ToolTip = " to IFC, NWC, DWFx with selected settings and file name"
            };
            PushButton ExportLocationButton = geoLocationPanel.AddItem(ExportLocationsData) as PushButton;
            ExportLocationButton.LargeImage = new BitmapImage(new Uri(Path.Combine(addinFolder, "res", "locationShare32x32.png")));


            //Mechanical Equipment Panel inport
            string equipmentExportPanelName = "Equipment Schedule";
            RibbonPanel equipmentExportPanel = application.CreateRibbonPanel(tabName, equipmentExportPanelName);
            var equipmentExportData = new PushButtonData("Export Equipment", "Export Equipment Numbers", Assembly.GetExecutingAssembly().Location, "WSPPolska_Tools.ExportEquipmentNuFormCommand")
            {
                ToolTipImage = new BitmapImage(new Uri(Path.Combine(addinFolder, "res", "WSP355x355.png"))),
                ToolTip = "Exporting and Importing Equipment Numbers with Excel"
            };

            PushButton equipmentExportButton = equipmentExportPanel.AddItem(equipmentExportData) as PushButton;
            equipmentExportButton.LargeImage = new BitmapImage(new Uri(Path.Combine(addinFolder, "res", "eq32x32.png")));

            //Coordination Panel inport
            string coordinationPanelName = "Coordination Panel";
            RibbonPanel coordinationPanel = application.CreateRibbonPanel(tabName, coordinationPanelName);
            var coordinationSpheresData = new PushButtonData("Create Spheres", "Create Coordination Spheres", Assembly.GetExecutingAssembly().Location, "WSPPolska_Tools.CreateCoordinationSpheresCommand")
            {
                ToolTipImage = new BitmapImage(new Uri(Path.Combine(addinFolder, "res", "WSP355x355.png"))),
                ToolTip = "Exporting and Importing Equipment Numbers with Excel"
            };

            PushButton createSpheresButton = coordinationPanel.AddItem(coordinationSpheresData) as PushButton;
            createSpheresButton.LargeImage = new BitmapImage(new Uri(Path.Combine(addinFolder, "res", "coordSph32x32.png")));


            //Transaction definition
            //delete elements
            deleteHandler = new DeleteElementsHandler();
            deleteEvent = ExternalEvent.Create(deleteHandler);
            //change Active Location  
            changeLocHandler = new ChangeLocationHandler();
            changeLocEvent = ExternalEvent.Create(changeLocHandler);
            //update Locations
            updateLocHandler = new UpdateLocationHandler();
            updateLocEvent = ExternalEvent.Create(updateLocHandler);
            //update Locations
            exportLocHandler = new ExportLocationsHandler();
            exportLocEvent = ExternalEvent.Create(exportLocHandler);
            //create Spheres 
            createSpheresHandler = new CreateSphereHandler();
            createSpheresEvent = ExternalEvent.Create(createSpheresHandler);
            //create Workset 
            createWorksetHandler = new CreateWorksetHandler();
            createWorksetEvent = ExternalEvent.Create(createWorksetHandler);



            return Result.Succeeded;
        }
        /// <summary>
        /// Called when Revit shutdown
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        
    }
}
