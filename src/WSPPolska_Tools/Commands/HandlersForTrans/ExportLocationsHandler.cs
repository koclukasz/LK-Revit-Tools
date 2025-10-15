using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BIM.IFC.Export.UI;
using System;
using System.Collections.Generic;


namespace WSPPolska_Tools
{
    //[Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class ExportLocationsHandler : IExternalEventHandler
    {
        public IFCExportOptions IFCExportOpt { get; set; }
        public NavisworksExportOptions NWCExportOptions { get; set; }
        public DWFXExportOptions DWFXExportOpt { get; set; }
        public string selectedPath { get; set; }
        public string fileName { get; set; }
        public int nameSt { get; set; }

        public ViewSet viewSet { get; set; }

        public bool exIFC {  get; set; }
        public bool exNWC { get; set; }
        public bool exDWFx { get; set; }

        public void Execute(UIApplication app)
        {
            Document doc = app.ActiveUIDocument.Document;
            BasePoint basePoint = BasePoint.GetProjectBasePoint(doc);
            XYZ basePointPos = basePoint.Position;
            ProjectLocation activeLoc = doc.ActiveProjectLocation;

            ProjectLocationSet allRevLocations = doc.ProjectLocations;

            foreach (ProjectLocation revLocation in allRevLocations)
            {
                string fileNameForExport = new ChangeNameToStandard(fileName, revLocation.Name, nameSt).GetModifiedName();
                try
                {
                    using (Transaction tx = new Transaction(doc, "Change Location"))
                    {
                        tx.Start();
                        doc.ActiveProjectLocation = revLocation;
                        if (exIFC)
                        {
                            doc.Export(selectedPath, fileNameForExport, IFCExportOpt);
                        }
                        
                        if (exDWFx)
                        {
                            doc.Export(selectedPath, fileNameForExport, viewSet, DWFXExportOpt);
                        }
                        tx.Commit();
                        if (exNWC)
                        {
                            doc.Export(selectedPath, fileNameForExport, NWCExportOptions);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }

        }
        public string GetName()
        { 
            return "Export Locations Handler";
        } 
    }
}
