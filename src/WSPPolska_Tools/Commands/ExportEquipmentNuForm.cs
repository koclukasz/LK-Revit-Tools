using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Excel = Microsoft.Office.Interop.Excel;

namespace WSPPolska_Tools
{
    public partial class ExportEquipmentNuForm : System.Windows.Forms.Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        private string selectedExcelFilePath;
        Excel.Application excelApp;
        Excel.Workbook workbook;
        private ExternalCommandData _commandData;
        private Document doc;
        private UIApplication uiapp;
        private UIDocument uidoc;

        public ExportEquipmentNuForm(string excelFilePath, ExternalCommandData commandData)
        {
            this.MouseDown += ExportEquipmentNuForm_MouseDown;
            InitializeComponent();
            selectedExcelFilePath = excelFilePath;
            _commandData = commandData;
            uiapp = _commandData.Application;
            uidoc = uiapp.ActiveUIDocument;
            doc = uidoc.Document;
            MessageBox.Show($"File {Path.GetFileName(selectedExcelFilePath)} Loaded", "Success", MessageBoxButtons.OK);

            excelApp = new Excel.Application();
            workbook = excelApp.Workbooks.Open(selectedExcelFilePath);
            selectedParam.Items.Add("Mark");
            selectedParam.Items.Add("WSP_Number");
            selectedParam.Items.Add("Custom");
            foreach (Excel.Worksheet sheet in workbook.Sheets)
            {
                tabList.Items.Add($"{sheet.Index}_{sheet.Name}");
            }

        }


        private void ExportImpEquip_Click(object sender, EventArgs e)
        {
            List<string> itemList = new List<string>();
            List<string> excelItemsList = new List<string>();
            List<string> notInRevit = new List<string>();
            List<Element> notCorrectNaming = new List<Element>();
            List<string> notInExcel = new List<string>();
            List<int> notInExcelIds = new List<int>();
            int selectedIndex = tabList.SelectedIndex;
            if (selectedIndex == -1)
            {
                MessageBox.Show("Please select Excel Sheet");
                return;
            }
            if (selectedParam.SelectedIndex == -1)
            {
                MessageBox.Show("Please select Parameter");
                return;
            }
            if (numberingScheme.Text == "" || Regex.IsMatch(numberingScheme.Text, @"[^LN_.]"))
            {
                MessageBox.Show("Please provide correct scheme for numbering. Scheme might contains incorrect parameters. Other than LN_. are not allowed.");
                return;
            }
            string userPattern = numberingScheme.Text;
            string regexPat = userPattern.Replace(".", "\\.").Replace("_", "_").Replace("L", "[A-Z]").Replace("N", "\\d") + "$";

            Worksheet selectedSheet = workbook.Sheets[selectedIndex + 1];
            int lastRowNo = ExcelHelper.GetLastNonEmptyRow(selectedSheet);
            for (int i = 10; i <= lastRowNo; i++)
            {
                var cellValue = selectedSheet.Cells[i, 2].Value;
                excelItemsList.Add(cellValue?.ToString() ?? string.Empty);
            }
            string paramNumb = selectedParam.SelectedItem as string;
            if (paramNumb == "Custom")
            {
                paramNumb = customParameter.Text;
            }
            IList<Element> allEquipment = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_MechanicalEquipment).WhereElementIsNotElementType().ToElements();
            //scan through revit Mech Eqipment and checks if this is matchin naming and if is in Excel
            foreach (Element revMechEquipment in allEquipment)
            {
                try 
                { 
                    string eqNumber = revMechEquipment.LookupParameter(paramNumb).AsString();
                    bool isMatching = Regex.IsMatch(eqNumber, regexPat);
                    if (!excelItemsList.Contains(eqNumber) && isMatching)
                    {
                        notInExcel.Add(eqNumber);
                        notInExcelIds.Add(revMechEquipment.Id.IntegerValue);
                    }
                    else if (!excelItemsList.Contains(eqNumber))
                    {
                        notCorrectNaming.Add(revMechEquipment);
                    }
                    itemList.Add(eqNumber);
                }

                catch
                {
                }
            }
            string message = string.Join(Environment.NewLine, excelItemsList);
            string messageRv = string.Join(Environment.NewLine, itemList);
            // checking if items from Excel exists in the model
            foreach (string el in excelItemsList)
            {
                if (!itemList.Contains(el))
                { 
                    notInRevit.Add(el);
                }
            }
            //color change for red if element is not present in Revit and to transparent if exists in Revit. 
            for (int i = 10; i <= lastRowNo; i++)
            {
                var cellValue = selectedSheet.Cells[i, 2].Value;

                if (notInRevit.Contains(cellValue?.ToString()))
                {
                    Range cell = selectedSheet.Cells[i, 2];
                    cell.Interior.Color = ColorTranslator.ToOle(System.Drawing.Color.Red);
                }
                else
                {
                    Range cell = selectedSheet.Cells[i, 2];
                    cell.Interior.ColorIndex = Excel.XlColorIndex.xlColorIndexNone;
                }
            }
            int exIndex = lastRowNo+1;
            foreach (string el in notInExcel)
            {
                selectedSheet.Cells[exIndex, 2].Value = el;
                exIndex++;
            }

            Worksheet incorrectNamingSheet = workbook.Sheets["IncorrectNaming"];
            int lastRowIncorrectNamingNo = ExcelHelper.GetLastNonEmptyRow(incorrectNamingSheet);
            List<int> existingIds = new List<int>();
            for (int i = 1; i <= lastRowIncorrectNamingNo; i++)
            {
                try 
                { 
                    int cellValue = Convert.ToInt32(incorrectNamingSheet.Cells[i, 3].Value);
                    if (notInExcelIds.Contains(cellValue))
                    {
                        incorrectNamingSheet.Cells[i, 1].ClearContents();
                        incorrectNamingSheet.Cells[i, 2].ClearContents();
                        incorrectNamingSheet.Cells[i, 3].ClearContents();
                    }
                    else
                    { 
                        existingIds.Add(cellValue);
                    }
                }
                catch { }
            }
            int incorrectIndex = lastRowIncorrectNamingNo+1;
            foreach (Element el in notCorrectNaming)
            {
                if (!existingIds.Contains(el.Id.IntegerValue))
                {
                    incorrectNamingSheet.Cells[incorrectIndex, 1].Value = tabList.SelectedItem as string;
                    incorrectNamingSheet.Cells[incorrectIndex, 2].Value = el.LookupParameter(paramNumb).AsString(); ;
                    incorrectNamingSheet.Cells[incorrectIndex, 3].Value = el.Id.IntegerValue;
                    incorrectIndex++;
                }
            }

            workbook.Save();
            workbook.Close();
            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
        }

        private void ExportEquipmentNuForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            try
            {
                workbook.Save();
                workbook.Close();
                excelApp.Quit();
            }
            catch { } 
            this.Close();
        }
    }
}
