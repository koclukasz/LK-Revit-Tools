using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using BIM.IFC.Export.UI;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace WSPPolska_Tools.Commands
{
    public partial class ExportLocationsForm : System.Windows.Forms.Form
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
        //IFCExportConfigurationsMap configMap;
        public ExportLocationsForm(ExternalCommandData commandData)
        {
            this.MouseDown += ExportLocationsForm_MouseDown;
            InitializeComponent();
            _commandData = commandData;
            uiapp = _commandData.Application;
            uidoc = uiapp.ActiveUIDocument;
            doc = uidoc.Document;
            userName = $"_{uiapp.Application.Username}";
            List<View3D> allViews = new FilteredElementCollector(doc).OfClass(typeof(View3D)).Cast<View3D>().Where(v => !v.IsTemplate).ToList();
            List<IntView3D> allViewsInt = allViews.Select(v => new IntView3D(v)).ToList();
            viewSelection.Items.AddRange(allViewsInt.ToArray());

            var ifcAppType = typeof(IFCCommandOverrideApplication);
            var theDocProp = ifcAppType.GetProperty("TheDocument");
            theDocProp.SetValue(null, doc);

            // Load built-in and saved configurations
            IFCExportConfigurationsMap configMap = new IFCExportConfigurationsMap();
            configMap.AddBuiltInConfigurations();
            configMap.AddSavedConfigurations();

            firstLocName = doc.ActiveProjectLocation.Name;
            fileName = doc.Title.Replace(userName, "");


            nameExample.Text = fileName;

            ifcExportOpt.Items.AddRange(configMap.Values.ToArray());

        }
        private void ExportLocationsForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void selectDirectory_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog1.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog1.SelectedPath))
                {
                    selectedPath = folderBrowserDialog1.SelectedPath;
                    MessageBox.Show($"Selected folder: {selectedPath}", "Folder Selected");
                }
            }

        }

        private void viewSelection_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ExportLocations_Click(object sender, EventArgs e)
        {
            if (selectedPath == null)
            {
                MessageBox.Show("Please select directory path", "Warning", MessageBoxButtons.OK);
                return;
            }
            if (ifcExportOpt.SelectedItem == null & exportIFC.Checked)
            {
                MessageBox.Show("Please select IFC Export Settings", "Warning", MessageBoxButtons.OK);
                return;
            }
            if (viewSelection.SelectedItem == null)
            {
                MessageBox.Show("Please select View", "Warning", MessageBoxButtons.OK);
                return;
            }
            if (namingSt.SelectedItem == null)
            {
                MessageBox.Show("Please select naming standard", "Warning", MessageBoxButtons.OK);
                return;
            }
            if (!exportIFC.Checked & !exportNWC.Checked & !exportDWFx.Checked)
            {
                MessageBox.Show("Please select at least one export type", "Warning", MessageBoxButtons.OK);
                return;
            }


            IFCExportOptions IFCExportOpt = new IFCExportOptions();
            IFCExportConfiguration IFCconfig = ifcExportOpt.SelectedItem as IFCExportConfiguration;
            IntView3D intView3D = viewSelection.SelectedItem as IntView3D;
            try
            {
                IFCconfig.UpdateOptions(IFCExportOpt, intView3D.viewInt.Id);
            }
            catch { }

            DWFXExportOptions optionsDWFx = new DWFXExportOptions();
            optionsDWFx.ExportObjectData = true;
            ViewSet viewSet = new ViewSet();
            viewSet.Insert(intView3D.viewInt);

            NavisworksExportOptions optionsNWC = new NavisworksExportOptions();
            optionsNWC.ViewId = intView3D.viewInt.Id;
            optionsNWC.ExportScope = NavisworksExportScope.View;
            optionsNWC.ExportLinks = exportLinks.Checked;
            optionsNWC.Coordinates = NavisworksCoordinates.Shared;
            optionsNWC.ExportParts = false;
            optionsNWC.ExportElementIds = true;
            optionsNWC.ConvertElementProperties = true;
            optionsNWC.ExportRoomAsAttribute = true;
            optionsNWC.ExportRoomGeometry = false;
            optionsNWC.ExportUrls = true;
            optionsNWC.DivideFileIntoLevels = false;

            Main.exportLocHandler.viewSet = viewSet;
            Main.exportLocHandler.exIFC = exportIFC.Checked;
            Main.exportLocHandler.exNWC = exportNWC.Checked;
            Main.exportLocHandler.exDWFx = exportDWFx.Checked;

            Main.exportLocHandler.nameSt = namingSt.SelectedIndex;
            Main.exportLocHandler.IFCExportOpt = IFCExportOpt;
            Main.exportLocHandler.DWFXExportOpt = optionsDWFx;
            Main.exportLocHandler.NWCExportOptions = optionsNWC;
            Main.exportLocHandler.selectedPath = selectedPath;
            Main.exportLocHandler.fileName = fileName;
            Main.exportLocEvent.Raise();

            
        }

        private void namingSt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (namingSt.SelectedIndex == 1)
            {
                nameExample.Text = $"{fileName}_{firstLocName}"; 
            }
            else if (namingSt.SelectedIndex == 0)
            {
                nameExample.Text = $"{firstLocName}_{fileName}";
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
