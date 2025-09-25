using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using static WSPPolska_Tools.GeolocationForm;


namespace WSPPolska_Tools
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class ImportLocationsHandler : IExternalEventHandler
    {
        public Dictionary<string, List<double>> AllNewLocations { get; set; }
        public ExternalCommandData CommandData { get; set; }

        public void Execute(UIApplication app)
        {
            Document doc = app.ActiveUIDocument.Document;
            BasePoint basePoint = BasePoint.GetProjectBasePoint(doc);
            XYZ basePointPos = basePoint.SharedPosition;

            using (Transaction tx = new Transaction(doc, "Delete Elements"))
            {
                tx.Start();

                foreach (ElementId id in ElementToBeDeleted.ElementIdsToDelete)
                {
                    Element el = doc.GetElement(id);
                    if (el is ProjectLocation location)
                    {
                        Transform prTransf = location.GetTotalTransform();
                        double NS = prTransf.Origin.X * 0.3048;
                        double EW = prTransf.Origin.Y * 0.3048;
                        double Z = prTransf.Origin.Z * 0.3048;
                        XYZ modelVec = prTransf.BasisX;
                        XYZ northVec = new XYZ(1, 0, 0);
                        XYZ planeVec = new XYZ(0, 0, 1);
                        double angleToNorth = 2 * Math.PI - northVec.AngleOnPlaneTo(modelVec, planeVec);
                        double angleDegrees = angleToNorth * (180 / Math.PI);

                        ElementToBeDeleted.DeletedElementDetails.Add(new string[]
                        {
                        "DELETED",
                        location.Name,
                        Math.Round(EW, 3).ToString(),
                        Math.Round(NS, 3).ToString(),
                        Math.Round(Z, 3).ToString(),
                        Math.Round(angleDegrees, 3).ToString()
                        });
                    }

                    doc.Delete(id);
                }

                tx.Commit();
            }
        }

        public string GetName() => "Delete Elements Handler";
    }

}
