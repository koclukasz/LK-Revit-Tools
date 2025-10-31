using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace WSPPolska_Tools
{
    //[Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class CreateSphereHandler : IExternalEventHandler
    {
        public string excelPath { get; set; }
        
        public ICollection<Workset> userWorksets { get; set; }

        public Dictionary<string, FamilySymbol> typeDict { get; set; }
        public Dictionary<int, Element> allCoorSphDict {  get; set; }

        private XYZ ParseLocation(string input, double ex, double ey, double ez)
        {
            // Assumes format "X,Y,Z"
            var parts = input?.Split(';');
            if (parts?.Length == 3 &&
                double.TryParse(parts[0], out double x) &&
                double.TryParse(parts[1], out double y) &&
                double.TryParse(parts[2], out double z))
            {
                return new XYZ((x / 0.3048)-ex, (y / 0.3048)-ey, (z / 0.3048)-ez);
            }
            return null;
        }
        public void Execute(UIApplication app)

        {
            Document doc = app.ActiveUIDocument.Document;
            ICollection<Element> basePoints = new FilteredElementCollector(doc).OfClass(typeof(BasePoint)).ToElements();
            XYZ sharedPosition = null;
            XYZ basePosition = null;
            ICollection<ElementId> resolvedClashesSpheres = new List<ElementId>();
            foreach (BasePoint point in basePoints)
            {
                if (!point.IsShared)
                {
                    sharedPosition = point.SharedPosition;
                    basePosition = point.Position;
                    break;
                }
            }
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(excelPath);
            Excel.Worksheet worksheetForList = (Excel.Worksheet)workbook.Sheets["MaxRows"];
            //try
            //{ 
            using (Transaction tx = new Transaction(doc, "CreateShpere"))
            {
                tx.Start();
                int MaxRowInSummary = Convert.ToInt32(worksheetForList.Cells[1, 6].Value2);
                Dictionary<string, int> ClashTestRowNumDict = new Dictionary<string, int>();
                for (int i = 1; i < MaxRowInSummary; i++)
                {
                    ClashTestRowNumDict[worksheetForList.Cells[i, 1].Value2] = Convert.ToInt32(worksheetForList.Cells[i, 2].Value2);
                }
                foreach (Worksheet clashTestWorksheet in workbook.Worksheets)
                {
                    
                    string clashTestName = clashTestWorksheet.Name;
                    if (clashTestName == "MaxRows")
                    {
                        continue;
                    }
                    int lastRow = ClashTestRowNumDict[clashTestName];
                    Workset selectedWorkset = userWorksets.FirstOrDefault(ws => ws.Name.Equals(clashTestName, StringComparison.OrdinalIgnoreCase));
                    if (selectedWorkset == null)
                    {
                        selectedWorkset = Workset.Create(doc, clashTestName);
                    }
                    object[,] existingData = clashTestWorksheet.Range[$"A2:I{lastRow}"].Value2;
                    
                    for (int row = 1; row <= lastRow-1; row++)
                    {
                        string clashGuid = existingData[row, 4]?.ToString();  // Column D
                        if(string.IsNullOrWhiteSpace(clashGuid))
                        {
                            continue;
                        }
                        string sphereId = existingData[row, 8]?.ToString(); //ColumnH
                        string comment = existingData[row, 8]?.ToString(); //ColumnI
                        string status = existingData[row, 2]?.ToString();  // Column B
                        string priority = existingData[row, 3]?.ToString(); // Column C
                        string clashName = existingData[row, 1]?.ToString(); // Column A
                        string ids1 = existingData[row, 5]?.ToString();  // Column E
                        string ids2 = existingData[row, 6]?.ToString();  // Column F
                        string locationStr = existingData[row, 7]?.ToString(); // Column G
                        FamilySymbol symbol;
                        FamilyInstance clashElement = null;
                        if (string.IsNullOrWhiteSpace(sphereId))
                        {

                            if (!typeDict.TryGetValue(status, out symbol))
                            {
                                MessageBox.Show($"{status} not found as Type");
                                continue;
                            }
                            if (!symbol.IsActive)
                                symbol.Activate();

                            XYZ location = ParseLocation(locationStr, sharedPosition.X - basePosition.X, sharedPosition.Y - basePosition.Y, sharedPosition.Z - basePosition.Z);
                            if (location == null)
                            {
                                existingData[row, 9] = "Location Incorrect"; // Write ID to column G
                                continue;
                            }
                            clashElement = doc.Create.NewFamilyInstance(location, symbol, StructuralType.NonStructural);
                            Autodesk.Revit.DB.Parameter worksetParam = clashElement.get_Parameter(BuiltInParameter.ELEM_PARTITION_PARAM);
                            if (worksetParam != null && !worksetParam.IsReadOnly)
                            {
                                worksetParam.Set(selectedWorkset.Id.IntegerValue);
                            }
                            Autodesk.Revit.DB.Parameter ClashNumberParam = clashElement.LookupParameter("Clash Name");
                            Autodesk.Revit.DB.Parameter ClashTestNameParam = clashElement.LookupParameter("Clash Test Name");
                            Autodesk.Revit.DB.Parameter ClashPriorityParam = clashElement.LookupParameter("Clash Priority");
                            Autodesk.Revit.DB.Parameter ClashCommentsParam = clashElement.LookupParameter("Clash Comments");
                            Autodesk.Revit.DB.Parameter ClashSphereIdParam = clashElement.LookupParameter("Clash SphereId");
                            Autodesk.Revit.DB.Parameter Item1IdsParam = clashElement.LookupParameter("Item1 Ids");
                            Autodesk.Revit.DB.Parameter Item2IdsParam = clashElement.LookupParameter("Item2 Ids");
                            ClashNumberParam.Set(clashName);
                            ClashTestNameParam.Set(clashTestName);
                            ClashPriorityParam.Set(priority);
                            ClashCommentsParam.Set(comment);
                            ClashSphereIdParam.Set(clashElement.Id.IntegerValue.ToString());
                            Item1IdsParam.Set(ids1);
                            Item2IdsParam.Set(ids2);


                            existingData[row, 8] = clashElement.Id.IntegerValue.ToString(); // Write ID to column H
                        }
                        else
                        {
                            try
                            {
                                clashElement = allCoorSphDict[int.Parse(sphereId)] as FamilyInstance;
                                string SymbolName = clashElement.Symbol.Name;
                                if (status == "Resolved")
                                {
                                    resolvedClashesSpheres.Add(clashElement.Id);
                                    //existingData[row, 1] = "";
                                    existingData[row, 3] = "";
                                    existingData[row, 4] = "";
                                    existingData[row, 5] = "";
                                    existingData[row, 6] = "";
                                    existingData[row, 7] = "";
                                    existingData[row, 8] = "";
                                    existingData[row, 9] = "";
                                    //replace object with ""
                                }
                                else if (SymbolName != status)
                                {
                                    if (!typeDict.TryGetValue(status, out symbol))
                                    {
                                        clashTestWorksheet.Cells[row+1, 8].Value2 = "Cannot change status in Revit";
                                        continue;
                                    }
                                    if (!symbol.IsActive)
                                        symbol.Activate();

                                    clashElement.Symbol = symbol;
                                }
                            }
                            catch (Exception ex)
                            {
                                clashTestWorksheet.Cells[row+1, 8].Value2 = "No sphere with this ID";
                            }
                        }

                    }
                    try
                    {
                        //string rangeAddress = $"A2:I{lastRow}";
                        //object[,] writeArray = new object[newRows.Count, 9];
                        //worksheetForList.Cells[RowValueList, 3].Value = newRowEnd;

                        //for (int i = 0; i < newRows.Count; i++)
                        //    for (int j = 0; j < 9; j++)
                        //        writeArray[i, j] = newRows[i][j];

                        clashTestWorksheet.Range[$"A2:I{lastRow}"].Value2 = existingData;
                    }
                    catch { }
                }
                try 
                { 
                    doc.Delete(resolvedClashesSpheres);
                }
                catch {}
                tx.Commit();
                MessageBox.Show("Coordination Spheres Updated");
                
            }
            //}
            //catch (Exception ex)
            //{

            //}
            workbook.Save();
            workbook.Close();
            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
        }
        public string GetName()
        {
            return "Create Coordination Spheres Handler";
        }
    }
}
