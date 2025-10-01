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
