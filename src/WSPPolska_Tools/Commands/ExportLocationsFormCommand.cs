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
    
    public class RevitExportLocWindowHandle : IWin32Window
    {
        private readonly IntPtr _handle;

        public RevitExportLocWindowHandle(IntPtr handle)
        {
            _handle = handle;
        }

        public IntPtr Handle => _handle;
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class ExportLocationsFormCommand : IExternalCommand
     {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var form = new ExportLocationsForm(commandData);
            
            form.Show(new RevitExportLocWindowHandle(commandData.Application.MainWindowHandle)); // Modal window
            
            return Result.Succeeded;
        }
    }

}
