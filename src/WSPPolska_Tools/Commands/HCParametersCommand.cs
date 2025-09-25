namespace WSPPolska_Tools
{
    using System.Collections.Generic;
    using System.Linq;
    using Autodesk.Revit.UI;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Plumbing;
    using System.Diagnostics;
    using System;

    /// <summary>
    /// Command to Be executed when button clicked
    /// </summary>
    /// <seealso cref="Autodesk.Revit.UI.IExternalCommand"></seealso>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]


    public class HCParametersCommand : IExternalCommand
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

            ///Collecting PIPES and creating list of not assigned
            ICollection<Element> viewPipes = new FilteredElementCollector(doc, doc.ActiveView.Id).OfCategory(BuiltInCategory.OST_PipeCurves).WhereElementIsNotElementType().ToElements();
            List<Element> notAssignedPipes = new List<Element>();

            ///Collecting PIPE FITTINGS and creating list of not assigned
            ICollection<Element> viewPipeFittings = new FilteredElementCollector(doc, doc.ActiveView.Id).OfCategory(BuiltInCategory.OST_PipeFitting).WhereElementIsNotElementType().ToElements();
            List<Element> notAssignedPipeFittings = new List<Element>();

            ///Collecting PIPE FITTINGS and creating list of not assigned
            ICollection<Element> viewPlumbingFixtures = new FilteredElementCollector(doc, doc.ActiveView.Id).OfCategory(BuiltInCategory.OST_PlumbingFixtures).WhereElementIsNotElementType().ToElements();
            List<Element> notAssignedPlumbingFixtures = new List<Element>();

            ///Collecting PIPE ACCESSORIES and creating list of not assigned
            ICollection<Element> viewPipeAccessories = new FilteredElementCollector(doc, doc.ActiveView.Id).OfCategory(BuiltInCategory.OST_PipeAccessory).WhereElementIsNotElementType().ToElements();
            List<Element> notAssignedPipeAccessories = new List<Element>();

            ///Collecting MECHANICAL EQUIPMENT and creating list of not assigned
            ICollection<Element> viewMechanicalEquipment = new FilteredElementCollector(doc, doc.ActiveView.Id).OfCategory(BuiltInCategory.OST_MechanicalEquipment).WhereElementIsNotElementType().ToElements();
            List<Element> notAssignedMechanicalEquipment = new List<Element>();

            ///Collecting Pipe INSULATION and creating list of not assigned
            ICollection<Element> viewPipeInsulation = new FilteredElementCollector(doc, doc.ActiveView.Id).OfCategory(BuiltInCategory.OST_PipeInsulations).WhereElementIsNotElementType().ToElements();
            List<Element> notAssignedPipeInsulation = new List<Element>();

            using (Transaction t = new Transaction(doc, "CommentParam"))
            {
                t.Start("Comm");
                notAssignedPipes = AssignForPipes(viewPipes, doc, sortedLevels);
                notAssignedPipeFittings = AssignForPipeFittings(viewPipeFittings, doc, sortedLevels);
                notAssignedPipeAccessories = AssignForPipeFittings(viewPipeAccessories, doc, sortedLevels);
                notAssignedPlumbingFixtures = AssignForPlumbingFixtures(viewPlumbingFixtures, doc, sortedLevels);
                notAssignedMechanicalEquipment = AssignForPlumbingFixtures(viewMechanicalEquipment, doc, sortedLevels);
                notAssignedPipeInsulation = AssignForPipeInsulation(viewPipeInsulation, doc, sortedLevels);
                t.Commit();
            }

            stopwatch.Stop();
            TaskDialog.Show("Values", "Time " + stopwatch.ElapsedMilliseconds/1000 + "s \n" +
            "Not assigned to: \nPipes\n" + CommonM.SB(notAssignedPipes)+
            "Pipe Fittings\n" + CommonM.SB(notAssignedPipeFittings) +
            "Pipe Accessories\n" + CommonM.SB(notAssignedPipeAccessories) +
            "Plumbing Fixtures\n" + CommonM.SB(notAssignedPlumbingFixtures) +
            "Mechanical Equipment\n" + CommonM.SB(notAssignedMechanicalEquipment) +
            "Pipe Insulation\n" + CommonM.SB(notAssignedPipeInsulation));

            return Result.Succeeded;
        }
        
        /// Methods for Pipes
        static List<Element> AssignForPipes(ICollection<Element> pipes, Document doc, Dictionary<Level, double> sortedLevels)
        {
            List<Element> notAssigned = new List<Element>();
            foreach (Pipe pipe in pipes)
            {
                Parameter parameterWSPLevel = pipe.LookupParameter("WSP_Level");
                Parameter parameterWSPLength = pipe.LookupParameter("WSP_Length");
                Parameter parameterWSPSystem = pipe.LookupParameter("WSP_System");
                Parameter parameterWSPDiameter = pipe.LookupParameter("WSP_Diameter");
                Parameter parameterWSPNominalDiameter = pipe.LookupParameter("WSP_NominalDiameter");
                try
                {
                    parameterWSPLength.Set(pipe.LookupParameter("Length").AsDouble());
                    parameterWSPSystem.Set(pipe.LookupParameter("System Name").AsString());
                    double elementEle = GetPipeLevel(pipe, doc);
                    Level correctLevel = CommonM.SearchForLevel(sortedLevels, elementEle);
                    parameterWSPLevel.Set(correctLevel.Name);
                    parameterWSPDiameter.Set(pipe.LookupParameter("Outside Diameter").AsDouble());
                    parameterWSPNominalDiameter.Set(pipe.LookupParameter("Diameter").AsDouble());
                }
                catch
                {
                    notAssigned.Add(pipe);
                }
            }
            return notAssigned;
        }

        static double GetPipeLevel(Pipe pipe, Document doc)
        {
            double elevation = 0;
            ElementId refLevelId = pipe.LookupParameter("Reference Level").AsElementId();
            Level refLevel = doc.GetElement(refLevelId) as Level;
            double levelEl = refLevel.Elevation;
            double ductOffset = pipe.LookupParameter("Middle Elevation").AsDouble();
            elevation = elevation + levelEl + ductOffset;
            return elevation;
        }

        /// Methods for Pipe Fittings, Pipe Accessories
        static List<Element> AssignForPipeFittings(ICollection<Element> fittings, Document doc, Dictionary<Level, double> sortedLevels)
        {
            List<Element> notAssigned = new List<Element>();
            foreach (FamilyInstance fitting in fittings)
            {
                Parameter parameterWSPLevel = fitting.LookupParameter("WSP_Level");
                Parameter parameterWSPLength = fitting.LookupParameter("WSP_Length");
                Parameter parameterWSPSystem = fitting.LookupParameter("WSP_System");
                Parameter parameterWSPDiameter = fitting.LookupParameter("WSP_Diameter");
                Parameter parameterWSPDiameter1 = fitting.LookupParameter("WSP_Diameter1");
                Parameter parameterWSPDiameter2 = fitting.LookupParameter("WSP_Diameter2");
                Parameter parameterWSPDiameter3 = fitting.LookupParameter("WSP_Diameter3");
                Parameter parameterWSPNominalDiameter = fitting.LookupParameter("WSP_NominalDiameter");
                Parameter parameterWSPArea = fitting.LookupParameter("WSP_Area");
                try
                {
                    ///parameterWSPLength.Set(fitting.LookupParameter("Length").AsDouble());
                    parameterWSPSystem.Set(fitting.LookupParameter("System Name").AsString());

                    double elementEle = GetInstanceLevel(fitting, doc);
                    Level correctLevel = CommonM.SearchForLevel(sortedLevels, elementEle);
                    parameterWSPLevel.Set(correctLevel.Name);

                    List<double> ductShape = ShapeFittingChecker(fitting);
                    parameterWSPDiameter1.Set(ductShape[0]);
                    parameterWSPDiameter2.Set(ductShape[1]);
                    parameterWSPDiameter3.Set(ductShape[2]);
                    parameterWSPDiameter.Set(ductShape[3]);
                    parameterWSPNominalDiameter.Set(ductShape[4]);
                }
                catch
                {
                    notAssigned.Add(fitting);
                }
            }
            return notAssigned;
        }

        /// Methods for Plumbing Fixtures, Mechanical Equipment
        static List<Element> AssignForPlumbingFixtures(ICollection<Element> fittings, Document doc, Dictionary<Level, double> sortedLevels)
        {
            List<Element> notAssigned = new List<Element>();
            foreach (FamilyInstance fitting in fittings)
            {
                Parameter parameterWSPLevel = fitting.LookupParameter("WSP_Level");
                Parameter parameterWSPSystem = fitting.LookupParameter("WSP_System");
                try
                {
                    parameterWSPSystem.Set(fitting.LookupParameter("System Name").AsString());

                    double elementEle = GetInstanceLevel(fitting, doc);
                    Level correctLevel = CommonM.SearchForLevel(sortedLevels, elementEle);
                    parameterWSPLevel.Set(correctLevel.Name);


                }
                catch
                {
                    notAssigned.Add(fitting);
                }
            }
            return notAssigned;
        }

        /// Methods for Pipe Insulation
        static List<Element> AssignForPipeInsulation(ICollection<Element> pipeInsulations, Document doc, Dictionary<Level, double> sortedLevels)
        {
            List<Element> notAssigned = new List<Element>();
            foreach (PipeInsulation pipeInsulation in pipeInsulations)
            {
                Element pipe = doc.GetElement(pipeInsulation.HostElementId) as Element;
                Parameter parameterWSPLevel = pipeInsulation.LookupParameter("WSP_Level");
                Parameter parameterWSPLength = pipeInsulation.LookupParameter("WSP_Length");
                Parameter parameterWSPSystem = pipeInsulation.LookupParameter("WSP_System");
                Parameter parameterWSPDiameter = pipeInsulation.LookupParameter("WSP_Diameter");
                Parameter parameterWSPArea = pipeInsulation.LookupParameter("WSP_Area");
                Parameter parameterWSPThickness = pipeInsulation.LookupParameter("WSP_Thickness");
                try
                {
                    parameterWSPSystem.Set(pipe.LookupParameter("System Name").AsString());
                    parameterWSPLevel.Set(pipe.LookupParameter("WSP_Level").AsString());
                    parameterWSPLength.Set(pipe.LookupParameter("WSP_Length").AsDouble());
                    parameterWSPDiameter.Set(pipe.LookupParameter("WSP_Diameter").AsDouble());
                    if (pipe.Category.Name == "Pipe")
                    parameterWSPArea.Set(pipe.LookupParameter("WSP_Diameter").AsDouble() * Math.PI * pipe.LookupParameter("WSP_Length").AsDouble());
                    parameterWSPThickness.Set(pipeInsulation.LookupParameter("Insulation Thickness").AsDouble());


                }
                catch
                {
                    notAssigned.Add(pipeInsulation);
                }
            }
            return notAssigned;
        }


        static double GetInstanceLevel(FamilyInstance fitting, Document doc)
        {
            double elevation = 0;
            ElementId refLevelId = fitting.LookupParameter("Level").AsElementId();
            Level refLevel = doc.GetElement(refLevelId) as Level;
            double levelEl = refLevel.Elevation;
            double ductOffset = fitting.LookupParameter("Elevation from Level").AsDouble();
            elevation = elevation + levelEl + ductOffset;
            return elevation;
        }

        static List<double> ShapeFittingChecker(FamilyInstance element)
        {
            List<double> connectorsDiameters = new List<double>();
            ConnectorSet fittingConnectorSet = element.MEPModel.ConnectorManager.Connectors;
            double diameter = 0;
            double outsideDiameter = 0;
            foreach (Connector connector in fittingConnectorSet)
            {
                connectorsDiameters.Add(connector.Radius * 2);
                if (connector.Radius > diameter)
                {
                    diameter = connector.Radius;
                    List<Connector> elemenSet = new List<Connector>();
                    foreach (Connector connector1 in connector.AllRefs)
                        elemenSet.Add(connector1);
                    try
                    {
                        outsideDiameter = elemenSet[0].Owner.LookupParameter("Outside Diameter").AsDouble();
                    }
                    catch
                    {
                        outsideDiameter = connector.Radius*2;
                    }
                }
            }
            if (fittingConnectorSet.Size == 2)
                connectorsDiameters.Add(0);
            else if (fittingConnectorSet.Size == 1)
            {
                connectorsDiameters.Add(0);
                connectorsDiameters.Add(0);
            }
            connectorsDiameters.Add(outsideDiameter);
            connectorsDiameters.Add(diameter * 2);

            return connectorsDiameters;
        }
    }
}
