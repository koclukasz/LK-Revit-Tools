using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using static WSPPolska_Tools.GeolocationForm;


namespace WSPPolska_Tools
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class DeleteElementsHandler : IExternalEventHandler
    {
        List<ElementId> elementIds = ElementToBeDeleted.ElementIdsToDelete;

        public void Execute(UIApplication app)
        {
            Document doc = app.ActiveUIDocument.Document;
            using (Transaction tx = new Transaction(doc, "Modify Elements"))
            {
                tx.Start();
                doc.Delete(elementIds);
                tx.Commit();
            }    

        }
        public string GetName() => "Delete Elements Handler";
    }
}
