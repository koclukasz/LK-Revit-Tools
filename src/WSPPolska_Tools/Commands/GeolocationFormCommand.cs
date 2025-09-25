using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSPPolska_Tools
{
    
    public class RevitWindowHandle : IWin32Window
    {
        private readonly IntPtr _handle;

        public RevitWindowHandle(IntPtr handle)
        {
            _handle = handle;
        }

        public IntPtr Handle => _handle;
    }
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class GeolocationFormCommand : IExternalCommand
     {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var form = new GeolocationForm(commandData);
            
            form.Show(new RevitWindowHandle(commandData.Application.MainWindowHandle)); // Modal window
            
            return Result.Succeeded;
        }
    }

}
