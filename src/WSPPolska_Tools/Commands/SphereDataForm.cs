using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using BIM.IFC.Export.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using Excel = Microsoft.Office.Interop.Excel;

namespace WSPPolska_Tools.Commands
{

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]


    public partial class SphereDataForm : System.Windows.Forms.Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        private ExternalCommandData _commandData;
        private Document doc;
        private UIApplication uiapp;
        private UIDocument uidoc;
        Element linkedElement = null;
        public SphereDataForm(ExternalCommandData commandData)
        {
            this.MouseDown += SpehereDataForm_MouseDown;
            _commandData = commandData;
            uiapp = _commandData.Application;
            uidoc = uiapp.ActiveUIDocument;
            doc = uidoc.Document;
            InitializeComponent();
        }

        private void SpehereDataForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }
        private void SphereDataForm_Load(object sender, EventArgs e)
        {

        }

        private void SphereId_Click(object sender, EventArgs e)
        {
            try
            {
                // Custom filter: only allow linked model elements
                ISelectionFilter linkElementFilter = new LinkElementSelectionFilter();

                // Step 1: Let user pick ONE element from a link
                Reference pickedRef = uidoc.Selection.PickObject(ObjectType.LinkedElement, linkElementFilter, "Select one element from a linked model");

                // Step 2: Get the linked element
                RevitLinkInstance linkInstance = doc.GetElement(pickedRef.ElementId) as RevitLinkInstance;
                Document linkedDoc = linkInstance.GetLinkDocument();
                linkedElement = linkedDoc.GetElement(pickedRef.LinkedElementId);

                // Copy LinkedElementId to clipboard
                Clipboard.SetText(linkedElement.Id.IntegerValue.ToString());
                sphereIdBox.Text = pickedRef.LinkedElementId.IntegerValue.ToString();

                //
                string elemsIds1Parm = linkedElement.LookupParameter("Item1 Ids").AsString();
                elemsIds1Box.Text = elemsIds1Parm;
                string elemsIds2Parm = linkedElement.LookupParameter("Item2 Ids").AsString();
                elemsIds2Box.Text = elemsIds2Parm;
                // Show confirmation
                TaskDialog.Show("Selection", $"You selected: {linkedElement.Name}\nID copied to clipboard: {linkedElement.Id.IntegerValue}");
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                TaskDialog.Show("Cancelled", "Selection was cancelled.");
            }
        }
        // Custom filter class
        private void ElemsIds1_Click(object sender, EventArgs e)
        {
            string elemsIds1Parm = linkedElement.LookupParameter("Item1 Ids").AsString();
            // Copy LinkedElementId to clipboard
            Clipboard.SetText(elemsIds1Parm);
        }


        public class LinkElementSelectionFilter : ISelectionFilter
        {
            public bool AllowElement(Element elem)
            {
                // Only allow RevitLinkInstance
                return elem is RevitLinkInstance;
            }

            public bool AllowReference(Reference reference, XYZ position)
            {
                return true; // We allow references inside links
            }


        }

        private void elemsIds2_Click(object sender, EventArgs e)
        {
            string elemsIds2Parm = linkedElement.LookupParameter("Item2 Ids").AsString();
            // Copy LinkedElementId to clipboard
            Clipboard.SetText(elemsIds2Parm);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}


