using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace WSPPolska_Tools
{
    //[Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class CreateWorksetHandler : IExternalEventHandler
    {
        public string worksetName { get; set; }

        public Workset CreatedWorkset { get; private set; }


        public void Execute(UIApplication app)
        {
            Document doc = app.ActiveUIDocument.Document;

            try
            {
                using (Transaction tx = new Transaction(doc, "Create Workset"))
                {
                    tx.Start();
                    CreatedWorkset = Workset.Create(doc, worksetName);
                    tx.Commit();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
            }
        }
        public string GetName()
        {
            return "Create Workset Handler";
        }
    }
}
