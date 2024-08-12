using AssetStudio;
using System;
using System.Linq;
using System.Windows.Forms;

namespace AssetStudioGUI
{
    public partial class ExportOptions : Form
    {
        public ExportOptions()
        {
            InitializeComponent();
            assetGroupOptions.SelectedIndex = Properties.Settings.Default.assetGroupOption;
            restoreExtensionName.Checked = Properties.Settings.Default.restoreExtensionName;
            converttexture.Checked = Properties.Settings.Default.convertTexture;
            exportSpriteWithAlphaMask.Checked = Properties.Settings.Default.exportSpriteWithMask;
            convertAudio.Checked = Properties.Settings.Default.convertAudio;
            var defaultImageType = Properties.Settings.Default.convertType.ToString();
            ((RadioButton)panel1.Controls.Cast<Control>().First(x => x.Text == defaultImageType)).Checked = true;
            openAfterExport.Checked = Properties.Settings.Default.openAfterExport;
            eulerFilter.Checked = Properties.Settings.Default.eulerFilter;
            filterPrecision.Value = Properties.Settings.Default.filterPrecision;
            exportAllNodes.Checked = Properties.Settings.Default.exportAllNodes;
            exportSkins.Checked = Properties.Settings.Default.exportSkins;
            exportAnimations.Checked = Properties.Settings.Default.exportAnimations;
            exportBlendShape.Checked = Properties.Settings.Default.exportBlendShape;
            castToBone.Checked = Properties.Settings.Default.castToBone;
            exportAllUvsAsDiffuseMaps.Checked = Properties.Settings.Default.exportAllUvsAsDiffuseMaps;
            boneSize.Value = Properties.Settings.Default.boneSize;
            scaleFactor.Value = Properties.Settings.Default.scaleFactor;
            fbxVersion.SelectedIndex = Properties.Settings.Default.fbxVersion;
            fbxFormat.SelectedIndex = Properties.Settings.Default.fbxFormat;
            var defaultMotionMode = Properties.Settings.Default.l2dMotionMode.ToString();
            ((RadioButton)l2dMotionExportMethodPanel.Controls.Cast<Control>().First(x => x.AccessibleName == defaultMotionMode)).Checked = true;
            l2dForceBezierCheckBox.Checked = Properties.Settings.Default.l2dForceBezier;
            filenameFormatComboBox.SelectedIndex = Properties.Settings.Default.filenameFormat;
            var maxParallelTasks = Environment.ProcessorCount;
            var taskCount = Properties.Settings.Default.parallelExportCount;
            parallelExportUpDown.Maximum = maxParallelTasks;
            parallelExportUpDown.Value = taskCount <= 0 ? maxParallelTasks : Math.Min(taskCount, maxParallelTasks);
            parallelExportMaxLabel.Text += maxParallelTasks;
            parallelExportCheckBox.Checked = Properties.Settings.Default.parallelExport;
        }

        private void OKbutton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.assetGroupOption = assetGroupOptions.SelectedIndex;
            Properties.Settings.Default.restoreExtensionName = restoreExtensionName.Checked;
            Properties.Settings.Default.convertTexture = converttexture.Checked;
            Properties.Settings.Default.exportSpriteWithMask = exportSpriteWithAlphaMask.Checked;
            Properties.Settings.Default.convertAudio = convertAudio.Checked;
            var checkedImageType = (RadioButton)panel1.Controls.Cast<Control>().First(x => ((RadioButton)x).Checked);
            Properties.Settings.Default.convertType = (ImageFormat)Enum.Parse(typeof(ImageFormat), checkedImageType.Text);
            Properties.Settings.Default.openAfterExport = openAfterExport.Checked;
            Properties.Settings.Default.eulerFilter = eulerFilter.Checked;
            Properties.Settings.Default.filterPrecision = filterPrecision.Value;
            Properties.Settings.Default.exportAllNodes = exportAllNodes.Checked;
            Properties.Settings.Default.exportSkins = exportSkins.Checked;
            Properties.Settings.Default.exportAnimations = exportAnimations.Checked;
            Properties.Settings.Default.exportBlendShape = exportBlendShape.Checked;
            Properties.Settings.Default.castToBone = castToBone.Checked;
            Properties.Settings.Default.exportAllUvsAsDiffuseMaps = exportAllUvsAsDiffuseMaps.Checked;
            Properties.Settings.Default.boneSize = boneSize.Value;
            Properties.Settings.Default.scaleFactor = scaleFactor.Value;
            Properties.Settings.Default.fbxVersion = fbxVersion.SelectedIndex;
            Properties.Settings.Default.fbxFormat = fbxFormat.SelectedIndex;
            var checkedMotionMode = (RadioButton)l2dMotionExportMethodPanel.Controls.Cast<Control>().First(x => ((RadioButton)x).Checked);
            Properties.Settings.Default.l2dMotionMode = (CubismLive2DExtractor.Live2DMotionMode)Enum.Parse(typeof(CubismLive2DExtractor.Live2DMotionMode), checkedMotionMode.AccessibleName);
            Properties.Settings.Default.l2dForceBezier = l2dForceBezierCheckBox.Checked;
            Properties.Settings.Default.filenameFormat = filenameFormatComboBox.SelectedIndex;
            Properties.Settings.Default.parallelExport = parallelExportCheckBox.Checked;
            Properties.Settings.Default.parallelExportCount = (int)parallelExportUpDown.Value;
            Properties.Settings.Default.Save();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void parallelExportCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            parallelExportUpDown.Enabled = parallelExportCheckBox.Checked;
        }
    }
}
