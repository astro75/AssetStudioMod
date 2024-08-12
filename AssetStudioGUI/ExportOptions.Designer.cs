namespace AssetStudioGUI
{
    partial class ExportOptions
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.OKbutton = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.filenameFormatLabel = new System.Windows.Forms.Label();
            this.filenameFormatComboBox = new System.Windows.Forms.ComboBox();
            this.exportSpriteWithAlphaMask = new System.Windows.Forms.CheckBox();
            this.openAfterExport = new System.Windows.Forms.CheckBox();
            this.restoreExtensionName = new System.Windows.Forms.CheckBox();
            this.assetGroupOptions = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.convertAudio = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.towebp = new System.Windows.Forms.RadioButton();
            this.totga = new System.Windows.Forms.RadioButton();
            this.tojpg = new System.Windows.Forms.RadioButton();
            this.topng = new System.Windows.Forms.RadioButton();
            this.tobmp = new System.Windows.Forms.RadioButton();
            this.converttexture = new System.Windows.Forms.CheckBox();
            this.l2dGroupBox = new System.Windows.Forms.GroupBox();
            this.l2dMotionExportMethodPanel = new System.Windows.Forms.Panel();
            this.l2dMonoBehaviourRadioButton = new System.Windows.Forms.RadioButton();
            this.l2dAnimationClipRadioButton = new System.Windows.Forms.RadioButton();
            this.l2dMotionExportMethodLabel = new System.Windows.Forms.Label();
            this.l2dForceBezierCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.exportAllUvsAsDiffuseMaps = new System.Windows.Forms.CheckBox();
            this.exportBlendShape = new System.Windows.Forms.CheckBox();
            this.exportAnimations = new System.Windows.Forms.CheckBox();
            this.scaleFactor = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.fbxFormat = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.fbxVersion = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.boneSize = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.exportSkins = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.filterPrecision = new System.Windows.Forms.NumericUpDown();
            this.castToBone = new System.Windows.Forms.CheckBox();
            this.exportAllNodes = new System.Windows.Forms.CheckBox();
            this.eulerFilter = new System.Windows.Forms.CheckBox();
            this.optionTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.parallelExportUpDown = new System.Windows.Forms.NumericUpDown();
            this.parallelExportCheckBox = new System.Windows.Forms.CheckBox();
            this.parallelExportMaxLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.l2dGroupBox.SuspendLayout();
            this.l2dMotionExportMethodPanel.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scaleFactor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.boneSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filterPrecision)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.parallelExportUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // OKbutton
            // 
            this.OKbutton.Location = new System.Drawing.Point(381, 380);
            this.OKbutton.Name = "OKbutton";
            this.OKbutton.Size = new System.Drawing.Size(75, 23);
            this.OKbutton.TabIndex = 4;
            this.OKbutton.Text = "OK";
            this.OKbutton.UseVisualStyleBackColor = true;
            this.OKbutton.Click += new System.EventHandler(this.OKbutton_Click);
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(462, 380);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 5;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.parallelExportMaxLabel);
            this.groupBox1.Controls.Add(this.parallelExportCheckBox);
            this.groupBox1.Controls.Add(this.parallelExportUpDown);
            this.groupBox1.Controls.Add(this.filenameFormatLabel);
            this.groupBox1.Controls.Add(this.filenameFormatComboBox);
            this.groupBox1.Controls.Add(this.exportSpriteWithAlphaMask);
            this.groupBox1.Controls.Add(this.openAfterExport);
            this.groupBox1.Controls.Add(this.restoreExtensionName);
            this.groupBox1.Controls.Add(this.assetGroupOptions);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.convertAudio);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.converttexture);
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(301, 272);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Export";
            // 
            // filenameFormatLabel
            // 
            this.filenameFormatLabel.AutoSize = true;
            this.filenameFormatLabel.Location = new System.Drawing.Point(177, 18);
            this.filenameFormatLabel.Name = "filenameFormatLabel";
            this.filenameFormatLabel.Size = new System.Drawing.Size(84, 13);
            this.filenameFormatLabel.TabIndex = 10;
            this.filenameFormatLabel.Text = "File name format";
            // 
            // filenameFormatComboBox
            // 
            this.filenameFormatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filenameFormatComboBox.FormattingEnabled = true;
            this.filenameFormatComboBox.Items.AddRange(new object[] {
            "assetName",
            "assetName@pathID",
            "pathID"});
            this.filenameFormatComboBox.Location = new System.Drawing.Point(177, 35);
            this.filenameFormatComboBox.Name = "filenameFormatComboBox";
            this.filenameFormatComboBox.Size = new System.Drawing.Size(118, 21);
            this.filenameFormatComboBox.TabIndex = 9;
            // 
            // exportSpriteWithAlphaMask
            // 
            this.exportSpriteWithAlphaMask.AutoSize = true;
            this.exportSpriteWithAlphaMask.Checked = true;
            this.exportSpriteWithAlphaMask.CheckState = System.Windows.Forms.CheckState.Checked;
            this.exportSpriteWithAlphaMask.Location = new System.Drawing.Point(6, 150);
            this.exportSpriteWithAlphaMask.Name = "exportSpriteWithAlphaMask";
            this.exportSpriteWithAlphaMask.Size = new System.Drawing.Size(205, 17);
            this.exportSpriteWithAlphaMask.TabIndex = 6;
            this.exportSpriteWithAlphaMask.Text = "Export sprites with alpha mask applied";
            this.exportSpriteWithAlphaMask.UseVisualStyleBackColor = true;
            // 
            // openAfterExport
            // 
            this.openAfterExport.AutoSize = true;
            this.openAfterExport.Checked = true;
            this.openAfterExport.CheckState = System.Windows.Forms.CheckState.Checked;
            this.openAfterExport.Location = new System.Drawing.Point(6, 196);
            this.openAfterExport.Name = "openAfterExport";
            this.openAfterExport.Size = new System.Drawing.Size(137, 17);
            this.openAfterExport.TabIndex = 8;
            this.openAfterExport.Text = "Open folder after export";
            this.openAfterExport.UseVisualStyleBackColor = true;
            // 
            // restoreExtensionName
            // 
            this.restoreExtensionName.AutoSize = true;
            this.restoreExtensionName.Checked = true;
            this.restoreExtensionName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.restoreExtensionName.Location = new System.Drawing.Point(6, 63);
            this.restoreExtensionName.Name = "restoreExtensionName";
            this.restoreExtensionName.Size = new System.Drawing.Size(275, 17);
            this.restoreExtensionName.TabIndex = 3;
            this.restoreExtensionName.Text = "Try to restore/Use original TextAsset extension name";
            this.optionTooltip.SetToolTip(this.restoreExtensionName, "If not checked, AssetStudio will export all TextAssets with the \".txt\" extension");
            this.restoreExtensionName.UseVisualStyleBackColor = true;
            // 
            // assetGroupOptions
            // 
            this.assetGroupOptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.assetGroupOptions.FormattingEnabled = true;
            this.assetGroupOptions.Items.AddRange(new object[] {
            "type name",
            "container path",
            "container path full (with name)",
            "source file name",
            "scene hierarchy",
            "do not group"});
            this.assetGroupOptions.Location = new System.Drawing.Point(6, 35);
            this.assetGroupOptions.Name = "assetGroupOptions";
            this.assetGroupOptions.Size = new System.Drawing.Size(165, 21);
            this.assetGroupOptions.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(127, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Group exported assets by";
            // 
            // convertAudio
            // 
            this.convertAudio.AutoSize = true;
            this.convertAudio.Checked = true;
            this.convertAudio.CheckState = System.Windows.Forms.CheckState.Checked;
            this.convertAudio.Location = new System.Drawing.Point(6, 173);
            this.convertAudio.Name = "convertAudio";
            this.convertAudio.Size = new System.Drawing.Size(213, 17);
            this.convertAudio.TabIndex = 7;
            this.convertAudio.Text = "Convert FMOD AudioClip to WAV(PCM)";
            this.convertAudio.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.towebp);
            this.panel1.Controls.Add(this.totga);
            this.panel1.Controls.Add(this.tojpg);
            this.panel1.Controls.Add(this.topng);
            this.panel1.Controls.Add(this.tobmp);
            this.panel1.Location = new System.Drawing.Point(18, 111);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(260, 33);
            this.panel1.TabIndex = 5;
            // 
            // towebp
            // 
            this.towebp.AutoSize = true;
            this.towebp.Location = new System.Drawing.Point(201, 7);
            this.towebp.Name = "towebp";
            this.towebp.Size = new System.Drawing.Size(54, 17);
            this.towebp.TabIndex = 4;
            this.towebp.Text = "Webp";
            this.towebp.UseVisualStyleBackColor = true;
            // 
            // totga
            // 
            this.totga.AutoSize = true;
            this.totga.Location = new System.Drawing.Point(150, 7);
            this.totga.Name = "totga";
            this.totga.Size = new System.Drawing.Size(44, 17);
            this.totga.TabIndex = 3;
            this.totga.Text = "Tga";
            this.totga.UseVisualStyleBackColor = true;
            // 
            // tojpg
            // 
            this.tojpg.AutoSize = true;
            this.tojpg.Location = new System.Drawing.Point(97, 7);
            this.tojpg.Name = "tojpg";
            this.tojpg.Size = new System.Drawing.Size(48, 17);
            this.tojpg.TabIndex = 2;
            this.tojpg.Text = "Jpeg";
            this.tojpg.UseVisualStyleBackColor = true;
            // 
            // topng
            // 
            this.topng.AutoSize = true;
            this.topng.Checked = true;
            this.topng.Location = new System.Drawing.Point(50, 7);
            this.topng.Name = "topng";
            this.topng.Size = new System.Drawing.Size(44, 17);
            this.topng.TabIndex = 1;
            this.topng.TabStop = true;
            this.topng.Text = "Png";
            this.topng.UseVisualStyleBackColor = true;
            // 
            // tobmp
            // 
            this.tobmp.AutoSize = true;
            this.tobmp.Location = new System.Drawing.Point(3, 7);
            this.tobmp.Name = "tobmp";
            this.tobmp.Size = new System.Drawing.Size(46, 17);
            this.tobmp.TabIndex = 0;
            this.tobmp.Text = "Bmp";
            this.tobmp.UseVisualStyleBackColor = true;
            // 
            // converttexture
            // 
            this.converttexture.AutoSize = true;
            this.converttexture.Checked = true;
            this.converttexture.CheckState = System.Windows.Forms.CheckState.Checked;
            this.converttexture.Location = new System.Drawing.Point(6, 87);
            this.converttexture.Name = "converttexture";
            this.converttexture.Size = new System.Drawing.Size(116, 17);
            this.converttexture.TabIndex = 4;
            this.converttexture.Text = "Convert Texture2D";
            this.converttexture.UseVisualStyleBackColor = true;
            // 
            // l2dGroupBox
            // 
            this.l2dGroupBox.Controls.Add(this.l2dMotionExportMethodPanel);
            this.l2dGroupBox.Controls.Add(this.l2dMotionExportMethodLabel);
            this.l2dGroupBox.Controls.Add(this.l2dForceBezierCheckBox);
            this.l2dGroupBox.Location = new System.Drawing.Point(12, 275);
            this.l2dGroupBox.Name = "l2dGroupBox";
            this.l2dGroupBox.Size = new System.Drawing.Size(301, 100);
            this.l2dGroupBox.TabIndex = 2;
            this.l2dGroupBox.TabStop = false;
            this.l2dGroupBox.Text = "Cubism Live2D";
            // 
            // l2dMotionExportMethodPanel
            // 
            this.l2dMotionExportMethodPanel.Controls.Add(this.l2dMonoBehaviourRadioButton);
            this.l2dMotionExportMethodPanel.Controls.Add(this.l2dAnimationClipRadioButton);
            this.l2dMotionExportMethodPanel.Location = new System.Drawing.Point(18, 40);
            this.l2dMotionExportMethodPanel.Name = "l2dMotionExportMethodPanel";
            this.l2dMotionExportMethodPanel.Size = new System.Drawing.Size(263, 27);
            this.l2dMotionExportMethodPanel.TabIndex = 2;
            // 
            // l2dMonoBehaviourRadioButton
            // 
            this.l2dMonoBehaviourRadioButton.AccessibleName = "MonoBehaviour";
            this.l2dMonoBehaviourRadioButton.AutoSize = true;
            this.l2dMonoBehaviourRadioButton.Checked = true;
            this.l2dMonoBehaviourRadioButton.Location = new System.Drawing.Point(3, 5);
            this.l2dMonoBehaviourRadioButton.Name = "l2dMonoBehaviourRadioButton";
            this.l2dMonoBehaviourRadioButton.Size = new System.Drawing.Size(167, 17);
            this.l2dMonoBehaviourRadioButton.TabIndex = 0;
            this.l2dMonoBehaviourRadioButton.TabStop = true;
            this.l2dMonoBehaviourRadioButton.Text = "MonoBehaviour (Fade motion)";
            this.optionTooltip.SetToolTip(this.l2dMonoBehaviourRadioButton, "If no Fade motions are found, the AnimationClip method will be used");
            this.l2dMonoBehaviourRadioButton.UseVisualStyleBackColor = true;
            // 
            // l2dAnimationClipRadioButton
            // 
            this.l2dAnimationClipRadioButton.AccessibleName = "AnimationClipV2";
            this.l2dAnimationClipRadioButton.AutoSize = true;
            this.l2dAnimationClipRadioButton.Location = new System.Drawing.Point(172, 5);
            this.l2dAnimationClipRadioButton.Name = "l2dAnimationClipRadioButton";
            this.l2dAnimationClipRadioButton.Size = new System.Drawing.Size(88, 17);
            this.l2dAnimationClipRadioButton.TabIndex = 1;
            this.l2dAnimationClipRadioButton.Text = "AnimationClip";
            this.l2dAnimationClipRadioButton.UseVisualStyleBackColor = true;
            // 
            // l2dMotionExportMethodLabel
            // 
            this.l2dMotionExportMethodLabel.AutoSize = true;
            this.l2dMotionExportMethodLabel.Location = new System.Drawing.Point(6, 21);
            this.l2dMotionExportMethodLabel.Name = "l2dMotionExportMethodLabel";
            this.l2dMotionExportMethodLabel.Size = new System.Drawing.Size(109, 13);
            this.l2dMotionExportMethodLabel.TabIndex = 1;
            this.l2dMotionExportMethodLabel.Text = "Motion export method";
            // 
            // l2dForceBezierCheckBox
            // 
            this.l2dForceBezierCheckBox.AutoSize = true;
            this.l2dForceBezierCheckBox.Location = new System.Drawing.Point(6, 77);
            this.l2dForceBezierCheckBox.Name = "l2dForceBezierCheckBox";
            this.l2dForceBezierCheckBox.Size = new System.Drawing.Size(278, 17);
            this.l2dForceBezierCheckBox.TabIndex = 3;
            this.l2dForceBezierCheckBox.Text = "Calculate Linear motion segments as Bezier segments";
            this.optionTooltip.SetToolTip(this.l2dForceBezierCheckBox, "May help if the exported motions look jerky/not smooth enough");
            this.l2dForceBezierCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.exportAllUvsAsDiffuseMaps);
            this.groupBox2.Controls.Add(this.exportBlendShape);
            this.groupBox2.Controls.Add(this.exportAnimations);
            this.groupBox2.Controls.Add(this.scaleFactor);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.fbxFormat);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.fbxVersion);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.boneSize);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.exportSkins);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.filterPrecision);
            this.groupBox2.Controls.Add(this.castToBone);
            this.groupBox2.Controls.Add(this.exportAllNodes);
            this.groupBox2.Controls.Add(this.eulerFilter);
            this.groupBox2.Location = new System.Drawing.Point(313, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(224, 362);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Fbx";
            // 
            // exportAllUvsAsDiffuseMaps
            // 
            this.exportAllUvsAsDiffuseMaps.AccessibleDescription = "";
            this.exportAllUvsAsDiffuseMaps.AutoSize = true;
            this.exportAllUvsAsDiffuseMaps.Location = new System.Drawing.Point(6, 185);
            this.exportAllUvsAsDiffuseMaps.Name = "exportAllUvsAsDiffuseMaps";
            this.exportAllUvsAsDiffuseMaps.Size = new System.Drawing.Size(168, 17);
            this.exportAllUvsAsDiffuseMaps.TabIndex = 9;
            this.exportAllUvsAsDiffuseMaps.Text = "Export all UVs as diffuse maps";
            this.optionTooltip.SetToolTip(this.exportAllUvsAsDiffuseMaps, "Unchecked: UV1 exported as normal map. Check this if your export is missing a UV " +
        "map.");
            this.exportAllUvsAsDiffuseMaps.UseVisualStyleBackColor = true;
            // 
            // exportBlendShape
            // 
            this.exportBlendShape.AutoSize = true;
            this.exportBlendShape.Checked = true;
            this.exportBlendShape.CheckState = System.Windows.Forms.CheckState.Checked;
            this.exportBlendShape.Location = new System.Drawing.Point(6, 138);
            this.exportBlendShape.Name = "exportBlendShape";
            this.exportBlendShape.Size = new System.Drawing.Size(114, 17);
            this.exportBlendShape.TabIndex = 7;
            this.exportBlendShape.Text = "Export blendshape";
            this.exportBlendShape.UseVisualStyleBackColor = true;
            // 
            // exportAnimations
            // 
            this.exportAnimations.AutoSize = true;
            this.exportAnimations.Checked = true;
            this.exportAnimations.CheckState = System.Windows.Forms.CheckState.Checked;
            this.exportAnimations.Location = new System.Drawing.Point(6, 114);
            this.exportAnimations.Name = "exportAnimations";
            this.exportAnimations.Size = new System.Drawing.Size(109, 17);
            this.exportAnimations.TabIndex = 6;
            this.exportAnimations.Text = "Export animations";
            this.exportAnimations.UseVisualStyleBackColor = true;
            // 
            // scaleFactor
            // 
            this.scaleFactor.DecimalPlaces = 2;
            this.scaleFactor.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.scaleFactor.Location = new System.Drawing.Point(83, 243);
            this.scaleFactor.Name = "scaleFactor";
            this.scaleFactor.Size = new System.Drawing.Size(60, 20);
            this.scaleFactor.TabIndex = 13;
            this.scaleFactor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.scaleFactor.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 245);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "ScaleFactor";
            // 
            // fbxFormat
            // 
            this.fbxFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fbxFormat.FormattingEnabled = true;
            this.fbxFormat.Items.AddRange(new object[] {
            "Binary",
            "Ascii"});
            this.fbxFormat.Location = new System.Drawing.Point(77, 275);
            this.fbxFormat.Name = "fbxFormat";
            this.fbxFormat.Size = new System.Drawing.Size(61, 21);
            this.fbxFormat.TabIndex = 15;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 280);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "FBXFormat";
            // 
            // fbxVersion
            // 
            this.fbxVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fbxVersion.FormattingEnabled = true;
            this.fbxVersion.Items.AddRange(new object[] {
            "6.1",
            "7.1",
            "7.2",
            "7.3",
            "7.4",
            "7.5"});
            this.fbxVersion.Location = new System.Drawing.Point(77, 308);
            this.fbxVersion.Name = "fbxVersion";
            this.fbxVersion.Size = new System.Drawing.Size(47, 21);
            this.fbxVersion.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 311);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "FBXVersion";
            // 
            // boneSize
            // 
            this.boneSize.Location = new System.Drawing.Point(65, 213);
            this.boneSize.Name = "boneSize";
            this.boneSize.Size = new System.Drawing.Size(46, 20);
            this.boneSize.TabIndex = 11;
            this.boneSize.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 216);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "BoneSize";
            // 
            // exportSkins
            // 
            this.exportSkins.AutoSize = true;
            this.exportSkins.Checked = true;
            this.exportSkins.CheckState = System.Windows.Forms.CheckState.Checked;
            this.exportSkins.Location = new System.Drawing.Point(6, 90);
            this.exportSkins.Name = "exportSkins";
            this.exportSkins.Size = new System.Drawing.Size(83, 17);
            this.exportSkins.TabIndex = 5;
            this.exportSkins.Text = "Export skins";
            this.exportSkins.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "FilterPrecision";
            // 
            // filterPrecision
            // 
            this.filterPrecision.DecimalPlaces = 2;
            this.filterPrecision.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.filterPrecision.Location = new System.Drawing.Point(127, 40);
            this.filterPrecision.Name = "filterPrecision";
            this.filterPrecision.Size = new System.Drawing.Size(51, 20);
            this.filterPrecision.TabIndex = 3;
            this.filterPrecision.Value = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            // 
            // castToBone
            // 
            this.castToBone.AutoSize = true;
            this.castToBone.Location = new System.Drawing.Point(6, 161);
            this.castToBone.Name = "castToBone";
            this.castToBone.Size = new System.Drawing.Size(131, 17);
            this.castToBone.TabIndex = 8;
            this.castToBone.Text = "All nodes cast to bone";
            this.castToBone.UseVisualStyleBackColor = true;
            // 
            // exportAllNodes
            // 
            this.exportAllNodes.AutoSize = true;
            this.exportAllNodes.Checked = true;
            this.exportAllNodes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.exportAllNodes.Location = new System.Drawing.Point(6, 66);
            this.exportAllNodes.Name = "exportAllNodes";
            this.exportAllNodes.Size = new System.Drawing.Size(101, 17);
            this.exportAllNodes.TabIndex = 4;
            this.exportAllNodes.Text = "Export all nodes";
            this.exportAllNodes.UseVisualStyleBackColor = true;
            // 
            // eulerFilter
            // 
            this.eulerFilter.AutoSize = true;
            this.eulerFilter.Checked = true;
            this.eulerFilter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.eulerFilter.Location = new System.Drawing.Point(6, 22);
            this.eulerFilter.Name = "eulerFilter";
            this.eulerFilter.Size = new System.Drawing.Size(72, 17);
            this.eulerFilter.TabIndex = 1;
            this.eulerFilter.Text = "EulerFilter";
            this.eulerFilter.UseVisualStyleBackColor = true;
            // 
            // parallelExportUpDown
            // 
            this.parallelExportUpDown.Location = new System.Drawing.Point(209, 218);
            this.parallelExportUpDown.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.parallelExportUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.parallelExportUpDown.Name = "parallelExportUpDown";
            this.parallelExportUpDown.Size = new System.Drawing.Size(42, 20);
            this.parallelExportUpDown.TabIndex = 13;
            this.parallelExportUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // parallelExportCheckBox
            // 
            this.parallelExportCheckBox.AutoSize = true;
            this.parallelExportCheckBox.Checked = true;
            this.parallelExportCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.parallelExportCheckBox.Location = new System.Drawing.Point(6, 219);
            this.parallelExportCheckBox.Name = "parallelExportCheckBox";
            this.parallelExportCheckBox.Size = new System.Drawing.Size(203, 17);
            this.parallelExportCheckBox.TabIndex = 15;
            this.parallelExportCheckBox.Text = "Export in parallel with number of tasks";
            this.optionTooltip.SetToolTip(this.parallelExportCheckBox, "*Requires slightly more RAM than in single-task mode");
            this.parallelExportCheckBox.UseVisualStyleBackColor = true;
            this.parallelExportCheckBox.CheckedChanged += new System.EventHandler(this.parallelExportCheckBox_CheckedChanged);
            // 
            // parallelExportMaxLabel
            // 
            this.parallelExportMaxLabel.AutoSize = true;
            this.parallelExportMaxLabel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.parallelExportMaxLabel.Location = new System.Drawing.Point(256, 221);
            this.parallelExportMaxLabel.Name = "parallelExportMaxLabel";
            this.parallelExportMaxLabel.Size = new System.Drawing.Size(33, 13);
            this.parallelExportMaxLabel.TabIndex = 16;
            this.parallelExportMaxLabel.Text = "Max: ";
            this.optionTooltip.SetToolTip(this.parallelExportMaxLabel, "*The maximum number matches the number of CPU cores");
            // 
            // ExportOptions
            // 
            this.AcceptButton = this.OKbutton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(549, 416);
            this.Controls.Add(this.l2dGroupBox);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OKbutton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportOptions";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export options";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.l2dGroupBox.ResumeLayout(false);
            this.l2dGroupBox.PerformLayout();
            this.l2dMotionExportMethodPanel.ResumeLayout(false);
            this.l2dMotionExportMethodPanel.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scaleFactor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boneSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filterPrecision)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.parallelExportUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button OKbutton;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox converttexture;
        private System.Windows.Forms.RadioButton tojpg;
        private System.Windows.Forms.RadioButton topng;
        private System.Windows.Forms.RadioButton tobmp;
        private System.Windows.Forms.RadioButton totga;
        private System.Windows.Forms.CheckBox convertAudio;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown boneSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox exportSkins;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown filterPrecision;
        private System.Windows.Forms.CheckBox castToBone;
        private System.Windows.Forms.CheckBox exportAllNodes;
        private System.Windows.Forms.CheckBox eulerFilter;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox fbxVersion;
        private System.Windows.Forms.ComboBox fbxFormat;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown scaleFactor;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox exportBlendShape;
        private System.Windows.Forms.CheckBox exportAnimations;
        private System.Windows.Forms.ComboBox assetGroupOptions;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox restoreExtensionName;
        private System.Windows.Forms.CheckBox openAfterExport;
        private System.Windows.Forms.CheckBox exportAllUvsAsDiffuseMaps;
        private System.Windows.Forms.ToolTip optionTooltip;
        private System.Windows.Forms.CheckBox exportSpriteWithAlphaMask;
        private System.Windows.Forms.RadioButton towebp;
        private System.Windows.Forms.GroupBox l2dGroupBox;
        private System.Windows.Forms.CheckBox l2dForceBezierCheckBox;
        private System.Windows.Forms.Label l2dMotionExportMethodLabel;
        private System.Windows.Forms.RadioButton l2dAnimationClipRadioButton;
        private System.Windows.Forms.RadioButton l2dMonoBehaviourRadioButton;
        private System.Windows.Forms.Panel l2dMotionExportMethodPanel;
        private System.Windows.Forms.ComboBox filenameFormatComboBox;
        private System.Windows.Forms.Label filenameFormatLabel;
        private System.Windows.Forms.NumericUpDown parallelExportUpDown;
        private System.Windows.Forms.CheckBox parallelExportCheckBox;
        private System.Windows.Forms.Label parallelExportMaxLabel;
    }
}