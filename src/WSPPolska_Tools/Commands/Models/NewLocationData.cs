using Autodesk.Revit.DB;

namespace WSPPolska_Tools
{
    public class NewLocationData
    {
        public ElementId ElementId { get; set; }
        public string LocName { get; set; }
        public double EW { get; set; }
        public double NS { get; set; }
        public double EL { get; set; }
        public double Rot { get; set; }

        // Optional: Constructor to initialize values
        public NewLocationData(ElementId elementId, string locname, double ew, double ns, double el, double rot)
        {
            ElementId = elementId;
            LocName = locname;
            EW = ew;
            NS = ns;
            EL = el;
            Rot = rot;
        }
    }
}
