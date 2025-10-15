using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WSPPolska_Tools.Commands;

namespace WSPPolska_Tools
{
    
    public class RevitExportEqWindowHandle : IWin32Window
    {
        private readonly IntPtr _handle;

        public RevitExportEqWindowHandle(IntPtr handle)
        {
            _handle = handle;
        }

        public IntPtr Handle => _handle;
    }
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class ExportEquipmentNuFormCommand : IExternalCommand
     {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            OpenFileDialog openDialog = new OpenFileDialog
            {
                Title = "Select an Excel File",
                Filter = "Excel Files|*.xls;*.xlsx"
            };

            DialogResult result = openDialog.ShowDialog();

            if (result != DialogResult.OK)
            {
                TaskDialog.Show("Warning", "No Excel file selected. Command will terminate.");
                return Result.Cancelled;
            }


            string selectedFile = openDialog.FileName;

            ExportEquipmentNuForm exEqform = new ExportEquipmentNuForm(selectedFile, commandData);
            Application.Run(exEqform);

            //form.Show(new RevitExportEqWindowHandle(commandData.Application.MainWindowHandle)); // Modal window

            return Result.Succeeded;
        }
    }

}
