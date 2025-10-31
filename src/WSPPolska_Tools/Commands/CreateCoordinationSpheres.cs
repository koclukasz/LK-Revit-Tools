using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using BIM.IFC.Export.UI;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using Excel = Microsoft.Office.Interop.Excel;



namespace WSPPolska_Tools
{



    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]

    public partial class CreateCoordinationSpheres : System.Windows.Forms.Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        private ExternalCommandData _commandData;
        private Document _doc;
        private UIApplication _uiapp;
        private UIDocument _uidoc;
        string _userName;
        private Dictionary<string, FamilySymbol> _typeDict = new Dictionary<string, FamilySymbol>();
        private Dictionary<int, Element> _allCoorSphDict = new Dictionary<int, Element>();
        private Dictionary<string, Workset> _worksetsDic = new Dictionary<string, Workset>();


        //Dictionary<string, NewLocationData> allNewLocations = new Dictionary<string, NewLocationData>();
        public CreateCoordinationSpheres(ExternalCommandData commandData)
        {
            this.MouseDown += CreateCoordinationSpheres_MouseDown;
            _commandData = commandData;
            _uiapp = _commandData.Application;
            _uidoc = _uiapp.ActiveUIDocument;
            _doc = _uidoc.Document;
            _userName = $"_{_uiapp.Application.Username}";

            InitializeCoordinationSphereData();
            InitializeComponent();
        }

        private void InitializeCoordinationSphereData()
        {
            List<Element> _allCoorSph = new FilteredElementCollector(_doc).OfClass(typeof(FamilyInstance)).Cast<FamilyInstance>().Where(fi => fi.Symbol != null && fi.Symbol.Family != null && fi.Symbol.Family.Name == "XX_GM_CLB_ClashSymbol").Cast<Element>().ToList();
            _allCoorSphDict = _allCoorSph.ToDictionary(e => e.Id.IntegerValue);
            _typeDict = new FilteredElementCollector(_doc).OfClass(typeof(FamilySymbol)).Cast<FamilySymbol>().Where(symbol => symbol.Family != null && symbol.Family.Name == "XX_GM_CLB_ClashSymbol").ToDictionary(symbol => symbol.Name);
            //MessageBox.Show($"{_allCoorSphDict.Count}_{_typeDict.Count}");
            if (_typeDict.Count == 0) { MessageBox.Show("Please load Sphere Family"); }
        }

        private void CreateCoordinationSpheres_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }



        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SynchronizeExcel_Click(object sender, EventArgs e)
        {

            if (!_doc.IsWorkshared)
            {
                TaskDialog.Show("Worksets Required", "This document is not workshared. Worksets are not available.");
                return;
            }
            ICollection<Workset> userWorksets = new FilteredWorksetCollector(_doc).OfKind(WorksetKind.UserWorkset).ToWorksets();


            OpenFileDialog openExcelExportDialog = new OpenFileDialog
            {
                Title = "Select an Excel File",
                Filter = "Excel Files|*.xls;*.xlsx"
            };
            DialogResult result = openExcelExportDialog.ShowDialog();
            if (result != DialogResult.OK)
            {
                MessageBox.Show("No Excel file selected. Command will terminate.", "Warning", MessageBoxButtons.OK);
                return;
            }
            Main.createSpheresHandler.excelPath = openExcelExportDialog.FileName;
            Main.createSpheresHandler.userWorksets = userWorksets;
            Main.createSpheresHandler.typeDict = _typeDict;
            Main.createSpheresHandler.allCoorSphDict = _allCoorSphDict;

            Main.createSpheresEvent.Raise();





            //Excel.Application excelApp = new Excel.Application();
            //Excel.Workbook workbook = excelApp.Workbooks.Open(openExcelExportDialog.FileName);
            //foreach (Worksheet clashTestWorksheet in workbook.Worksheets)
            //{
            //    int lastRow = ExcelHelper.GetLastNonEmptyRow(clashTestWorksheet);
            //    string clashTestName = clashTestWorksheet.Name;
            //    Workset selectedWorkset = userWorksets.FirstOrDefault(ws => ws.Name.Equals(clashTestName, StringComparison.OrdinalIgnoreCase));
            //    if (selectedWorkset == null)
            //    {
            //        Main.createWorksetHandler.worksetName = clashTestName;
            //        Main.createWorksetEvent.Raise();
            //        selectedWorkset = Main.createWorksetHandler.CreatedWorkset;
            //        //MessageBox.Show($"{selectedWorkset.Id.IntegerValue}");
            //    }
            //    MessageBox.Show(lastRow.ToString());
            //for (int row = 1; row <= lastRow; row++)
            //{
            //    string trigger = clashTestWorksheet.Cells[row, 3]?.Value2?.ToString(); // Column C
            //    if (string.IsNullOrWhiteSpace(trigger))
            //        continue;

            //    string comment = clashTestWorksheet.Cells[row, 1]?.Value2?.ToString(); // Column A
            //    string status = clashTestWorksheet.Cells[row, 2]?.Value2?.ToString();  // Column B
            //    string locationStr = clashTestWorksheet.Cells[row, 6]?.Value2?.ToString(); // Column F
            //    string elementIdStr = clashTestWorksheet.Cells[row, 7]?.Value2?.ToString(); // Column G
            //    FamilySymbol symbol;
            //    FamilyInstance clashElement = null;
            //    if (string.IsNullOrWhiteSpace(elementIdStr))
            //    {

            //        if (!typeDict.TryGetValue(status, out symbol))
            //        {
            //            MessageBox.Show($"{status} not found as Type");
            //            continue;
            //        }

            //        XYZ location = ParseLocation(locationStr);
            //        if (location == null)
            //        {
            //            clashTestWorksheet.Cells[row, 7].Value2 = "Location Incorrect"; // Write ID to column G
            //            continue;
            //        }

            //        Main.createSpheresHandler.location = location;
            //        Main.createSpheresHandler.symbol = symbol;
            //        Main.createSpheresHandler.workset = selectedWorkset;
            //        Main.createSpheresHandler.commentVal = comment;
            //        Main.createSpheresEvent.Raise();

            //        clashElement = doc.Create.NewFamilyInstance(location, symbol, StructuralType.NonStructural);
            //        clashTestWorksheet.Cells[row, 7].Value2 = clashElement.Id.IntegerValue; // Write ID to column G
            //    }
            //    else
            //    {
            //        clashElement = allCoorSphDict[clashElement.Id.IntegerValue] as FamilyInstance;
            //        if (clashElement.Symbol.Name != status)
            //        {
            //            //modify ty1pe
            //        }
            //    }

            //if (clashElement != null)
            //{
            //    using (Transaction tx = new Transaction(doc, "Update Clash Element"))
            //    {
            //        tx.Start();

            //        // Update Workset
            //        WorksetTable worksetTable = doc.GetWorksetTable();
            //        Workset workset = worksetTable.GetWorkset();
            //        if (workset != null)
            //        {
            //            Parameter wsParam = clashElement.get_Parameter(BuiltInParameter.ELEM_PARTITION_PARAM);
            //            if (wsParam != null && !wsParam.IsReadOnly)
            //                wsParam.Set(workset.Id.IntegerValue);
            //        }

            //        // Update Status (Type Name)
            //        FamilyInstance fi = clashElement as FamilyInstance;
            //        if (fi != null && fi.Symbol.Name != status)
            //        {
            //            FamilySymbol newSymbol = new FilteredElementCollector(doc)
            //                .OfClass(typeof(FamilySymbol))
            //                .Cast<FamilySymbol>()
            //                .FirstOrDefault(s => s.Name == status && s.Family.Name == "XX_GM_CLB_ClashSymbol");

            //            if (newSymbol != null)
            //                fi.Symbol = newSymbol;
            //        }

            //        // Update Comment
            //        Parameter commentParam = clashElement.LookupParameter("Comments");
            //        if (commentParam != null && !commentParam.IsReadOnly)
            //            commentParam.Set(comment);

            //        tx.Commit();
            //}
            //}
        }
    }
}
//}
