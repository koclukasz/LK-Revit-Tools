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
}
