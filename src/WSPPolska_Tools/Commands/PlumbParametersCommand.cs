namespace WSPPolska_Tools
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Autodesk.Revit.UI;
    using Autodesk.Revit.Exceptions;
    using Autodesk.Revit.ApplicationServices;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Plumbing;
    using Autodesk.Revit.DB.Mechanical;
    using System.Diagnostics;
    using System;

    /// <summary>
    /// Command to Be executed when button clicked
    /// </summary>
    /// <seealso cref="Autodesk.Revit.UI.IExternalCommand"></seealso>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]


    public class PlumbParametersCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            throw new System.NotImplementedException();
        }
    }
}
