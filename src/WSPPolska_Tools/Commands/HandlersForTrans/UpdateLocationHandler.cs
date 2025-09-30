using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;


namespace WSPPolska_Tools
{
    //[Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class UpdateLocationHandler : IExternalEventHandler
    {
        public Dictionary<string,NewLocationData> newLocationDic { get; set; }

        public void Execute(UIApplication app)
        {
            Document doc = app.ActiveUIDocument.Document;
            BasePoint basePoint = BasePoint.GetProjectBasePoint(doc);
            XYZ basePointPos = basePoint.SharedPosition;
            ProjectLocation activeLoc = doc.ActiveProjectLocation;

            try
            {
                using (Transaction tx = new Transaction(doc, "Change Location"))
                {
                    tx.Start();
                    foreach (KeyValuePair<string, NewLocationData> newLoc in newLocationDic)
                    { 
                        double NS = newLoc.Value.NS;
                        double EW = newLoc.Value.EW;
                        double EL = newLoc.Value.EL;
                        double angleToNorth = newLoc.Value.Rot;
                        var newPosition = new ProjectPosition(NS, EW, EL, -angleToNorth * (Math.PI / 180));
                        if (newLoc.Value.ElementId.IntegerValue == -1)
                        {
                            ProjectLocation newLocation = activeLoc.Duplicate(newLoc.Key);
                            newLocation.SetProjectPosition(basePointPos, newPosition);
                        }
                        else
                        { 
                            ProjectLocation exLocation = doc.GetElement(newLoc.Value.ElementId) as ProjectLocation;
                            exLocation.SetProjectPosition(basePointPos, newPosition);
                        } 
                    }
                    tx.Commit();
                }
            }
            catch (Exception ex)
            {

            }

        }
        public string GetName()
        { 
            return "Delete Elements Handler";
        } 
    }
}
