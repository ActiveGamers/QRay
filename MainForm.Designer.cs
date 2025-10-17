namespace QRay
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private TabControl tabControl;
        private TabPage tabScan;
        private TabPage tabGenerate;
        private TabPage tabHistory;
        private PictureBox picPreview;
        private Button btnBrowse;
        private Button btnDecode;
        private FlowLayoutPanel pnlResults;
        private TextBox txtPayload;
        private Button btnGenerate;
        private Button btnSaveQR;
        private PictureBox picQRPreview;
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;
        private FolderBrowserDialog folderBrowserDialog;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
        private ToolTip toolTip;
        private RadioButton radPNG;
        private RadioButton radJPG;
        private CheckBox chkTransparent;
        private NumericUpDown numSize;
        private ComboBox cmbErrorCorrection;
        private NumericUpDown numMargin;
        private Button btnColorForeground;
        private Button btnColorBackground;
        private ColorDialog colorDialog;
        private Button btnBatchScan;
        private ProgressBar progressBar;
        private Button btnCancel;
        private DataGridView dgvHistory;
        private Button btnClearHistory;
        private Button btnExportHistory;
        private TextBox txtSearchHistory;
        private Button btnOpenAppData;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            tabControl = new TabControl();
            tabScan = new TabPage();
            progressBar = new ProgressBar();
            btnCancel = new Button();
            pnlResults = new FlowLayoutPanel();
            btnDecode = new Button();
            picPreview = new PictureBox();
            btnBrowse = new Button();
            btnBatchScan = new Button();
            tabGenerate = new TabPage();
            btnColorBackground = new Button();
            btnColorForeground = new Button();
            numMargin = new NumericUpDown();
            cmbErrorCorrection = new ComboBox();
            numSize = new NumericUpDown();
            chkTransparent = new CheckBox();
            radJPG = new RadioButton();
            radPNG = new RadioButton();
            txtPayload = new TextBox();
            btnGenerate = new Button();
            btnSaveQR = new Button();
            picQRPreview = new PictureBox();
            tabHistory = new TabPage();
            btnOpenAppData = new Button();
            txtSearchHistory = new TextBox();
            btnExportHistory = new Button();
            btnClearHistory = new Button();
            dgvHistory = new DataGridView();
            tabAbout = new TabPage();
            linklblAboutUs = new LinkLabel();
            statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel();
            openFileDialog = new OpenFileDialog();
            saveFileDialog = new SaveFileDialog();
            folderBrowserDialog = new FolderBrowserDialog();
            toolTip = new ToolTip(components);
            colorDialog = new ColorDialog();
            tabControl.SuspendLayout();
            tabScan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picPreview).BeginInit();
            tabGenerate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numMargin).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numSize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picQRPreview).BeginInit();
            tabHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHistory).BeginInit();
            tabAbout.SuspendLayout();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabScan);
            tabControl.Controls.Add(tabGenerate);
            tabControl.Controls.Add(tabHistory);
            tabControl.Controls.Add(tabAbout);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(884, 561);
            tabControl.TabIndex = 0;
            tabControl.SelectedIndexChanged += tabControl_SelectedIndexChanged;
            // 
            // tabScan
            // 
            tabScan.Controls.Add(progressBar);
            tabScan.Controls.Add(btnCancel);
            tabScan.Controls.Add(pnlResults);
            tabScan.Controls.Add(btnDecode);
            tabScan.Controls.Add(picPreview);
            tabScan.Controls.Add(btnBrowse);
            tabScan.Controls.Add(btnBatchScan);
            tabScan.Location = new Point(4, 24);
            tabScan.Name = "tabScan";
            tabScan.Padding = new Padding(3);
            tabScan.Size = new Size(876, 533);
            tabScan.TabIndex = 0;
            tabScan.Text = "Scan QR Codes";
            tabScan.UseVisualStyleBackColor = true;
            // 
            // progressBar
            // 
            progressBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            progressBar.Location = new Point(8, 506);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(779, 23);
            progressBar.TabIndex = 6;
            progressBar.Visible = false;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Location = new Point(793, 506);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Visible = false;
            btnCancel.Click += BtnCancel_Click;
            // 
            // pnlResults
            // 
            pnlResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            pnlResults.AutoScroll = true;
            pnlResults.BorderStyle = BorderStyle.FixedSingle;
            pnlResults.FlowDirection = FlowDirection.TopDown;
            pnlResults.Location = new Point(546, 8);
            pnlResults.Name = "pnlResults";
            pnlResults.Size = new Size(324, 492);
            pnlResults.TabIndex = 5;
            pnlResults.WrapContents = false;
            // 
            // btnDecode
            // 
            btnDecode.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDecode.Location = new Point(367, 8);
            btnDecode.Name = "btnDecode";
            btnDecode.Size = new Size(173, 31);
            btnDecode.TabIndex = 4;
            btnDecode.Text = "Decode";
            toolTip.SetToolTip(btnDecode, "Decode QR codes in the selected image");
            btnDecode.UseVisualStyleBackColor = true;
            btnDecode.Click += BtnDecode_Click;
            // 
            // picPreview
            // 
            picPreview.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            picPreview.BorderStyle = BorderStyle.FixedSingle;
            picPreview.Location = new Point(8, 45);
            picPreview.Name = "picPreview";
            picPreview.Size = new Size(532, 455);
            picPreview.SizeMode = PictureBoxSizeMode.Zoom;
            picPreview.TabIndex = 3;
            picPreview.TabStop = false;
            // 
            // btnBrowse
            // 
            btnBrowse.Location = new Point(8, 8);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(170, 31);
            btnBrowse.TabIndex = 0;
            btnBrowse.Text = "Browse";
            toolTip.SetToolTip(btnBrowse, "Select an image file to scan for QR codes");
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += BtnBrowse_Click;
            // 
            // btnBatchScan
            // 
            btnBatchScan.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBatchScan.Location = new Point(184, 8);
            btnBatchScan.Name = "btnBatchScan";
            btnBatchScan.Size = new Size(177, 31);
            btnBatchScan.TabIndex = 2;
            btnBatchScan.Text = "Batch Scan";
            toolTip.SetToolTip(btnBatchScan, "Scan multiple images in a folder");
            btnBatchScan.UseVisualStyleBackColor = true;
            btnBatchScan.Click += BtnBatchScan_Click;
            // 
            // tabGenerate
            // 
            tabGenerate.Controls.Add(btnColorBackground);
            tabGenerate.Controls.Add(btnColorForeground);
            tabGenerate.Controls.Add(numMargin);
            tabGenerate.Controls.Add(cmbErrorCorrection);
            tabGenerate.Controls.Add(numSize);
            tabGenerate.Controls.Add(chkTransparent);
            tabGenerate.Controls.Add(radJPG);
            tabGenerate.Controls.Add(radPNG);
            tabGenerate.Controls.Add(txtPayload);
            tabGenerate.Controls.Add(btnGenerate);
            tabGenerate.Controls.Add(btnSaveQR);
            tabGenerate.Controls.Add(picQRPreview);
            tabGenerate.Location = new Point(4, 24);
            tabGenerate.Name = "tabGenerate";
            tabGenerate.Padding = new Padding(3);
            tabGenerate.Size = new Size(876, 533);
            tabGenerate.TabIndex = 1;
            tabGenerate.Text = "Generate QR Code";
            tabGenerate.UseVisualStyleBackColor = true;
            // 
            // btnColorBackground
            // 
            btnColorBackground.Location = new Point(546, 111);
            btnColorBackground.Name = "btnColorBackground";
            btnColorBackground.Size = new Size(94, 23);
            btnColorBackground.TabIndex = 14;
            btnColorBackground.Text = "Background";
            toolTip.SetToolTip(btnColorBackground, "Select background color");
            btnColorBackground.UseVisualStyleBackColor = true;
            btnColorBackground.Click += BtnColorBackground_Click;
            // 
            // btnColorForeground
            // 
            btnColorForeground.Location = new Point(546, 82);
            btnColorForeground.Name = "btnColorForeground";
            btnColorForeground.Size = new Size(94, 23);
            btnColorForeground.TabIndex = 13;
            btnColorForeground.Text = "Foreground";
            toolTip.SetToolTip(btnColorForeground, "Select foreground color");
            btnColorForeground.UseVisualStyleBackColor = true;
            btnColorForeground.Click += BtnColorForeground_Click;
            // 
            // numMargin
            // 
            numMargin.Location = new Point(546, 53);
            numMargin.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numMargin.Name = "numMargin";
            numMargin.Size = new Size(94, 23);
            numMargin.TabIndex = 12;
            toolTip.SetToolTip(numMargin, "Quiet zone margin");
            numMargin.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numMargin.ValueChanged += numMargin_ValueChanged;
            // 
            // cmbErrorCorrection
            // 
            cmbErrorCorrection.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbErrorCorrection.FormattingEnabled = true;
            cmbErrorCorrection.Items.AddRange(new object[] { "L (7%)", "M (15%)", "Q (25%)", "H (30%)" });
            cmbErrorCorrection.Location = new Point(546, 24);
            cmbErrorCorrection.Name = "cmbErrorCorrection";
            cmbErrorCorrection.Size = new Size(121, 23);
            cmbErrorCorrection.TabIndex = 11;
            toolTip.SetToolTip(cmbErrorCorrection, "Error correction level");
            cmbErrorCorrection.SelectedIndexChanged += cmbErrorCorrection_SelectedIndexChanged;
            // 
            // numSize
            // 
            numSize.Location = new Point(673, 24);
            numSize.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            numSize.Minimum = new decimal(new int[] { 50, 0, 0, 0 });
            numSize.Name = "numSize";
            numSize.Size = new Size(87, 23);
            numSize.TabIndex = 10;
            toolTip.SetToolTip(numSize, "QR code size (width = height)");
            numSize.Value = new decimal(new int[] { 512, 0, 0, 0 });
            numSize.ValueChanged += numSize_ValueChanged;
            // 
            // chkTransparent
            // 
            chkTransparent.AutoSize = true;
            chkTransparent.Location = new Point(673, 53);
            chkTransparent.Name = "chkTransparent";
            chkTransparent.Size = new Size(87, 19);
            chkTransparent.TabIndex = 9;
            chkTransparent.Text = "Transparent";
            toolTip.SetToolTip(chkTransparent, "Use transparent background (PNG only)");
            chkTransparent.UseVisualStyleBackColor = true;
            chkTransparent.CheckedChanged += ChkTransparent_CheckedChanged;
            // 
            // radJPG
            // 
            radJPG.AutoSize = true;
            radJPG.Location = new Point(739, 82);
            radJPG.Name = "radJPG";
            radJPG.Size = new Size(44, 19);
            radJPG.TabIndex = 8;
            radJPG.Text = "JPG";
            toolTip.SetToolTip(radJPG, "Save as JPEG format");
            radJPG.UseVisualStyleBackColor = true;
            radJPG.CheckedChanged += radJPG_CheckedChanged;
            // 
            // radPNG
            // 
            radPNG.AutoSize = true;
            radPNG.Checked = true;
            radPNG.Location = new Point(673, 82);
            radPNG.Name = "radPNG";
            radPNG.Size = new Size(49, 19);
            radPNG.TabIndex = 7;
            radPNG.TabStop = true;
            radPNG.Text = "PNG";
            toolTip.SetToolTip(radPNG, "Save as PNG format");
            radPNG.UseVisualStyleBackColor = true;
            // 
            // txtPayload
            // 
            txtPayload.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtPayload.Location = new Point(8, 24);
            txtPayload.Multiline = true;
            txtPayload.Name = "txtPayload";
            txtPayload.ScrollBars = ScrollBars.Vertical;
            txtPayload.Size = new Size(532, 110);
            txtPayload.TabIndex = 5;
            toolTip.SetToolTip(txtPayload, "Enter text or structured data for QR code");
            txtPayload.TextChanged += txtPayload_TextChanged;
            // 
            // btnGenerate
            // 
            btnGenerate.Location = new Point(8, 140);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(135, 23);
            btnGenerate.TabIndex = 4;
            btnGenerate.Text = "Generate";
            toolTip.SetToolTip(btnGenerate, "Generate QR code from input");
            btnGenerate.UseVisualStyleBackColor = true;
            btnGenerate.Click += BtnGenerate_Click;
            // 
            // btnSaveQR
            // 
            btnSaveQR.Location = new Point(149, 140);
            btnSaveQR.Name = "btnSaveQR";
            btnSaveQR.Size = new Size(135, 23);
            btnSaveQR.TabIndex = 3;
            btnSaveQR.Text = "Save";
            toolTip.SetToolTip(btnSaveQR, "Save QR code to file");
            btnSaveQR.UseVisualStyleBackColor = true;
            btnSaveQR.Click += BtnSaveQR_Click;
            // 
            // picQRPreview
            // 
            picQRPreview.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            picQRPreview.BorderStyle = BorderStyle.FixedSingle;
            picQRPreview.Location = new Point(8, 169);
            picQRPreview.Name = "picQRPreview";
            picQRPreview.Size = new Size(532, 356);
            picQRPreview.SizeMode = PictureBoxSizeMode.Zoom;
            picQRPreview.TabIndex = 2;
            picQRPreview.TabStop = false;
            // 
            // tabHistory
            // 
            tabHistory.Controls.Add(btnOpenAppData);
            tabHistory.Controls.Add(txtSearchHistory);
            tabHistory.Controls.Add(btnExportHistory);
            tabHistory.Controls.Add(btnClearHistory);
            tabHistory.Controls.Add(dgvHistory);
            tabHistory.Location = new Point(4, 24);
            tabHistory.Name = "tabHistory";
            tabHistory.Size = new Size(876, 533);
            tabHistory.TabIndex = 2;
            tabHistory.Text = "History";
            tabHistory.UseVisualStyleBackColor = true;
            // 
            // btnOpenAppData
            // 
            btnOpenAppData.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnOpenAppData.Location = new Point(770, 8);
            btnOpenAppData.Name = "btnOpenAppData";
            btnOpenAppData.Size = new Size(100, 23);
            btnOpenAppData.TabIndex = 4;
            btnOpenAppData.Text = "Open App Data";
            toolTip.SetToolTip(btnOpenAppData, "Open application data folder");
            btnOpenAppData.UseVisualStyleBackColor = true;
            btnOpenAppData.Click += BtnOpenAppData_Click;
            // 
            // txtSearchHistory
            // 
            txtSearchHistory.Location = new Point(8, 8);
            txtSearchHistory.Name = "txtSearchHistory";
            txtSearchHistory.PlaceholderText = "Search history...";
            txtSearchHistory.Size = new Size(200, 23);
            txtSearchHistory.TabIndex = 3;
            toolTip.SetToolTip(txtSearchHistory, "Search history entries");
            txtSearchHistory.TextChanged += TxtSearchHistory_TextChanged;
            // 
            // btnExportHistory
            // 
            btnExportHistory.Location = new Point(214, 8);
            btnExportHistory.Name = "btnExportHistory";
            btnExportHistory.Size = new Size(100, 23);
            btnExportHistory.TabIndex = 2;
            btnExportHistory.Text = "Export History";
            toolTip.SetToolTip(btnExportHistory, "Export history to CSV or JSON");
            btnExportHistory.UseVisualStyleBackColor = true;
            btnExportHistory.Click += BtnExportHistory_Click;
            // 
            // btnClearHistory
            // 
            btnClearHistory.Location = new Point(320, 8);
            btnClearHistory.Name = "btnClearHistory";
            btnClearHistory.Size = new Size(100, 23);
            btnClearHistory.TabIndex = 1;
            btnClearHistory.Text = "Clear History";
            toolTip.SetToolTip(btnClearHistory, "Clear scan history");
            btnClearHistory.UseVisualStyleBackColor = true;
            btnClearHistory.Click += BtnClearHistory_Click;
            // 
            // dgvHistory
            // 
            dgvHistory.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHistory.Location = new Point(8, 37);
            dgvHistory.Name = "dgvHistory";
            dgvHistory.ReadOnly = true;
            dgvHistory.Size = new Size(862, 492);
            dgvHistory.TabIndex = 0;
            dgvHistory.CellDoubleClick += DgvHistory_CellDoubleClick;
            // 
            // tabAbout
            // 
            tabAbout.Controls.Add(linklblAboutUs);
            tabAbout.Location = new Point(4, 24);
            tabAbout.Name = "tabAbout";
            tabAbout.Padding = new Padding(3);
            tabAbout.Size = new Size(876, 533);
            tabAbout.TabIndex = 3;
            tabAbout.Text = "About Us";
            tabAbout.UseVisualStyleBackColor = true;
            // 
            // linklblAboutUs
            // 
            linklblAboutUs.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            linklblAboutUs.LinkArea = new LinkArea(105, 28);
            linklblAboutUs.Location = new Point(3, 3);
            linklblAboutUs.Name = "linklblAboutUs";
            linklblAboutUs.Size = new Size(870, 527);
            linklblAboutUs.TabIndex = 1;
            linklblAboutUs.TabStop = true;
            linklblAboutUs.Text = "QRay - Version 1.0\r\nThis Application Is Made With ♥ By ActiveGamer In Iran For You!\r\n\r\nGit Hub Repository: \r\ngithub.com/ActiveGamers/QRay\r\n\r\nPlease report issues if you saw one of them!";
            linklblAboutUs.UseCompatibleTextRendering = true;
            linklblAboutUs.LinkClicked += linklblAboutUs_LinkClicked;
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new ToolStripItem[] { statusLabel });
            statusStrip.Location = new Point(0, 561);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(884, 22);
            statusStrip.TabIndex = 1;
            statusStrip.Text = "statusStrip";
            // 
            // statusLabel
            // 
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(869, 17);
            statusLabel.Spring = true;
            statusLabel.Text = "Ready";
            statusLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // openFileDialog
            // 
            openFileDialog.Filter = "Image files|*.png;*.jpg;*.jpeg;*.bmp;*.gif;*.webp|All files|*.*";
            openFileDialog.Title = "Select image to scan";
            // 
            // saveFileDialog
            // 
            saveFileDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg;*.jpeg";
            saveFileDialog.Title = "Save QR Code";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(884, 583);
            Controls.Add(tabControl);
            Controls.Add(statusStrip);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(900, 600);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "QRay - QR Code Scanner & Generator";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            tabControl.ResumeLayout(false);
            tabScan.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picPreview).EndInit();
            tabGenerate.ResumeLayout(false);
            tabGenerate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numMargin).EndInit();
            ((System.ComponentModel.ISupportInitialize)numSize).EndInit();
            ((System.ComponentModel.ISupportInitialize)picQRPreview).EndInit();
            tabHistory.ResumeLayout(false);
            tabHistory.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHistory).EndInit();
            tabAbout.ResumeLayout(false);
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
        private TabPage tabAbout;
        private LinkLabel linklblAboutUs;
    }
}