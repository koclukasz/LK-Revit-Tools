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
            XYZ basePointPos = basePoint.Position;
            ProjectLocation activeLoc = doc.ActiveProjectLocation;

            try
            {
                using (Transaction tx = new Transaction(doc, "Change Location"))
                {
                    tx.Start();
                    foreach (KeyValuePair<string, NewLocationData> newLoc in newLocationDic)
                    { 
                        double NS = newLoc.Value.NS / 0.3048;
                        double EW = newLoc.Value.EW / 0.3048;
                        double EL = newLoc.Value.EL / 0.3048;
                        double angleToNorth = newLoc.Value.Rot;
                        var newPosition = new ProjectPosition(EW, NS, EL, -angleToNorth * (Math.PI / 180));
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
