using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace WSPPolska_Tools.Commands
{
    public partial class StructureCostForm : System.Windows.Forms.Form
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
        string userName;
        string fileName;
        string firstLocName;
        string selectedPath;
        int concretePrice;
        int reinforcementPrice;
        int structuralSteelPrice;
        Dictionary<string, List<FamilyInstance>> elementDictionaryFI = new Dictionary<string, List<FamilyInstance>>();
        Dictionary<string, List<Element>> elementDictionaryEl = new Dictionary<string, List<Element>>();
        Dictionary<string, int> priceList = new Dictionary<string, int>();
        List<ElementId> incorrectNaming = new List<ElementId>();
        List<ElementId> notCalculated = new List<ElementId>();
        List<ElementId> remainingElements = new List<ElementId>();

        public StructureCostForm(ExternalCommandData commandData)
        {
            InitializeComponent();

            this.MouseDown += StructureCostForm_MouseDown;
            _commandData = commandData;
            uiapp = _commandData.Application;
            uidoc = uiapp.ActiveUIDocument;
            doc = uidoc.Document;
            userName = $"_{uiapp.Application.Username}";
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open("I:\\0030\\TECHNICAL LIBRARY\\SMEP\\01 STRUCTURAL\\17_Kosztorysy\\StructuralUnitPrices.xlsx");
            Excel.Worksheet worksheet = workbook.Sheets[1];


            for (int row = 1; row <= 30; row++)
            {
                string key = worksheet.Cells[row, 1].Text;
                string valueText = worksheet.Cells[row, 2].Text;

                if (string.IsNullOrWhiteSpace(key))
                    continue; // Skip empty keys

                if (int.TryParse(valueText, out int value))
                {
                    priceList[key] = value; // Add to dictionary
                }
            }
            reinforcementPrice = priceList["Reinforcement"];
            concretePrice = priceList["Concrete"];
            structuralSteelPrice = priceList["Steel"];
            workbook.Close();
            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            generalReinforcementRatios.Rows.Add();
            pathTextBox.Text = @"I:\0030\TECHNICAL LIBRARY\SMEP\01 STRUCTURAL\17_Kosztorysy\StructuralUnitPrices.xlsx";

        }



        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;
            const int HTCLIENT = 1;
            const int HTTOP = 12;
            const int HTBOTTOM = 15;
            //const int HTTOPLEFT = 13;
            //const int HTTOPRIGHT = 14;
            //const int HTBOTTOMLEFT = 16;
            //const int HTBOTTOMRIGHT = 17;

            if (m.Msg == WM_NCHITTEST)
            {
                base.WndProc(ref m);

                if ((int)m.Result == HTCLIENT)
                {
                    System.Drawing.Point cursor = PointToClient(Cursor.Position);
                    int resizeArea = 8; // Thickness for resize area

                    bool top = cursor.Y <= resizeArea;
                    bool bottom = cursor.Y >= this.ClientSize.Height - resizeArea;

                    // Corners still allowed if you want diagonal resize
                    if (top)
                        m.Result = (IntPtr)HTTOP;
                    else if (bottom)
                        m.Result = (IntPtr)HTBOTTOM;
                }
                return;
            }

            base.WndProc(ref m);
        }



        private void StructureCostForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }
        private void ClearData_Click(object sender, EventArgs e)
        {
            CostInformationGrid.Rows.Clear();
            NotCalcNo.Text = string.Empty;
            IncNamingNo.Text = string.Empty;
            remainingCounter.Text = string.Empty;
        }

        private void SelectElementsFromRows_Click(object sender, EventArgs e)
        {
            if (CostInformationGrid.SelectedRows.Count > 0)
            {
                List<ElementId> elementIds = new List<ElementId>();
                foreach (DataGridViewRow selectedRow in CostInformationGrid.SelectedRows)
                {
                    elementIds.AddRange(selectedRow.Tag as List<ElementId>);
                }

                if (elementIds != null && elementIds.Count > 0)
                {
                    uidoc.Selection.SetElementIds(elementIds);
                }
            }
        }

        private void CostAnalysis_Click(object sender, EventArgs e)
        {

            elementDictionaryFI.Clear();
            elementDictionaryEl.Clear();
            incorrectNaming.Clear();
            notCalculated.Clear();
            remainingElements.Clear();
            CostInformationGrid.Rows.Clear();

            Dictionary<string, double> reinfRations = new Dictionary<string, double>();
            try
            { 
                if (double.TryParse(generalReinforcementRatios.Rows[0].Cells[0].Value.ToString(), out double beamR))
                {
                    reinfRations["Beams"] = beamR;
                }
                if (double.TryParse(generalReinforcementRatios.Rows[0].Cells[1].Value.ToString(), out double columnR))
                {
                    reinfRations["Columns"] = columnR;
                }

                if (double.TryParse(generalReinforcementRatios.Rows[0].Cells[2].Value.ToString(), out double slabR))
                {
                    reinfRations["Floors"] = slabR;
                }
                if (double.TryParse(generalReinforcementRatios.Rows[0].Cells[3].Value.ToString(), out double wallR))
                {
                    reinfRations["Walls"] = wallR;
                }
                if (double.TryParse(generalReinforcementRatios.Rows[0].Cells[4].Value.ToString(), out double foundationR))
                {
                    reinfRations["Foundations"] = foundationR;
                }
            }
            catch 
            {
                MessageBox.Show("Please provide general reinforcement Rations");
                return;
            }
            try
            {
                List<Element> selectedElements = new List<Element>();

                // Step 1: Let user select elements
                IList<Reference> selectedRefs = uidoc.Selection.PickObjects(ObjectType.Element, "Select elements to analyze");

                // Step 2: Collect elements
                selectedElements = selectedRefs
                    .Select(r => doc.GetElement(r))
                    .ToList();


                Dictionary<string, BuiltInCategory> categoryNamesFI = new Dictionary<string, BuiltInCategory>()
                {
                    { "Columns", BuiltInCategory.OST_StructuralColumns },
                    { "Beams", BuiltInCategory.OST_StructuralFraming }
                };
                Dictionary<string, BuiltInCategory> categoryNamesEl = new Dictionary<string, BuiltInCategory>()
                { 
                    { "Floors", BuiltInCategory.OST_Floors },
                    { "Foundations", BuiltInCategory.OST_StructuralFoundation },
                    { "Walls", BuiltInCategory.OST_Walls }
                };
                var filteredElementsFI = selectedElements
                                .Where(element => categoryNamesFI.Values.Any(cat => element.Category.Id.IntegerValue == (int)cat))
                                .ToList();
                var filteredElementsEl = selectedElements
                                .Where(element => categoryNamesEl.Values.Any(cat => element.Category.Id.IntegerValue == (int)cat))
                                .ToList();

                var filteredGroups = selectedElements
                    .Where(element => element.Category.Id.IntegerValue == (int)BuiltInCategory.OST_IOSModelGroups)
                    .ToList();


                remainingElements = selectedElements
                                .Except(filteredElementsFI.Concat(filteredElementsEl).Concat(filteredGroups))
                                .Select(f => f.Id)
                                .ToList();
                remainingCounter.Text = remainingElements.Count.ToString();

                foreach (FamilyInstance element in filteredElementsFI)
                {
                    string elCategory = categoryNamesFI.FirstOrDefault(x => (int)x.Value == element.Category.Id.IntegerValue).Key;
                    string elFamily = element.Symbol.Family.Name;
                    string elType = element.Symbol.Name;
                    string elMaterial = element.LookupParameter("WSP_MaterialClass").AsString();
                    string elReinforcementRatio = string.Empty;
                    if (elFamily.Contains("CNC"))
                    {
                        elReinforcementRatio = element.LookupParameter("WSP_ReinforcementRatio").AsValueString();
                    }
                    string dictionaryKey = string.Join("|", new[] { elCategory, elFamily, elType, elMaterial, elReinforcementRatio });
                    if (elementDictionaryFI.ContainsKey(dictionaryKey))
                    {
                        List<FamilyInstance> existingList = elementDictionaryFI[dictionaryKey];
                        existingList.Add(element);
                        elementDictionaryFI[dictionaryKey] = existingList;
                    }
                    else
                    {
                        List<FamilyInstance> newList = new List<FamilyInstance>() { element };
                        elementDictionaryFI[dictionaryKey] = newList;
                    }
                }
                foreach (Element element in filteredElementsEl)
                {
                    string elCategory = categoryNamesEl.FirstOrDefault(x => (int)x.Value == element.Category.Id.IntegerValue).Key;
                    //string elFamily = element.Symbol.Family.Name;
                    string elType = element.Name;
                    string elMaterial = element.LookupParameter("WSP_MaterialClass").AsString();
                    string elReinforcementRatio = string.Empty;
                    elReinforcementRatio = element.LookupParameter("WSP_ReinforcementRatio").AsValueString();
                    string dictionaryKey = string.Join("|", new[] { elCategory, elType, elMaterial, elReinforcementRatio });

                    if (elementDictionaryEl.ContainsKey(dictionaryKey))
                    {
                        List<Element> existingListEl = elementDictionaryEl[dictionaryKey];
                        existingListEl.Add(element);
                        elementDictionaryEl[dictionaryKey] = existingListEl;
                    }
                    else
                    {
                        List<Element> newList = new List<Element>() { element };
                        elementDictionaryEl[dictionaryKey] = newList;
                    }
                }

                foreach(var kvp in elementDictionaryFI.OrderBy(k => k.Key))
                {
                    string[] dKey = kvp.Key.Split('|');

                    string elCategory = dKey.Length > 0 ? dKey[0] : string.Empty;
                    string elFamily = dKey.Length > 1 ? dKey[1] : string.Empty;
                    string elType = dKey.Length > 1 ? dKey[2] : string.Empty;
                    string elMaterial = dKey.Length > 2 ? dKey[3] : string.Empty;
                    double elReinforcementRatio = 0;
                    double elReinforcementRatioToShow = 0;
                    List<FamilyInstance> familyInstances = kvp.Value;
                    double volume = 0;
                    double length = 0;
                    double unitCost = 0;
                    double totalCost = 0;
                    if (elFamily.Contains("CNC"))
                    {
                        if (priceList.ContainsKey(elMaterial))
                        {
                            concretePrice = priceList[elMaterial];
                        }
                        foreach (var familyInstance in familyInstances)
                        {
                            try
                            {
                                volume += Math.Round(familyInstance.LookupParameter("Volume").AsDouble() * 0.028316846, 2);
                            }
                            catch
                            {
                                MessageBox.Show($"Element {familyInstance.Id.IntegerValue} cannot be checked for volume");
                                notCalculated.Add(familyInstance.Id);
                            }
                        }
                        if (double.TryParse(dKey[4], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out elReinforcementRatio) && elReinforcementRatio > 0)
                        {
                            elReinforcementRatioToShow = elReinforcementRatio;
                        }
                        else
                        {
                            elReinforcementRatioToShow = 0;
                            elReinforcementRatio = reinfRations[elCategory];
                        }

                        unitCost = elReinforcementRatio * reinforcementPrice / 1000 + concretePrice;
                        totalCost = unitCost * volume;
                        int rowIndex = CostInformationGrid.Rows.Add("CO " + elCategory, elType, elMaterial, volume, elReinforcementRatioToShow, length, unitCost, Math.Round(totalCost, 0));
                        CostInformationGrid.Rows[rowIndex].Tag = familyInstances.Select(f => f.Id).Distinct().ToList();
                    }
                    else if (elFamily.Contains("STL"))
                    {
                        double kgPerm = 0;
                        if (priceList.ContainsKey(elMaterial))
                        {
                            structuralSteelPrice = priceList[elMaterial];
                        }
                        
                        foreach (var familyInstance in familyInstances)
                        {
                            try
                            {
                                length += Math.Round(familyInstance.LookupParameter("Length").AsDouble() * 0.3048, 2);
                            }
                            catch
                            {
                                MessageBox.Show($"Script cannot find length value for element with Id {familyInstance.Id.IntegerValue}");
                                notCalculated.Add(familyInstance.Id);
                            }
                        }
                        try
                        {
                            kgPerm = familyInstances[0].Symbol.LookupParameter("WSP_MassPerUnitLength").AsDouble() / 0.3048;
                            if (kgPerm == 9999)
                            {
                                MessageBox.Show($"Type {elType} does not have kg/m value provided.");
                                notCalculated.AddRange(familyInstances.Select(f => f.Id).Distinct().ToList());
                                continue;
                            }
                            unitCost = kgPerm /1000 * structuralSteelPrice;
                        }
                        catch
                        {
                            MessageBox.Show($"{familyInstances[0].Symbol.Id.IntegerValue.ToString()} type id, WSP_MassPerUnitLength issue.\nProbably the parameter does not exists in family.");
                        }
                        totalCost = (unitCost * length);
                        int rowIndex = CostInformationGrid.Rows.Add("ST "+elCategory, elType, elMaterial, volume, kgPerm, length, unitCost, Math.Round(totalCost, 0));
                        CostInformationGrid.Rows[rowIndex].Tag = familyInstances.Select(f => f.Id).Distinct().ToList();
                    }
                    else
                    {
                        foreach (var familyInstance in familyInstances)
                        {
                            incorrectNaming.Add(familyInstance.Id);
                        }
                    }

                }
                foreach (var kvp in elementDictionaryEl.OrderBy(k => k.Key))
                {
                    string[] dKey = kvp.Key.Split('|');
                    string elCategory = dKey.Length > 0 ? dKey[0] : string.Empty;
                    string elType = dKey.Length > 1 ? dKey[1] : string.Empty;
                    string elMaterial = dKey.Length > 2 ? dKey[2] : string.Empty;
                    double elReinforcementRatio = 0;
                    double elReinforcementRatioToShow = 0;
                    List<Element> familyInstances = kvp.Value;
                    double volume = 0;
                    double length = 0;
                    double unitCost = 0;
                    double totalCost = 0;
                    foreach (Element element in familyInstances)
                    {
                        try
                        {
                            volume += Math.Round(element.LookupParameter("Volume").AsDouble() * 0.028316846, 2);
                        }
                        catch
                        {
                            MessageBox.Show($"Element {element.Id.IntegerValue} cannot be checked for volume");
                            notCalculated.Add(element.Id);
                        }
                    }
                    if (double.TryParse(dKey[3], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out elReinforcementRatio) && elReinforcementRatio > 0)
                    {
                        elReinforcementRatioToShow = elReinforcementRatio;
                    }
                    else
                    {
                        elReinforcementRatioToShow = 0;
                        elReinforcementRatio = reinfRations[elCategory];
                    }
                    unitCost = elReinforcementRatio * reinforcementPrice / 1000 + concretePrice;
                    totalCost = unitCost * volume;

                    int rowIndex = CostInformationGrid.Rows.Add(elCategory, elType, elMaterial, volume, elReinforcementRatioToShow, length, unitCost, Math.Round(totalCost, 0));
                    CostInformationGrid.Rows[rowIndex].Tag = familyInstances.Select(f => f.Id).Distinct().ToList();
                }
                double wholeTotalCost = 0;
                foreach (DataGridViewRow row in CostInformationGrid.Rows)
                {

                    if (!row.IsNewRow)
                    {
                        var cellValue = row.Cells[CostInformationGrid.Columns.Count - 1].Value;
                        if (cellValue != null && double.TryParse(cellValue.ToString(), out double value))
                        {
                            wholeTotalCost += value;
                        }
                    }

                }
                CostInformationGrid.Sort(CostInformationGrid.Columns[0], ListSortDirection.Ascending);
                CostInformationGrid.Rows.Add();
                CostInformationGrid.Rows.Add();
                string formatted = Math.Round(wholeTotalCost, 2).ToString("N2", System.Globalization.CultureInfo.CurrentCulture);
                CostInformationGrid.Rows.Add("TotalPrice", "", "", "", "", "", "", formatted);
                IncNamingNo.Text = incorrectNaming.Count.ToString();
                NotCalcNo.Text = notCalculated.Count.ToString();
                // Step 5: Populate DataGridView



            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                TaskDialog.Show("Cancelled", "Selection was cancelled.");
            }


        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CostInformationGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void IncNaming_Click(object sender, EventArgs e)
        {
            uidoc.Selection.SetElementIds(incorrectNaming);
        }

        private void VolNotCal_Click(object sender, EventArgs e)
        {
            uidoc.Selection.SetElementIds(notCalculated);
        }
        private void RemainingSel_Click(object sender, EventArgs e)
        {
            uidoc.Selection.SetElementIds(remainingElements);
        }

      
    }
}
