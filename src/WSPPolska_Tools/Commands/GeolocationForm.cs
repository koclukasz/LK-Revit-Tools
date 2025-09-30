using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Globalization;



namespace WSPPolska_Tools
{

    

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    
    public partial class GeolocationForm : System.Windows.Forms.Form
    {
        private ExternalCommandData _commandData;
        private Document doc;
        private UIApplication uiapp;
        private UIDocument uidoc;
        string disciplineRepl = "";
        int disciplineReplInd;
        string userName;
       

        //Dictionary<string, NewLocationData> allNewLocations = new Dictionary<string, NewLocationData>();
        public GeolocationForm(ExternalCommandData commandData)
        {
            _commandData = commandData;
            uiapp = _commandData.Application;
            uidoc = uiapp.ActiveUIDocument;
            doc = uidoc.Document;
            userName = $"_{uiapp.Application.Username}";
            InitializeComponent();
            try
            {
                string[] splittedName = Regex.Split(doc.Title.Replace(userName,""), @"[,; _-]+");
                foreach (string namePart in splittedName)
                {
                    checkedListBox1.Items.Add(namePart);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        

        private void ExportData_Click(object sender, EventArgs e)
        {
            BasePoint basePoint = BasePoint.GetProjectBasePoint(doc);
            XYZ basePointPos = basePoint.SharedPosition;
            if (disciplineRepl == "")
            {
                MessageBox.Show("Please select block to be replaced in file name", "Warning", MessageBoxButtons.OK);
                return;
            }
            List<RevitLinkInstance> pickedElements;
            try
            {
                IList<Reference> pickedRefs = uidoc.Selection.PickObjects(ObjectType.Element, "Select multiple links");
                pickedElements = pickedRefs.Select(r => doc.GetElement(r)).OfType<RevitLinkInstance>().ToList();
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                TaskDialog.Show("Selection Canceled", "You canceled the selection. No elements were picked.");
                return;
            }


            foreach (Instance linkedModel in pickedElements)
            {
                Transform modelTransform = linkedModel.GetTotalTransform();
                XYZ modelTransformVector = modelTransform.BasisX;
                // Project to XY plane (ignore Z)
                XYZ modelVec = new XYZ(modelTransformVector.X, modelTransformVector.Y, 0);
                XYZ northVec = new XYZ(1, 0, 0);// East direction as reference
                XYZ planeVec = new XYZ(0, 0, 1);// Plane reference
                // Compute transformed coordinates
                string typeName = linkedModel.LookupParameter("Type").AsValueString();
                //updated type name to remove Discipline
                string toBeReplaced = Regex.Split(typeName, @"[,; _-]+")[disciplineReplInd];
                string changedTypeName = typeName.Replace(toBeReplaced, "DISC").Replace(".rvt","");
                string locationName = linkedModel.LookupParameter("Name").AsValueString();
                double EW = (basePointPos.X + modelTransform.Origin.X)*0.3048;
                double NS = (basePointPos.Y + modelTransform.Origin.Y) * 0.3048;
                double Z = (basePointPos.Z + modelTransform.Origin.Z) * 0.3048;
                // Calculate angle to north (in radians)
                double angleToNorth = 2*Math.PI - northVec.AngleOnPlaneTo(modelVec, planeVec);
                double angleDegrees = angleToNorth*(180/Math.PI);
                //MessageBox.Show($"Angle{angleDegrees}\nEW = {EW}\nNS = {NS}","BasePointValues",MessageBoxButtons.OK);
                PositionsDataGrid.Rows.Add(changedTypeName, locationName, Math.Round(EW, 3), Math.Round(NS, 3), Math.Round(Z, 3), Math.Round(angleDegrees, 3));
            }
            PositionsDataGrid.Sort(PositionsDataGrid.Columns[1], ListSortDirection.Ascending);

            //open file dialog for Excel to save Coordinates
            DialogResult fileLoaded = openCoordinatesExcel.ShowDialog();
            if (fileLoaded != DialogResult.OK)
            {
                MessageBox.Show("File was not selected. Script will be terminated.", "Warning", MessageBoxButtons.OK);
                return;
            }
            MessageBox.Show($"File {openCoordinatesExcel.SafeFileName} Loaded", "Success", MessageBoxButtons.OK);


            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(openCoordinatesExcel.FileName);
            Excel.Worksheet worksheet = workbook.Sheets[1];
            int lastRowNo = worksheet.UsedRange.Rows.Count;
            foreach (DataGridViewRow row in PositionsDataGrid.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    lastRowNo++;
                    worksheet.Cells[lastRowNo, 1].Value = row.Cells[0].Value?.ToString();
                    worksheet.Cells[lastRowNo, 2].Value = row.Cells[1].Value?.ToString();
                    worksheet.Cells[lastRowNo, 3].Value = row.Cells[2].Value;
                    worksheet.Cells[lastRowNo, 4].Value = row.Cells[3].Value;
                    worksheet.Cells[lastRowNo, 5].Value = row.Cells[4].Value;
                    worksheet.Cells[lastRowNo, 6].Value = row.Cells[5].Value;
                }
            }
            workbook.Save();
            workbook.Close();
            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            MessageBox.Show("Data Exported");
            //try
            //{
            //    // Prompt user to select elements
            //    ICollection<ElementId> selectedIds = uidoc.Selection.GetElementIds();

            //    if (selectedIds.Count == 0)
            //    {
            //        TaskDialog.Show("Selection", "Please select one or more elements in the Revit UI before running the command.");
            //        return;
            //    }

            //    // Access the selected elements
            //    List<Element> selectedElements = selectedIds.Select(id => doc.GetElement(id)).ToList();

            //    // Do something with the selected elements
            //    TaskDialog.Show("Selected Count", $"You selected {selectedElements.Count} elements.");


            //    return;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

        }

        private void PositionsDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        string fileDiscName; 


        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                // Uncheck all other items
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (i != e.Index)
                    {
                        checkedListBox1.SetItemChecked(i, false);
                    }
                }
            }
            // Delay execution until after the check state is updated
            this.BeginInvoke((MethodInvoker)delegate 
            {
                if (checkedListBox1.CheckedItems.Count == 1)
                {
                    disciplineReplInd = checkedListBox1.CheckedIndices[0];
                    disciplineRepl = checkedListBox1.CheckedItems[0].ToString();
                    fileDiscName = doc.Title.Replace(disciplineRepl, "DISC");

                    try
                    {
                        fileDiscName = fileDiscName.Replace(userName, "");
                    }
                    catch { }

                    StandardName.Text = fileDiscName;
                }
                else if (checkedListBox1.CheckedItems.Count > 1)
                {
                    MessageBox.Show("Check only one item to be replaced", "Warning", MessageBoxButtons.OK);
                }
            });
        }


        private void ImportLocations_Click(object sender, EventArgs e)
        {
            Dictionary<string, NewLocationData> allNewLocations = new Dictionary<string, NewLocationData>();
            BasePoint basePoint = BasePoint.GetProjectBasePoint(doc);
            XYZ basePointPos = basePoint.Position;
            //open File Dialog to import Excel
            DialogResult fileLoaded = OpenForImport.ShowDialog();
            if (fileLoaded != DialogResult.OK)
            {
                MessageBox.Show("File was not selected. Script will be terminated.", "Warning", MessageBoxButtons.OK);
                return;
            }
            MessageBox.Show($"File {OpenForImport.SafeFileName} Loaded", "Success", MessageBoxButtons.OK);
            if (fileDiscName == null)
            {
                MessageBox.Show("Please select Field to be replaced");
                return;
            }
            
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(OpenForImport.FileName);
            Excel.Worksheet worksheet = workbook.Sheets[1];
            int lastRowNo = worksheet.UsedRange.Rows.Count;


            for (int i = 2; i < lastRowNo; i++)
            {
                if (worksheet.Cells[i, 1].Text == fileDiscName)
                { 
                    double val3 = Convert.ToDouble(worksheet.Cells[i, 3].Value ?? 0, CultureInfo.GetCultureInfo("pl-PL"));
                    double val4 = Convert.ToDouble(worksheet.Cells[i, 4].Value ?? 0, CultureInfo.GetCultureInfo("pl-PL"));
                    double val5 = Convert.ToDouble(worksheet.Cells[i, 5].Value ?? 0, CultureInfo.GetCultureInfo("pl-PL"));
                    double val6 = Convert.ToDouble(worksheet.Cells[i, 6].Value ?? 0, CultureInfo.GetCultureInfo("pl-PL"));
                    string key = worksheet.Cells[i, 2].Text;
                    if (!allNewLocations.ContainsKey(key))
                    {
                        allNewLocations[key] = new NewLocationData(new ElementId(-1), key, val3, val4, val5, val6 );
                    }
                }
            }
            //workbook?.Save();
            if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
            workbook?.Close();
            if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            excelApp?.Quit();
            if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

            if (allNewLocations.Count == 0)
            {
                MessageBox.Show("No Locations For this File", "Warning", MessageBoxButtons.OK);
                return;
            }
            else
            { 
                MessageBox.Show("Data Imported", "Success", MessageBoxButtons.OK);
            }
            ProjectLocationSet allRevLocations = doc.ProjectLocations;

            List<ProjectLocation> locToDelete = new List<ProjectLocation>();
            //Dictionary<string, ProjectLocation> locToCorrect = new Dictionary<string, ProjectLocation>();
            foreach (ProjectLocation revLocation in allRevLocations)
            {
                if (!allNewLocations.ContainsKey(revLocation.Name))
                { 
                    locToDelete.Add(revLocation);
                    continue;
                }
                else
                {
                    NewLocationData locElement = allNewLocations[revLocation.Name];
                    locElement.ElementId = revLocation.Id;
                }
            }
            List<ElementId> idsToDel = new List<ElementId>();
            foreach (ProjectLocation dexLocation in locToDelete)
            {
                Transform prTransf= dexLocation.GetTotalTransform();
                //double NS = prTransf.Origin.X * 0.3048;
                //double EW = prTransf.Origin.Y * 0.3048;
                //double Z = prTransf.Origin.Z * 0.3048;
                ProjectPosition projectPositionPoint = dexLocation.GetProjectPosition(basePointPos);
                //double EW = (basePointPos.X + prTransf.Origin.X) * 0.3048;
                //double NS = (basePointPos.Y + prTransf.Origin.Y) * 0.3048;
                //double Z = (basePointPos.Z + prTransf.Origin.Z) * 0.3048;
                //XYZ modelVec = prTransf.BasisX;
                //XYZ northVec = new XYZ(1, 0, 0);// East direction as reference
                //XYZ planeVec = new XYZ(0, 0, 1);// Plane reference
                //double angleToNorth = northVec.AngleOnPlaneTo(modelVec, planeVec);
                double angleDegrees = projectPositionPoint.Angle * (180 / Math.PI);
                //PositionsDataGrid.Rows.Add("DELETED", dexLocation.Name, Math.Round(EW, 3), Math.Round(NS, 3), Math.Round(Z, 3), Math.Round(angleDegrees, 3));
                PositionsDataGrid.Rows.Add("DELETED", dexLocation.Name, Math.Round(projectPositionPoint.EastWest * 0.3048, 3), Math.Round(projectPositionPoint.NorthSouth * 0.3048, 3), Math.Round(projectPositionPoint.Elevation * 0.3048, 3), Math.Round(angleDegrees, 3));
                idsToDel.Add(dexLocation.Id);

            }

            //checking if current location shall be deleted 
            if (idsToDel.Contains(doc.ActiveProjectLocation.Id))
            {
                Main.changeLocHandler.newLocation = allNewLocations.OrderBy(entry => entry.Key).First().Value.ElementId;
                Main.changeLocEvent.Raise();
            }


            Main.deleteHandler.elementIds = idsToDel;
            Main.deleteEvent.Raise();

            Main.updateLocHandler.newLocationDic = allNewLocations;
            Main.updateLocEvent.Raise();



            foreach (KeyValuePair<string, NewLocationData> loc in allNewLocations)
            {
                //MessageBox.Show($"{loc.Key} id {loc.Value.ElementId.IntegerValue}");
                if (loc.Value.ElementId.IntegerValue == -1)
                {
                    PositionsDataGrid.Rows.Add("NEW", loc.Key, Math.Round(loc.Value.EW, 3), Math.Round(loc.Value.NS, 3), Math.Round(loc.Value.EL, 3), Math.Round(loc.Value.Rot, 3));
                }
                else
                { 
                    PositionsDataGrid.Rows.Add("EDITED", loc.Key, Math.Round(loc.Value.EW, 3), Math.Round(loc.Value.NS, 3), Math.Round(loc.Value.EL, 3), Math.Round(loc.Value.Rot, 3));
                }

            }


            MessageBox.Show("Locations updated");
            
        }

        private void StandardName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
