namespace WSPPolska_Tools
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Autodesk.Revit.UI;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Mechanical;
    using System.Diagnostics;
    using System;
    using Excel = Microsoft.Office.Interop.Excel;

    /// <summary>
    /// Command to Be executed when button clicked
    /// </summary>
    /// <seealso cref="Autodesk.Revit.UI.IExternalCommand"></seealso>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]


    public class CommonM
    {
        public static string GetSpaceForElement(Element element)
        {
            FamilyInstance familyElement = element as FamilyInstance;
            try
            {
                string spaceNumber = familyElement.Space.Number;
                return spaceNumber;
            }
            catch
            {
                return "-";
            }
        }

        public static Level SearchForLevel(Dictionary<Level, double> levelDictionary, double elevation)
        {
            int levelInd = 0;
            foreach (KeyValuePair<Level, double> entry in levelDictionary)
            {
                if (elevation > entry.Value)
                    levelInd = levelInd + 1;
                else
                    break;
            }
            if (levelInd > 0)
                levelInd = levelInd - 1;
            return levelDictionary.Keys.ToList()[levelInd];
        }


        public static StringBuilder SB(List<Element> element)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Element w in element)
                sb.Append(w.Name + " " + w.Id + "\n");
            return sb;
        }

        public static StringBuilder SBLev(Dictionary<Level, double> elems)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<Level, double> w in elems)
                sb.Append(w.Key.Name + " " + w.Value + "\n");
            return sb;
        }
        public static StringBuilder SBStr(List<string> errorsStr)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string error in errorsStr)
                sb.Append(error);
            return sb;
        }
    }
    public class IntView3D
    {
        public View3D viewInt { get; private set; }

        public IntView3D(View3D view)
        {
            viewInt = view;
        }
        public override string ToString()
        {
            return viewInt.Name;
        }
    }
    public static class ExcelHelper
    {
        /// <summary>
        /// Gets the last non-empty row in the specified Excel worksheet.
        /// </summary>
        /// <param name="sheet">The Excel worksheet to inspect.</param>
        /// <returns>The row number of the last non-empty row, or 0 if none found.</returns>
        public static int GetLastNonEmptyRow(Excel.Worksheet sheet)
        {
            if (sheet == null)
                throw new ArgumentNullException(nameof(sheet));

            int lastRow = sheet.UsedRange.Rows.Count;

            for (int row = lastRow; row >= 1; row--)
            {
                Excel.Range cell = sheet.Cells[row, 2] as Excel.Range; // Column B is index 2

                if (cell != null && cell.Value2 != null && !string.IsNullOrWhiteSpace(cell.Value2.ToString()))
                {
                    return row;
                }
            }

            return 0;

        }

    }
}
