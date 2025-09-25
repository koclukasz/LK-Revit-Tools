using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;


namespace WSPPolska_Tools
{
    //[Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class DeleteElementsHandler : IExternalEventHandler
    {
        public List<ElementId> elementIds { get; set; }

        public void Execute(UIApplication app)
        {
            Document doc = app.ActiveUIDocument.Document;

            //try
            //{
            using (Transaction tx = new Transaction(doc, "Modify Elements"))
            {
                tx.Start();
                doc.Delete(elementIds);
                tx.Commit();
            }
            //}
            //catch (Exception ex)
            //{ 

            //}

        }
        public string GetName()
        { 
            return "Delete Elements Handler";
        } 
    }
}
