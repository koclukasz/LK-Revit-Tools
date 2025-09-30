namespace WSPPolska_Tools
{
    using Autodesk.Revit.ApplicationServices;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Mechanical;
    using Autodesk.Revit.DB.Plumbing;
    using Autodesk.Revit.Exceptions;
    using Autodesk.Revit.UI;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// Command to Be executed when button clicked
    /// </summary>
    /// <seealso cref="Autodesk.Revit.UI.IExternalCommand"></seealso>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]


    public class SplitElements: IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            Options opt1 = new Options();
            List<Level> allLevels = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Levels).WhereElementIsNotElementType().ToElements() as List<Level>;

            List<List<Element>> allElemsAtLevels = new List<List<Element>>();

            foreach (Level level in allLevels)
            {
                List<Element> allElemsAtLevel;
                allElemsAtLevel = GetElementsAtLevel(level.Name, doc);
                allElemsAtLevels.Add(allElemsAtLevel);

            }

            for (int index = 0; index < allElemsAtLevels.Count; index++)
            {
                Debug.WriteLine($"{index}: {allElemsAtLevels[index]}");
            }

            ///Reference elRef1 = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element,"Select first object");
            ///Element el1 = doc.GetElement(elRef1);
            Reference elRef1 = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element, "Select first object");
            Element el1 = doc.GetElement(elRef1);
            Reference elRef2 = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element, "Select second object");
            Element el2 = doc.GetElement(elRef2);

            ///double intersectionVolume;
            ///intersectionVolume = GetIntersectingVolume(el1, el2, opt1);

            ///TaskDialog.Show("Proba", intersectionVolume.ToString());

            using (Transaction t = new Transaction(doc, "Join elements"))
            {
                t.Start("Comm");
                JoinElements(el1, el2, opt1, doc);
                t.Commit();
            }


            return Result.Succeeded;


        }
        public void JoinElements(Element el1, Element el2, Options opt, Document doc)
        {
            double vol = GetIntersectingVolume(el1, el2, opt);
            if  (vol > 0)
            JoinGeometryUtils.JoinGeometry(doc, el1, el2);

        }

        public double GetIntersectingVolume(Element element1, Element element2, Options opt)
        {
            double intersectionVolume;
            Solid sol1 = GetSolid(element1, opt);
            Solid sol2 = GetSolid(element2, opt);
            try
            {
                Solid utils1;
                utils1 = BooleanOperationsUtils.ExecuteBooleanOperation(sol1, sol2, BooleanOperationsType.Intersect);
                intersectionVolume = utils1.Volume;
            }
            catch
            {
                intersectionVolume = 0;
            }
            return intersectionVolume;
        }
        public Solid GetSolid(Element element, Options opt1)
        {
            GeometryElement el2Geom = element.get_Geometry(opt1);
            Solid sol;
            foreach (GeometryObject geom in el2Geom)
            {
                Solid solEl = geom as Solid;
                if (null != solEl && solEl.Volume > 0)
                {
                    sol = solEl;
                    return sol;
                }
            }
            return null;
        }

            public List<Element> GetElementsAtLevel(string level, Document doc)
        {
            List<Element> viewWallForParam = new FilteredElementCollector(doc, doc.ActiveView.Id).OfCategory(BuiltInCategory.OST_Walls).WhereElementIsNotElementType().ToElements() as List<Element>;
            Element el = viewWallForParam[0];
            ElementId paramId = el.LookupParameter("WSP_Level").Id;
            List<Element> elemListAtLevel = new List<Element>();

            ParameterValueProvider provider = new ParameterValueProvider(paramId);
            FilterStringEquals evaluator = new FilterStringEquals();
            FilterStringRule levelRule = new FilterStringRule(provider, evaluator, level);
            ElementParameterFilter levelFilter = new ElementParameterFilter(levelRule);

            ICollection<Element> viewWalls = new FilteredElementCollector(doc, doc.ActiveView.Id).OfCategory(BuiltInCategory.OST_Walls).WherePasses(levelFilter).ToElements();
            foreach (Element elem in viewWalls)
                elemListAtLevel.Add(elem);
            ICollection<Element> viewColumns = new FilteredElementCollector(doc, doc.ActiveView.Id).OfCategory(BuiltInCategory.OST_StructuralColumns).WherePasses(levelFilter).ToElements();
            foreach (Element elem in viewColumns)
                elemListAtLevel.Add(elem);
            ICollection<Element> viewFloors = new FilteredElementCollector(doc, doc.ActiveView.Id).OfCategory(BuiltInCategory.OST_Floors).WherePasses(levelFilter).ToElements();
            foreach (Element elem in viewWalls)
                elemListAtLevel.Add(elem);
            ICollection<Element> viewFraming = new FilteredElementCollector(doc, doc.ActiveView.Id).OfCategory(BuiltInCategory.OST_StructuralFraming).WherePasses(levelFilter).ToElements();
            foreach (Element elem in viewFraming)
                elemListAtLevel.Add(elem);
            return elemListAtLevel;
        }
    }
}
