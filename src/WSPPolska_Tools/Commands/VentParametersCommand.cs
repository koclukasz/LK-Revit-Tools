namespace WSPPolska_Tools
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Autodesk.Revit.UI;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Mechanical;
    using System.Diagnostics;
    using System;

    /// <summary>
    /// Command to Be executed when button clicked
    /// </summary>
    /// <seealso cref="Autodesk.Revit.UI.IExternalCommand"></seealso>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]


    public class VentParametersCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            
            /// Collecting Levels and sorting by elevation
            ICollection<Element> allLevels = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Levels).WhereElementIsNotElementType().ToElements();
            Dictionary<Level, double> levelEle = new Dictionary<Level, double>();
            foreach (Level level in allLevels)
            levelEle.Add(level, level.Elevation);
            var levelEleSorted = from entry in levelEle orderby entry.Value ascending select entry;
            Dictionary<Level, double> sortedLevels = levelEleSorted.ToDictionary(pair => pair.Key, pair => pair.Value);

            ///Collecting DUCTS and creating list of not assigned
            ICollection<Element> viewDucts = new FilteredElementCollector(doc, doc.ActiveView.Id).OfCategory(BuiltInCategory.OST_DuctCurves).WhereElementIsNotElementType().ToElements();
            List<Element> notAssignedDucts = new List<Element>();

            ///Collecting FLEX DUCTS and creating list of not assigned
            ICollection<Element> viewFlexDucts = new FilteredElementCollector(doc, doc.ActiveView.Id).OfCategory(BuiltInCategory.OST_FlexDuctCurves).WhereElementIsNotElementType().ToElements();
            List<Element> notAssignedFlexDucts = new List<Element>();

            ///Collecting DUCTS INSULATION and creating list of not assigned
            ICollection<Element> viewDuctInsulations = new FilteredElementCollector(doc, doc.ActiveView.Id).OfCategory(BuiltInCategory.OST_DuctInsulations).WhereElementIsNotElementType().ToElements();
            List<Element> notAssignedDuctInsulations = new List<Element>();

            ///Collecting DUCTS ACCESSORY and creating list of not assigned
            ICollection<Element> viewDuctAccessory = new FilteredElementCollector(doc, doc.ActiveView.Id).OfCategory(BuiltInCategory.OST_DuctAccessory).WhereElementIsNotElementType().ToElements();
            List<Element> notAssignedDuctAccessory = new List<Element>();

            ///Collecting DUCTS TERMINAL and creating list of not assigned
            ICollection<Element> viewDuctTerminal = new FilteredElementCollector(doc, doc.ActiveView.Id).OfCategory(BuiltInCategory.OST_DuctTerminal).WhereElementIsNotElementType().ToElements();
            List<Element> notAssignedDuctTerminal = new List<Element>();

            ///Collecting MECHANICAL EQUIPMENT and creating list of not assigned
            ICollection<Element> viewMechanicalE = new FilteredElementCollector(doc, doc.ActiveView.Id).OfCategory(BuiltInCategory.OST_MechanicalEquipment).WhereElementIsNotElementType().ToElements();
            List<Element> notAssignedMechanicalE = new List<Element>();

            ///Collecting DUCTS FITTINGS and creating list of not assigned
            ICollection<Element> viewDuctFitting = new FilteredElementCollector(doc, doc.ActiveView.Id).OfCategory(BuiltInCategory.OST_DuctFitting).WhereElementIsNotElementType().ToElements();
            List<Element> notAssignedDuctFitting = new List<Element>();


            using (Transaction t = new Transaction(doc, "CommentParam"))
            {
                t.Start("Comm");
                notAssignedDucts = AssignForDucts(viewDucts, doc, sortedLevels);
                notAssignedFlexDucts = AssignForFlexDucts(viewFlexDucts, doc, sortedLevels);
                notAssignedDuctFitting = AssignForDuctFittings(viewDuctFitting, doc, sortedLevels);
                notAssignedDuctAccessory = AssignForDuctAccesories(viewDuctAccessory, doc, sortedLevels);
                notAssignedDuctTerminal = AssignForDuctTerminal(viewDuctTerminal, doc, sortedLevels);
                notAssignedMechanicalE = AssignForMechanicalE(viewMechanicalE, doc, sortedLevels);
                notAssignedDuctInsulations = AssignForDuctInsulation(viewDuctInsulations, doc);
                t.Commit();
            }
         
            
            stopwatch.Stop();
            TaskDialog.Show("Values", "Time " + stopwatch.ElapsedMilliseconds/1000 + "s \n" + 
            "Not assigned to: \nDucts\n" + SB(notAssignedDucts) 
            +"FlexDucts \n" + SB(notAssignedFlexDucts)
            +"DuctFittings \n" + SB(notAssignedDuctFitting)
            +"DuctsInsulation \n" + SB(notAssignedDuctInsulations) 
            +"DuctAccessory \n" + SB(notAssignedDuctAccessory) 
            +"AirTerminals \n" + SB(notAssignedDuctTerminal) 
            +"MechanicalEqipment \n" + SB(notAssignedMechanicalE));

            return Result.Succeeded;
        }
        
       /// Methods for Ducts
   
        static List<Element> AssignForDucts(ICollection<Element> ducts, Document doc, Dictionary<Level, double> sortedLevels)
        {
            List<Element> notAssigned = new List<Element>();
            foreach (Duct duct in ducts)
            {
                Parameter parameterWSPLevel = duct.LookupParameter("WSP_Level");
                Parameter parameterWSPLength = duct.LookupParameter("WSP_Length");
                Parameter parameterWSPSystem = duct.LookupParameter("WSP_System");
                Parameter parameterWSPWidth = duct.LookupParameter("WSP_Width");
                Parameter parameterWSPHeight = duct.LookupParameter("WSP_Height");
                Parameter parameterWSPDiameter = duct.LookupParameter("WSP_Diameter");
                Parameter parameterWSPArea = duct.LookupParameter("WSP_Area");
                try
                {
                    parameterWSPLength.Set(duct.LookupParameter("Length").AsDouble());
                    parameterWSPSystem.Set(duct.LookupParameter("System Name").AsString());
                    List<double> ductShape = ShapeChecker(duct);
                    if (ductShape[0] == 1)
                    {
                        parameterWSPDiameter.Set(ductShape[1]);
                        parameterWSPArea.Set(ductShape[2]);
                    }
                    else
                    {
                        parameterWSPWidth.Set(ductShape[1]);
                        parameterWSPHeight.Set(ductShape[2]);
                        parameterWSPArea.Set(ductShape[3]);
                    }
                    double elementEle = GetDuctLevel(duct, doc);
                    Level correctLevel = SearchForLevel(sortedLevels, elementEle);
                    parameterWSPLevel.Set(correctLevel.Name);
                }
                catch 
                {
                    notAssigned.Add(duct);
                }
            }
            return notAssigned;
        }

        static double GetDuctLevel(Duct duct, Document doc)
        {
            double elevation = 0;
            ElementId refLevelId = duct.LookupParameter("Reference Level").AsElementId();
            Level refLevel = doc.GetElement(refLevelId) as Level;
            double levelEl = refLevel.Elevation;
            double ductOffset = duct.LookupParameter("Middle Elevation").AsDouble();
            elevation = elevation + levelEl + ductOffset;
            return elevation;
        }

        static List<double> ShapeChecker(Duct duct)
        {
            List<double> res = new List<double>();
            ConnectorSet ductConnectorSet = duct.ConnectorManager.Connectors;
            List<Connector> listConnectors = new List<Connector>();
            foreach (Connector connector in ductConnectorSet)
                listConnectors.Add(connector);
          

            if (listConnectors[0].Shape == ConnectorProfileType.Round)
            {
                res.Add(1);
                res.Add(duct.LookupParameter("Diameter").AsDouble());
                res.Add(duct.LookupParameter("Diameter").AsDouble()*Math.PI*duct.LookupParameter("Length").AsDouble());
            }
            else  
            {
                res.Add(2);
                res.Add(duct.LookupParameter("Width").AsDouble());
                res.Add(duct.LookupParameter("Height").AsDouble());
                res.Add(2*(duct.LookupParameter("Height").AsDouble()+ duct.LookupParameter("Width").AsDouble())* duct.LookupParameter("Length").AsDouble());
            }

            return res;
        }

        /// Methods for Flex Ducts

        static List<Element> AssignForFlexDucts(ICollection<Element> flexDucts, Document doc, Dictionary<Level, double> sortedLevels)
        {
            List<Element> notAssigned = new List<Element>();
            foreach (FlexDuct flexDuct in flexDucts)
            {
                Parameter parameterWSPLevel = flexDuct.LookupParameter("WSP_Level");
                Parameter parameterWSPLength = flexDuct.LookupParameter("WSP_Length");
                Parameter parameterWSPSystem = flexDuct.LookupParameter("WSP_System");
                Parameter parameterWSPWidth = flexDuct.LookupParameter("WSP_Width");
                Parameter parameterWSPHeight = flexDuct.LookupParameter("WSP_Height");
                Parameter parameterWSPDiameter = flexDuct.LookupParameter("WSP_Diameter");
                try
                {
                    parameterWSPLength.Set(flexDuct.LookupParameter("Length").AsDouble());
                    parameterWSPSystem.Set(flexDuct.LookupParameter("System Name").AsString());
                    List<double> ductShape = ShapeFlexChecker(flexDuct);
                    if (ductShape[0] == 1)
                    {
                        parameterWSPDiameter.Set(ductShape[1]);
                    }
                    else
                    {
                        parameterWSPWidth.Set(ductShape[1]);
                        parameterWSPHeight.Set(ductShape[2]);
                    }
                    double elementEle = GetFlexDuctLevel(flexDuct, doc);
                    Level correctLevel = SearchForLevel(sortedLevels, elementEle);
                    parameterWSPLevel.Set(correctLevel.Name);
                }
                catch
                {
                    notAssigned.Add(flexDuct);
                }
            }
            return notAssigned;
        }

        static double GetFlexDuctLevel(FlexDuct flexDuct, Document doc)
        {
            LocationCurve flexCurve = flexDuct.Location as LocationCurve;
            double elevation = flexCurve.Curve.GetEndPoint(0).Z;
            return elevation;
        }

        static List<double> ShapeFlexChecker(FlexDuct flexDuct)
        {
            List<double> res = new List<double>();
            if (flexDuct.ConnectorManager.Lookup(1).Shape == ConnectorProfileType.Round)
            {
                res.Add(1);
                res.Add(flexDuct.LookupParameter("Diameter").AsDouble());
            }
            else
            {
                res.Add(2);
                res.Add(flexDuct.LookupParameter("Width").AsDouble());
                res.Add(flexDuct.LookupParameter("Height").AsDouble());
            }
            return res;
        }

        /// Methods for Ducts Insulation

        static List<Element> AssignForDuctInsulation(ICollection<Element> ductInsulations, Document doc)
        {
            List<Element> notAssigned = new List<Element>();
            foreach (DuctInsulation ductInsulation in ductInsulations)
            {
                Element duct = doc.GetElement(ductInsulation.HostElementId) as Element;
                Parameter parameterWSPLevel = ductInsulation.LookupParameter("WSP_Level");
                Parameter parameterWSPLength = ductInsulation.LookupParameter("WSP_Length");
                Parameter parameterWSPSystem = ductInsulation.LookupParameter("WSP_System");
                Parameter parameterWSPThickness = ductInsulation.LookupParameter("WSP_Thickness");
                Parameter parameterWSPArea = ductInsulation.LookupParameter("WSP_Area");
                try
                {
                    parameterWSPLevel.Set(duct.LookupParameter("WSP_Level").AsString());
                    parameterWSPLength.Set(duct.LookupParameter("WSP_Length").AsDouble());
                    parameterWSPSystem.Set(duct.LookupParameter("System Name").AsString());
                    parameterWSPThickness.Set(ductInsulation.LookupParameter("Insulation Thickness").AsDouble());
                    if (duct.Category.Name != "Duct Accessories" && duct.Category.Name != "Flex Ducts")
                    { 
                        parameterWSPArea.Set(duct.LookupParameter("WSP_Area").AsDouble());
                    }
                    else
                    {
                        if (duct.LookupParameter("WSP_Diameter").AsDouble() == 0)
                        {
                            double area = 2 * (duct.LookupParameter("WSP_Width").AsDouble() + duct.LookupParameter("WSP_Height").AsDouble()) * duct.LookupParameter("WSP_Length").AsDouble();
                            parameterWSPArea.Set(area);
                        }
                    }
                    
                }
                catch
                {
                    notAssigned.Add(ductInsulation);
                }
            }
            return notAssigned;
        }


        /// Methods for Duct Fittings

        static List<Element> AssignForDuctFittings(ICollection<Element> accessories, Document doc, Dictionary<Level, double> sortedLevels)
        {
            List<Element> notAssigned = new List<Element>();

            foreach (FamilyInstance accessory in accessories)
            {
                Parameter parameterWSPLevel = accessory.LookupParameter("WSP_Level");
                Parameter parameterWSPLength = accessory.LookupParameter("WSP_Length");
                Parameter parameterWSPSystem = accessory.LookupParameter("WSP_System");
                Parameter parameterWSPWidth1 = accessory.LookupParameter("WSP_Width1");
                Parameter parameterWSPHeight1 = accessory.LookupParameter("WSP_Height1");
                Parameter parameterWSPDiameter1 = accessory.LookupParameter("WSP_Diameter1");
                Parameter parameterWSPWidth2 = accessory.LookupParameter("WSP_Width2");
                Parameter parameterWSPHeight2 = accessory.LookupParameter("WSP_Height2");
                Parameter parameterWSPDiameter2 = accessory.LookupParameter("WSP_Diameter2");
                Parameter parameterWSPWidth3 = accessory.LookupParameter("WSP_Width3");
                Parameter parameterWSPHeight3 = accessory.LookupParameter("WSP_Height3");
                Parameter parameterWSPDiameter3 = accessory.LookupParameter("WSP_Diameter3");
                try
                {
                    parameterWSPSystem.Set(accessory.LookupParameter("System Name").AsString());
                    if (accessory.MEPModel.ConnectorManager.Connectors.Size < 2)
                    {
                        double accessoryLength = 0;
                        parameterWSPLength.Set(accessoryLength);
                    }
                    else
                    {
                        double accessoryLength = GetFittingLength(accessory);
                        parameterWSPLength.Set(accessoryLength);
                    }
                    List<double> ductShape = ShapeFittingChecker(accessory);
                    parameterWSPDiameter1.Set(ductShape[0]);
                    parameterWSPDiameter2.Set(ductShape[3]);
                    parameterWSPDiameter3.Set(ductShape[6]);
                    parameterWSPWidth1.Set(ductShape[1]);
                    parameterWSPWidth2.Set(ductShape[4]);
                    parameterWSPWidth3.Set(ductShape[7]);
                    parameterWSPHeight1.Set(ductShape[2]);
                    parameterWSPHeight2.Set(ductShape[5]);
                    parameterWSPHeight3.Set(ductShape[8]);
                    double elementEle = GetFittingLevel(accessory, doc);
                    Level correctLevel = SearchForLevel(sortedLevels, elementEle);
                    parameterWSPLevel.Set(correctLevel.Name);

                }
                catch
                {
                    notAssigned.Add(accessory);
                }
            }
            return notAssigned;
        }

        static double GetFittingLevel(FamilyInstance accessory, Document doc)
        {
            double elevation = 0;
            ElementId refLevelId = accessory.LookupParameter("Level").AsElementId();
            Level refLevel = doc.GetElement(refLevelId) as Level;
            double levelEl = refLevel.Elevation;
            double ductOffset = accessory.LookupParameter("Elevation from Level").AsDouble();
            elevation = elevation + levelEl + ductOffset;
            return elevation;
        }

        static List<double> ShapeFittingChecker(FamilyInstance accessory)
        {
            List<double> res = new List<double>();
            ConnectorSet ductConnectorSet = accessory.MEPModel.ConnectorManager.Connectors;
            foreach (Connector connector in ductConnectorSet)
            {
                if (connector.Shape == ConnectorProfileType.Round)
                {
                    res.Add(connector.Radius);
                    res.Add(0);
                    res.Add(0);
                }
                else
                {
                    res.Add(0);
                    res.Add(connector.Height);
                    res.Add(connector.Width);
                }
            }
            for (int i=0; i<9- 3*ductConnectorSet.Size; i++)
            {
                res.Add(0);
            }
            return res;
        }

        static double GetFittingLength(FamilyInstance accessory)
        {
            ConnectorSet ductConnectorSet = accessory.MEPModel.ConnectorManager.Connectors;
            if (ductConnectorSet.Size == 0)
            {
                return 0;
            }
            else
            {
                List<Connector> listConnectors = new List<Connector>();
                foreach (Connector connector in ductConnectorSet)
                    listConnectors.Add(connector);
                Connector connector1 = listConnectors[0];
                Connector connector2 = listConnectors[1];
                double distance = connector1.Origin.DistanceTo(connector2.Origin);
                return distance;
            }
        }

        /// Methods for Duct Accessories

        static List<Element> AssignForDuctAccesories(ICollection<Element> accessories, Document doc, Dictionary<Level, double> sortedLevels)
        {
            List<Element> notAssigned = new List<Element>();

            foreach (FamilyInstance accessory in accessories)
            {
                Parameter parameterWSPLevel = accessory.LookupParameter("WSP_Level");
                Parameter parameterWSPLength = accessory.LookupParameter("WSP_Length");
                Parameter parameterWSPSystem = accessory.LookupParameter("WSP_System");
                Parameter parameterWSPWidth = accessory.LookupParameter("WSP_Width");
                Parameter parameterWSPHeight = accessory.LookupParameter("WSP_Height");
                Parameter parameterWSPDiameter = accessory.LookupParameter("WSP_Diameter");
                Parameter parameterWSPSpace = accessory.LookupParameter("WSP_Space");
                try
                {
                    parameterWSPSystem.Set(accessory.LookupParameter("System Name").AsString());
                    double accessoryLength = GetAccessoryLength(accessory);
                    parameterWSPLength.Set(accessoryLength);
                    List<double> ductShape = ShapeAccessoryChecker(accessory);
                    if (ductShape[0] == 1)
                    {
                        parameterWSPDiameter.Set(ductShape[1]);
                    }
                    else
                    {
                        parameterWSPWidth.Set(ductShape[1]);
                        parameterWSPHeight.Set(ductShape[2]);
                    }
                    double elementEle = GetAccessoryLevel(accessory, doc);
                    Level correctLevel = SearchForLevel(sortedLevels, elementEle);
                    parameterWSPLevel.Set(correctLevel.Name);
                    string elementSpace = GetSpaceForElement(accessory);
                    parameterWSPSpace.Set(elementSpace);

                }
                catch
                {
                    notAssigned.Add(accessory);
                }
            }
            return notAssigned;
        }

        static double GetAccessoryLevel(FamilyInstance accessory, Document doc)
        {
            double elevation = 0;
            ElementId refLevelId = accessory.LookupParameter("Level").AsElementId();
            Level refLevel = doc.GetElement(refLevelId) as Level;
            double levelEl = refLevel.Elevation;
            double ductOffset = accessory.LookupParameter("Elevation from Level").AsDouble();
            elevation = elevation + levelEl + ductOffset;
            return elevation;
        }

        static List<double> ShapeAccessoryChecker(FamilyInstance accessory)
        {
            List<double> res = new List<double>();
            ConnectorSet ductConnectorSet = accessory.MEPModel.ConnectorManager.Connectors;
            List<Connector> listConnectors = new List<Connector>();
            foreach (Connector connector in ductConnectorSet)
                listConnectors.Add(connector);

            if (listConnectors[0].Shape == ConnectorProfileType.Round)
            {
                res.Add(1);
                res.Add(listConnectors[0].Radius);
            }
            else
            {
                res.Add(2);
                res.Add(listConnectors[0].Width);
                res.Add(listConnectors[0].Height);
            }

            return res;
        }

        static double GetAccessoryLength(FamilyInstance accessory)
        {
            ConnectorSet ductConnectorSet = accessory.MEPModel.ConnectorManager.Connectors;
            if (ductConnectorSet.Size == 0 )
            {
                return 0;
            }
            else
            {
                List<Connector> listConnectors = new List<Connector>();
                foreach (Connector connector in ductConnectorSet)
                listConnectors.Add(connector);
                Connector connector1 = listConnectors[0];
                Connector connector2 = listConnectors[1];
                double distance = connector1.Origin.DistanceTo(connector2.Origin);
                return distance;
            }
        }

        /// Methods for Air Terminals
        static List<Element> AssignForDuctTerminal(ICollection<Element> airTerminals, Document doc, Dictionary<Level, double> sortedLevels)
        {
            List<Element> notAssigned = new List<Element>();
            foreach (FamilyInstance airTerminal in airTerminals)
            {
                Parameter parameterWSPLevel = airTerminal.LookupParameter("WSP_Level");
                Parameter parameterWSPSystem = airTerminal.LookupParameter("WSP_System");
                Parameter parameterWSPWidth = airTerminal.LookupParameter("WSP_Width");
                Parameter parameterWSPHeight = airTerminal.LookupParameter("WSP_Height");
                Parameter parameterWSPDiameter = airTerminal.LookupParameter("WSP_Diameter");
                Parameter parameterWSPSpace = airTerminal.LookupParameter("WSP_Space");
                try
                {
                    parameterWSPSystem.Set(airTerminal.LookupParameter("System Name").AsString());
                    List<double> airTerminalShape = ShapeAirTerminalChecker(airTerminal);
                    if (airTerminalShape[0] == 1)
                    {
                        parameterWSPDiameter.Set(airTerminalShape[1]);
                    }
                    else
                    {
                        parameterWSPWidth.Set(airTerminalShape[1]);
                        parameterWSPHeight.Set(airTerminalShape[2]);
                    }
                    double elementEle = GetAirTerminalLevel(airTerminal, doc);
                    Level correctLevel = SearchForLevel(sortedLevels, elementEle);
                    parameterWSPLevel.Set(correctLevel.Name);
                    parameterWSPSpace.Set(GetSpaceForElement(airTerminal));
                }
                catch 
                {
                    notAssigned.Add(airTerminal);
                }
            }
            return notAssigned;
        }

        static double GetAirTerminalLevel(FamilyInstance accessory, Document doc)
        {
            double elevation = 0;
            ElementId refLevelId = accessory.LookupParameter("Level").AsElementId();
            Level refLevel = doc.GetElement(refLevelId) as Level;
            double levelEl = refLevel.Elevation;
            double ductOffset = accessory.LookupParameter("Elevation from Level").AsDouble();
            elevation = elevation + levelEl + ductOffset;
            return elevation;
        }

        static List<double> ShapeAirTerminalChecker(FamilyInstance airTerminal)
        {
            List<double> res = new List<double>();
            ConnectorSet terminalConnectorSet = airTerminal.MEPModel.ConnectorManager.Connectors;
            List<Connector> listConnectors = new List<Connector>();
            foreach (Connector connector in terminalConnectorSet)
                listConnectors.Add(connector);


            if (listConnectors[0].Shape == ConnectorProfileType.Round)
            {
                res.Add(1);
                res.Add(listConnectors[0].Radius*2);
            }
            else
            {
                res.Add(2);
                res.Add(listConnectors[0].Width);
                res.Add(listConnectors[0].Height);
            }

            return res;
        }

        /// Methods for Mechanical Equipment

        static List<Element> AssignForMechanicalE(ICollection<Element> mechanicalEquipment, Document doc, Dictionary<Level, double> sortedLevels)
        {
            List<Element> notAssigned = new List<Element>();
            foreach (FamilyInstance mechanicalE in mechanicalEquipment)
            {
                Parameter parameterWSPLevel = mechanicalE.LookupParameter("WSP_Level");
                Parameter parameterWSPSystem = mechanicalE.LookupParameter("WSP_System");
                Parameter parameterWSPSpace = mechanicalE.LookupParameter("WSP_Space");
                try
                {
                    parameterWSPSystem.Set(mechanicalE.LookupParameter("System Name").AsString());
                    double elementEle = GetMechanicalELevel(mechanicalE, doc);
                    Level correctLevel = SearchForLevel(sortedLevels, elementEle);
                    parameterWSPLevel.Set(correctLevel.Name);
                    parameterWSPSpace.Set(GetSpaceForElement(mechanicalE));
                }
                catch
                {
                    notAssigned.Add(mechanicalE);
                }
            }
            return notAssigned;
        }

        static double GetMechanicalELevel(FamilyInstance mechanicalE, Document doc)
        {
            double elevation = 0;
            ElementId refLevelId = mechanicalE.LookupParameter("Level").AsElementId();
            Level refLevel = doc.GetElement(refLevelId) as Level;
            double levelEl = refLevel.Elevation;
            double ductOffset = mechanicalE.LookupParameter("Elevation from Level").AsDouble();
            elevation = elevation + levelEl + ductOffset;
            return elevation;
        }
        
        
        
        /// Methods for all elements


        static string GetSpaceForElement(Element element)
        {
            FamilyInstance familyElement = element as FamilyInstance;
            try
            {
                string spaceNumber = familyElement.Space.Number;
                return spaceNumber;
            }
            catch
            {
                return "-";
            }
        }

        public static Level SearchForLevel(Dictionary<Level, double> levelDictionary, double elevation)
        {
            int levelInd = 0;
            foreach (KeyValuePair<Level, double> entry in levelDictionary)
            {
                if (elevation > entry.Value)
                    levelInd = levelInd + 1;
                else
                    break;
            }
            if (levelInd > 0)
                levelInd = levelInd - 1;
            return levelDictionary.Keys.ToList()[levelInd];
        }


        public StringBuilder SB(List<Element> element)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Element w in element)
                sb.Append(w.Name + " " + w.Id + "\n");
            return sb;
        }

        public StringBuilder SBLev(Dictionary<Level, double> elems)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<Level, double> w in elems)
                sb.Append(w.Key.Name + " " + w.Value + "\n");
            return sb;
        }
        public StringBuilder SBStr(List<string> errorsStr)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string error in errorsStr)
                sb.Append(error);
            return sb;
        }
    }
}
