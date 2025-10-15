using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSPPolska_Tools
{
    internal class ChangeNameToStandard
    {
        public string Name { get; set; }
        public string AdditionalPart { get; set; }
        public int ChangeType { get; set; }

        public ChangeNameToStandard(string name,string additionalPart, int changeType)
        {
            Name = name;
            ChangeType = changeType;
            AdditionalPart = additionalPart;
        }
        public string GetModifiedName()

        {
            switch (ChangeType)
            {
                case 0:
                    return $"{AdditionalPart}_{Name}";
                case 1:
                    return $"{Name}_{AdditionalPart}";
                default:
                    return Name; // fallback if ChangeType is not 0 or 1
            }
        }


    }
}
