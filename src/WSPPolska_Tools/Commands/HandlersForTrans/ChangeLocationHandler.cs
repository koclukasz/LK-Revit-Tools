using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;


namespace WSPPolska_Tools
{
    //[Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class ChangeLocationHandler : IExternalEventHandler
    {
        public ElementId newLocation { get; set; }

        public void Execute(UIApplication app)
        {
            Document doc = app.ActiveUIDocument.Document;

            try
            {
                using (Transaction tx = new Transaction(doc, "Change Location"))
                {
                    tx.Start();
                    doc.ActiveProjectLocation = doc.GetElement(newLocation) as ProjectLocation;
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
