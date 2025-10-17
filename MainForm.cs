using System.Data;
using System.Linq;
using Microsoft.Data.Sqlite;
using OpenCvSharp;
using QRCoder;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using Timer = System.Windows.Forms.Timer;

namespace QRay
{
    public partial class MainForm : Form
    {
        #region Fields and Properties
        private Logger logger;
        private SettingsManager settings;
        private HistoryRepository historyRepo;
        private Decoder decoder;
        private Generator generator;
        private ImageUtils imageUtils;
        private WorkerManager workerManager;

        private string currentImagePath;
        private Bitmap currentQRCode;
        private List<ScanResult> currentResults;
        private CancellationTokenSource cancellationTokenSource;

        private Color foregroundColor = Color.Black;
        private Color backgroundColor = Color.White;
        #endregion

        #region Initialization
        public MainForm()
        {
            InitializeComponent();
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            logger = new Logger();
            settings = new SettingsManager();
            historyRepo = new HistoryRepository();
            decoder = new Decoder();
            generator = new Generator();
            imageUtils = new ImageUtils();
            workerManager = new WorkerManager();

            cmbErrorCorrection.SelectedIndex = 3;
            UpdateStatus("Ready - App Data: %localappdata%\\ActiveGamers\\QRay\\");

            logger.Info("App Opened");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadHistory();
        }
        #endregion

        #region Event Handlers - Scan Tab
        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    LoadImageForScanning(openFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to open file", ex);
            }
        }

        private void PnlDragDrop_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void PnlDragDrop_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0 && IsSupportedImageFormat(files[0]))
                {
                    LoadImageForScanning(files[0]);
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to load dragged file", ex);
            }
        }

        private async void BtnDecode_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentImagePath))
            {
                MessageBox.Show("Please select an image first.", "No Image",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            await DecodeImageAsync(currentImagePath);
        }

        private void BtnBatchScan_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                StartBatchScan(folderBrowserDialog.SelectedPath);
            }
        }
        #endregion

        #region Event Handlers - Generate Tab
        private void numMargin_ValueChanged(object sender, EventArgs e)
        {
            SetSaveButtonActive();
        }

        private void txtPayload_TextChanged(object sender, EventArgs e)
        {
            SetSaveButtonActive();
        }

        private void cmbErrorCorrection_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSaveButtonActive();
        }

        private void numSize_ValueChanged(object sender, EventArgs e)
        {
            SetSaveButtonActive();
        }

        private void radJPG_CheckedChanged(object sender, EventArgs e)
        {
            SetSaveButtonActive();
            chkTransparent.Enabled = !radJPG.Checked;
            if (radJPG.Checked)
            {
                chkTransparent.Checked = false;
            }
        }

        private async void BtnGenerate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPayload.Text.Trim()))
            {
                MessageBox.Show("Please enter content for the QR code.", "No Content",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SetSaveButtonActive(true);
            await GenerateQRCodeAsync();
        }

        private void BtnSaveQR_Click(object sender, EventArgs e)
        {
            if (currentQRCode == null)
            {
                MessageBox.Show("Please generate a QR code first.", "No QR Code",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveQRCode();
        }

        private void BtnColorForeground_Click(object sender, EventArgs e)
        {
            SetSaveButtonActive();
            colorDialog.Color = foregroundColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                foregroundColor = colorDialog.Color;
                btnColorForeground.BackColor = foregroundColor;
            }
        }

        private void BtnColorBackground_Click(object sender, EventArgs e)
        {
            SetSaveButtonActive();
            if (chkTransparent.Checked)
            {
                MessageBox.Show("Background color is disabled when transparent background is selected.",
                    "Transparent Background", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            colorDialog.Color = backgroundColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                backgroundColor = colorDialog.Color;
                btnColorBackground.BackColor = backgroundColor;
            }
        }

        private void ChkTransparent_CheckedChanged(object sender, EventArgs e)
        {
            SetSaveButtonActive();
            btnColorBackground.Enabled = !chkTransparent.Checked;
        }
        #endregion

        #region Event Handlers - History Tab
        private void TxtSearchHistory_TextChanged(object sender, EventArgs e)
        {
            FilterHistory(txtSearchHistory.Text);
        }

        private void BtnExportHistory_Click(object sender, EventArgs e)
        {
            ExportHistory();
        }

        private void BtnClearHistory_Click(object sender, EventArgs e)
        {
            ClearHistory();
        }

        private void DgvHistory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ShowHistoryItemDetails(e.RowIndex);
            }
        }

        private void BtnOpenAppData_Click(object sender, EventArgs e)
        {
            OpenAppDataFolder();
        }
        #endregion

        #region Event Handlers - Common
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            cancellationTokenSource?.Cancel();
            UpdateProgressUI(false);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            cancellationTokenSource?.Cancel();
            logger.Info("Closing Application (User closed app)");
            logger.WriteEOF();
        }
        #endregion

        #region Scan Methods
        private void LoadImageForScanning(string filePath)
        {
            try
            {
                currentImagePath = filePath;
                picPreview.Image = Image.FromFile(filePath);
                pnlResults.Controls.Clear();
                currentResults = null;
                UpdateStatus($"Loaded: {Path.GetFileName(filePath)}");
            }
            catch (Exception ex)
            {
                ShowError($"Failed to load image '{Path.GetFileName(filePath)}'", ex);
            }
        }

        private async Task DecodeImageAsync(string imagePath)
        {
            try
            {
                UpdateStatus("Decoding QR codes...");
                btnDecode.Enabled = false;

                var results = await Task.Run(() => decoder.DecodeImage(imagePath));

                if (results != null && results.Count > 0)
                {
                    currentResults = results;
                    DisplayScanResults(results);
                    SaveToHistory(imagePath, results);
                    UpdateStatus($"Found {results.Count} QR code(s) in {Path.GetFileName(imagePath)}");
                    logger.Info($"Scanned image '{Path.GetFileName(imagePath)}' - found {results.Count} QR codes");
                }
                else
                {
                    MessageBox.Show("No QR codes found in the image.", "No Results",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateStatus("No QR codes found");
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to decode image", ex);
            }
            finally
            {
                btnDecode.Enabled = true;
            }
        }

        private void DisplayScanResults(List<ScanResult> results)
        {
            pnlResults.Controls.Clear();

            foreach (var result in results)
            {
                var resultPanel = CreateResultPanel(result);
                pnlResults.Controls.Add(resultPanel);
            }

            DrawBoundingBoxes(results);
        }

        private Panel CreateResultPanel(ScanResult result)
        {
            var panel = new Panel
            {
                Width = pnlResults.Width - 25,
                Height = 120,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(5)
            };

            var lblType = new Label { Text = result.Type.ToString(), Location = new Point(5, 5), AutoSize = true };
            var lblContent = new Label
            {
                Text = result.Payload,
                Location = new Point(5, 25),
                MaximumSize = new Size(panel.Width - 10, 40),
                AutoSize = true
            };

            var btnCopy = new Button { Text = "Copy", Location = new Point(5, 70), Size = new Size(60, 25) };
            btnCopy.Click += (s, e) => CopyToClipboard(result.Payload);

            panel.Controls.AddRange(new Control[] { lblType, lblContent, btnCopy });

            switch (result.Type)
            {
                case PayloadType.Url:
                    var btnOpen = new Button { Text = "Open", Location = new Point(70, 70), Size = new Size(60, 25) };
                    btnOpen.Click += (s, e) => OpenUrl(result.Payload);
                    panel.Controls.Add(btnOpen);
                    break;
                case PayloadType.WiFi:
                    var btnGenerate = new Button { Text = "Generate QR", Location = new Point(70, 70), Size = new Size(80, 25) };
                    btnGenerate.Click += (s, e) => PrepopulateWiFiGenerator(result.Payload);
                    panel.Controls.Add(btnGenerate);
                    break;
                case PayloadType.vCard:
                    var btnExport = new Button { Text = "Export vCard", Location = new Point(70, 70), Size = new Size(80, 25) };
                    btnExport.Click += (s, e) => ExportVCard(result.Payload);
                    panel.Controls.Add(btnExport);
                    break;
            }

            return panel;
        }

        private void DrawBoundingBoxes(List<ScanResult> results)
        {
            try
            {
                if (picPreview.Image == null || results == null || results.Count == 0)
                    return;

                var originalImage = new Bitmap(picPreview.Image);
                using (var graphics = Graphics.FromImage(originalImage))
                {
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    var colors = new Color[]
                    {
                Color.Red, Color.Blue, Color.Green, Color.Orange,
                Color.Purple, Color.Teal, Color.Magenta, Color.DarkGreen
                    };

                    var pens = colors.Select(c => new Pen(c, 3)).ToArray();
                    var brushes = colors.Select(c => new SolidBrush(Color.FromArgb(50, c))).ToArray();

                    for (int i = 0; i < results.Count; i++)
                    {
                        var result = results[i];
                        var colorIndex = i % colors.Length;

                        var bbox = result.BoundingBox;

                        graphics.FillRectangle(brushes[colorIndex], bbox);

                        graphics.DrawRectangle(pens[colorIndex], bbox);

                        var font = new Font("Arial", 12, FontStyle.Bold);
                        var text = $"{i + 1}";
                        var textSize = graphics.MeasureString(text, font);
                        var textPoint = new PointF(bbox.X + 5, bbox.Y + 5);

                        graphics.FillRectangle(Brushes.White,
                            textPoint.X - 2, textPoint.Y - 2,
                            textSize.Width + 4, textSize.Height + 4);

                        graphics.DrawString(text, font, Brushes.Black, textPoint);

                        var typeText = result.Type.ToString();
                        var typeSize = graphics.MeasureString(typeText, font);
                        var typePoint = new PointF(bbox.X + bbox.Width - typeSize.Width - 5, bbox.Y + 5);

                        graphics.FillRectangle(Brushes.White,
                            typePoint.X - 2, typePoint.Y - 2,
                            typeSize.Width + 4, typeSize.Height + 4);

                        graphics.DrawString(typeText, font, Brushes.Black, typePoint);
                    }

                    foreach (var pen in pens) pen.Dispose();
                    foreach (var brush in brushes) brush.Dispose();
                }

                picPreview.Image = originalImage;

                picPreview.MouseClick += (s, e) => SelectResultByClick(e.Location, results);
            }
            catch (Exception ex)
            {
                logger.Error($"Failed to draw bounding boxes: {ex.Message}");
            }
        }

        private void SelectResultByClick(Point clickPoint, List<ScanResult> results)
        {
            try
            {
                if (picPreview.Image == null) return;

                var imageSize = picPreview.Image.Size;
                var displaySize = picPreview.ClientSize;

                float scaleX = (float)imageSize.Width / displaySize.Width;
                float scaleY = (float)imageSize.Height / displaySize.Height;

                var imagePoint = new Point(
                    (int)(clickPoint.X * scaleX),
                    (int)(clickPoint.Y * scaleY)
                );

                for (int i = 0; i < results.Count; i++)
                {
                    if (results[i].BoundingBox.Contains(imagePoint))
                    {

                        if (pnlResults.Controls.Count > i)
                        {
                            var resultPanel = pnlResults.Controls[i];
                            resultPanel.BackColor = Color.LightBlue;

                            pnlResults.ScrollControlIntoView(resultPanel);

                            var timer = new Timer { Interval = 2000 };
                            timer.Tick += (s, e) =>
                            {
                                resultPanel.BackColor = SystemColors.Control;
                                timer.Stop();
                                timer.Dispose();
                            };
                            timer.Start();

                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Failed to select result by click: {ex.Message}");
            }
        }
        #endregion

        #region Generate Methods
        private async Task GenerateQRCodeAsync()
        {
            try
            {
                UpdateStatus("Generating QR code...");
                btnGenerate.Enabled = false;

                var payload = txtPayload.Text.Trim();
                var size = (int)numSize.Value;
                var errorCorrection = GetErrorCorrectionLevel();
                var margin = (int)numMargin.Value;

                var qrCode = await Task.Run(() => generator.GenerateQRCode(
                    payload, foregroundColor, backgroundColor, size, errorCorrection, margin));

                if (qrCode != null)
                {
                    currentQRCode = qrCode;
                    picQRPreview.Image = qrCode;
                    UpdateStatus("QR code generated successfully");
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to generate QR code", ex);
            }
            finally
            {
                btnGenerate.Enabled = true;
            }
        }

        private void SaveQRCode()
        {
            try
            {
                saveFileDialog.FilterIndex = radPNG.Checked ? 1 : 2;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var format = radPNG.Checked ? System.Drawing.Imaging.ImageFormat.Png :
                                                 System.Drawing.Imaging.ImageFormat.Jpeg;

                    currentQRCode.Save(saveFileDialog.FileName, format);
                    UpdateStatus($"QR code saved to {Path.GetFileName(saveFileDialog.FileName)}");

                    if (MessageBox.Show("QR code saved successfully. Open containing folder?",
                        "Save Complete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        OpenContainingFolder(saveFileDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to save QR code", ex);
            }
        }

        private QRCodeGenerator.ECCLevel GetErrorCorrectionLevel()
        {
            return cmbErrorCorrection.SelectedIndex switch
            {
                0 => QRCodeGenerator.ECCLevel.L,
                1 => QRCodeGenerator.ECCLevel.M,
                2 => QRCodeGenerator.ECCLevel.Q,
                3 => QRCodeGenerator.ECCLevel.H,
                _ => QRCodeGenerator.ECCLevel.M
            };
        }
        #endregion

        #region Batch Operations
        private async void StartBatchScan(string folderPath)
        {
            try
            {
                var imageFiles = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
                    .Where(f => IsSupportedImageFormat(f)).ToArray();

                if (imageFiles.Length == 0)
                {
                    MessageBox.Show("No supported image files found in the selected folder.",
                        "No Images", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                UpdateProgressUI(true);
                progressBar.Maximum = imageFiles.Length;

                cancellationTokenSource = new CancellationTokenSource();

                await ProcessBatchScanAsync(imageFiles, cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                ShowError("Batch scan failed", ex);
            }
            finally
            {
                UpdateProgressUI(false);
            }
        }

        private async Task ProcessBatchScanAsync(string[] imageFiles, CancellationToken cancellationToken)
        {
            int processed = 0;
            int totalFound = 0;
            List<ScanResult> scanResults = new();
            foreach (var file in imageFiles)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                try
                {
                    var results = await Task.Run(() => decoder.DecodeImage(file));
                    scanResults.AddRange(results);
                    if (results != null)
                    {
                        totalFound += results.Count;
                        SaveToHistory(file, results);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error($"Failed to process '{Path.GetFileName(file)}': {ex.Message}");
                }

                processed++;
                UpdateProgress(processed, imageFiles.Length, $"Processed {processed}/{imageFiles.Length} - Found {totalFound} QR codes");
            }

            DisplayScanResults(scanResults);

            MessageBox.Show($"Batch scan complete.\nProcessed: {processed} files\nQR codes found: {totalFound}\nCheck history tab for more information",
                "Batch Scan Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #region History Methods
        private void LoadHistory()
        {
            try
            {
                var history = historyRepo.GetAll();
                dgvHistory.DataSource = history;
                UpdateStatus($"History loaded: {history.Count} entries");
            }
            catch (Exception ex)
            {
                ShowError("Failed to load history", ex);
            }
        }

        private void FilterHistory(string searchText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    LoadHistory();
                    return;
                }

                var allHistory = historyRepo.GetAll();

                var filteredHistory = allHistory.Where(entry =>
                    entry.Filename?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    entry.Payload?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    entry.Type.ToString().IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    entry.Timestamp.ToString("yyyy-MM-dd HH:mm:ss").IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0
                ).ToList();

                dgvHistory.DataSource = filteredHistory;
                UpdateStatus($"Found {filteredHistory.Count} history entries matching '{searchText}'");
            }
            catch (Exception ex)
            {
                ShowError("Failed to filter history", ex);
            }
        }

        private void ExportHistory()
        {
            try
            {
                using (SaveFileDialog dlg = new SaveFileDialog())
                {
                    dlg.Filter = "CSV Files|*.csv|JSON Files|*.json";
                    dlg.Title = "Export History";

                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        historyRepo.Export(dlg.FileName);
                        UpdateStatus($"History exported to {Path.GetFileName(dlg.FileName)}");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to export history", ex);
            }
        }

        private void ClearHistory()
        {
            if (MessageBox.Show("Are you sure you want to clear all history? This action cannot be undone.",
                "Confirm Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                historyRepo.Clear();
                LoadHistory();
                UpdateStatus("History cleared");
            }
        }

        private void ShowHistoryItemDetails(int rowIndex)
        {
            try
            {
                if (dgvHistory.DataSource is List<HistoryEntry> history && rowIndex < history.Count)
                {
                    var entry = history[rowIndex];

                    using (var detailForm = new Form())
                    {
                        detailForm.Text = $"QR Code Details - {entry.Filename}";
                        detailForm.Size = new Size(500, 400);
                        detailForm.StartPosition = FormStartPosition.CenterParent;
                        detailForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                        detailForm.MaximizeBox = false;
                        detailForm.MinimizeBox = false;

                        var tabControl = new TabControl { Dock = DockStyle.Fill };


                        var tabBasic = new TabPage("Basic Information");
                        var basicPanel = new Panel { Dock = DockStyle.Fill, AutoScroll = true };

                        var controls = new List<Control>
                {
                    CreateDetailLabel("Timestamp:", entry.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"), 10, 10),
                    CreateDetailLabel("Filename:", entry.Filename, 10, 40),
                    CreateDetailLabel("Type:", entry.Type.ToString(), 10, 70),
                    CreateDetailLabel("Bounding Box:", entry.BoundingBox ?? "N/A", 10, 100)
                };

                        var tabPayload = new TabPage("Payload");
                        var payloadTextBox = new TextBox
                        {
                            Multiline = true,
                            ReadOnly = true,
                            ScrollBars = ScrollBars.Vertical,
                            Dock = DockStyle.Fill,
                            Text = entry.Payload
                        };

                        var payloadPanel = new Panel { Dock = DockStyle.Fill };
                        payloadPanel.Controls.Add(payloadTextBox);

                        var btnCopy = new Button { Text = "Copy Payload", Location = new Point(10, 130), Size = new Size(100, 30) };
                        btnCopy.Click += (s, e) => CopyToClipboard(entry.Payload);

                        var btnOpenImage = new Button { Text = "Open Image", Location = new Point(120, 130), Size = new Size(100, 30) };
                        btnOpenImage.Click += (s, e) => OpenImageFile(entry.ImagePath);

                        var btnRegenerate = new Button { Text = "Generate QR", Location = new Point(230, 130), Size = new Size(100, 30) };
                        btnRegenerate.Click += (s, e) =>
                        {
                            PrepopulateGeneratorFromHistory(entry);
                            detailForm.Close();
                        };

                        controls.AddRange([btnCopy, btnOpenImage, btnRegenerate]);

                        foreach (var control in controls)
                            basicPanel.Controls.Add(control);

                        tabBasic.Controls.Add(basicPanel);
                        tabPayload.Controls.Add(payloadPanel);

                        tabControl.TabPages.AddRange(new TabPage[] { tabBasic, tabPayload });
                        detailForm.Controls.Add(tabControl);

                        detailForm.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to show history details", ex);
            }
        }

        private void PrepopulateGeneratorFromHistory(HistoryEntry entry)
        {
            tabControl.SelectedTab = tabGenerate;
            txtPayload.Text = entry.Payload;
        }

        private Label CreateDetailLabel(string labelText, string value, int x, int y)
        {
            return new Label
            {
                Text = $"{labelText} {value}",
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Regular)
            };
        }

        private void OpenImageFile(string imagePath)
        {
            try
            {
                if (File.Exists(imagePath))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = imagePath,
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show("Original image file not found.", "File Not Found",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to open image file", ex);
            }
        }

        private void SaveToHistory(string imagePath, List<ScanResult> results)
        {
            try
            {
                foreach (var result in results)
                {
                    historyRepo.Add(new HistoryEntry
                    {
                        Timestamp = DateTime.Now,
                        Filename = Path.GetFileName(imagePath),
                        Payload = result.Payload,
                        Type = result.Type,
                        BoundingBox = $"{result.BoundingBox.X},{result.BoundingBox.Y},{result.BoundingBox.Width},{result.BoundingBox.Height}",
                        ImagePath = imagePath
                    });
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Failed to save to history: {ex.Message}");
            }
        }
        #endregion

        #region UI Helpers
        private void UpdateStatus(string message)
        {

            statusLabel.Text = message;

        }

        private void UpdateProgress(int value, int maximum, string status)
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new Action<int, int, string>(UpdateProgress), value, maximum, status);
            }
            else
            {
                progressBar.Value = value;
                progressBar.Maximum = maximum;
                UpdateStatus(status);
            }
        }

        private void UpdateProgressUI(bool show)
        {
            progressBar.Visible = show;
            btnCancel.Visible = show;
            progressBar.Value = 0;
        }

        private void ShowError(string message, Exception ex)
        {
            logger.Error($"{message}: {ex.Message}");
            MessageBox.Show($"{message}: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion

        #region Action Methods
        private void SetSaveButtonActive(bool Active = false)
        {
            if (Active)
            {
                btnSaveQR.Text = "Save";
                btnSaveQR.Enabled = true;
                return;
            }
            btnSaveQR.Text = "Generate First!";
            btnSaveQR.Enabled = false;
        }

        private void CopyToClipboard(string text)
        {
            try
            {
                Clipboard.SetText(text);
                UpdateStatus("Copied to clipboard");
            }
            catch (Exception ex)
            {
                ShowError("Failed to copy to clipboard", ex);
            }
        }

        private void OpenUrl(string url)
        {
            try
            {
                var uri = new Uri(url);
                var domain = uri.Host;

                if (MessageBox.Show($"Open URL: {domain}?\n\n{url}", "Confirm Open",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to open URL", ex);
            }
        }

        private void PrepopulateWiFiGenerator(string wifiPayload)
        {
            tabControl.SelectedTab = tabGenerate;
            txtPayload.Text = wifiPayload;
        }

        private void ExportVCard(string vcardData)
        {
            try
            {
                using (SaveFileDialog dlg = new SaveFileDialog())
                {
                    dlg.Filter = "vCard Files|*.vcf";
                    dlg.DefaultExt = "vcf";

                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllText(dlg.FileName, vcardData);
                        UpdateStatus("vCard exported successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to export vCard", ex);
            }
        }

        private void OpenContainingFolder(string filePath)
        {
            try
            {
                System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{filePath}\"");
            }
            catch (Exception ex)
            {
                ShowError("Failed to open folder", ex);
            }
        }

        private void OpenAppDataFolder()
        {
            try
            {
                var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var qrayPath = Path.Combine(appData, "ActiveGamers", "QRay");

                if (!Directory.Exists(qrayPath))
                    Directory.CreateDirectory(qrayPath);

                System.Diagnostics.Process.Start("explorer.exe", qrayPath);
            }
            catch (Exception ex)
            {
                ShowError("Failed to open app data folder", ex);
            }
        }

        private bool IsSupportedImageFormat(string filePath)
        {
            var ext = Path.GetExtension(filePath).ToLower();
            return ext == ".png" || ext == ".jpg" || ext == ".jpeg" ||
                   ext == ".bmp" || ext == ".gif" || ext == ".webp";
        }
        #endregion

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == 2)
            {
                LoadHistory();
            }
        }

        private void linklblAboutUs_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenGitHub();
        }

        private void OpenGitHub()
        {
            try
            {
                string githubUrl = "https://github.com/ActiveGamers/QRay";

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = githubUrl,
                    UseShellExecute = true
                });

                logger.Info($"Opened GitHub repository: {githubUrl}");
                UpdateStatus("Opened GitHub repository in browser");
            }
            catch (Exception ex)
            {
                ShowError("Failed to open GitHub repository", ex);
            }
        }
    }

    #region Helper Classes
    public enum PayloadType { Text, Url, WiFi, vCard, Email, SMS, Phone, Geo, Unknown }

    public class ScanResult
    {
        public string Payload { get; set; }
        public PayloadType Type { get; set; }
        public Rectangle BoundingBox { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class HistoryEntry
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Filename { get; set; }
        public string Payload { get; set; }
        public PayloadType Type { get; set; }
        public string BoundingBox { get; set; }
        public string ImagePath { get; set; }
    }

    public class Logger
    {
        private readonly string logPath;
        private readonly object lockObject = new object();

        public Logger()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var logDir = Path.Combine(appData, "ActiveGamers", "QRay", "Logs");
            Directory.CreateDirectory(logDir);

            logPath = Path.Combine(logDir, $"{DateTime.Now:yyyyMMdd-HHmmss}_qray.log");
        }

        public void Info(string message)
        {
            WriteLog("Info", message);
        }

        public void Warn(string message)
        {
            WriteLog("Warn", message);
        }

        public void Error(string message)
        {
            WriteLog("Error", message);
        }

        private void WriteLog(string level, string message)
        {
            lock (lockObject)
            {
                File.AppendAllText(logPath, $"{DateTime.Now:yyyy/MM/dd-HH:mm:ss} [{level}] {message}\n");
            }
        }

        public void WriteEOF()
        {
            lock (lockObject)
            {
                File.AppendAllText(logPath, "EOF\n");
            }
        }
    }

    public class SettingsManager
    {
        public SettingsManager()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var configDir = Path.Combine(appData, "ActiveGamers", "QRay", "Config");
            Directory.CreateDirectory(configDir);
        }
    }

    public class HistoryRepository
    {
        private readonly string dbPath;

        public HistoryRepository()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dbDir = Path.Combine(appData, "ActiveGamers", "QRay", "DB");
            Directory.CreateDirectory(dbDir);

            dbPath = Path.Combine(dbDir, "history.db");
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS History (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Timestamp TEXT NOT NULL,
                        Filename TEXT NOT NULL,
                        Payload TEXT NOT NULL,
                        Type INTEGER NOT NULL,
                        BoundingBox TEXT,
                        ImagePath TEXT
                    )";
                command.ExecuteNonQuery();
            }
        }

        public List<HistoryEntry> GetAll()
        {
            var entries = new List<HistoryEntry>();

            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM History ORDER BY Timestamp DESC";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        entries.Add(new HistoryEntry
                        {
                            Id = reader.GetInt32(0),
                            Timestamp = DateTime.Parse(reader.GetString(1)),
                            Filename = reader.GetString(2),
                            Payload = reader.GetString(3),
                            Type = (PayloadType)reader.GetInt32(4),
                            BoundingBox = reader.IsDBNull(5) ? null : reader.GetString(5),
                            ImagePath = reader.IsDBNull(6) ? null : reader.GetString(6)
                        });
                    }
                }
            }

            return entries;
        }

        public void Add(HistoryEntry entry)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO History (Timestamp, Filename, Payload, Type, BoundingBox, ImagePath)
                    VALUES (@timestamp, @filename, @payload, @type, @bbox, @imagePath)";

                command.Parameters.AddWithValue("@timestamp", entry.Timestamp.ToString("O"));
                command.Parameters.AddWithValue("@filename", entry.Filename);
                command.Parameters.AddWithValue("@payload", entry.Payload);
                command.Parameters.AddWithValue("@type", (int)entry.Type);
                command.Parameters.AddWithValue("@bbox", (object)entry.BoundingBox ?? DBNull.Value);
                command.Parameters.AddWithValue("@imagePath", (object)entry.ImagePath ?? DBNull.Value);

                command.ExecuteNonQuery();
            }
        }

        public void Clear()
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "DROP TABLE History";
                command.ExecuteNonQuery();
            }
            InitializeDatabase();
        }

        public void Export(string exportPath)
        {
            var entries = GetAll();

            if (Path.GetExtension(exportPath).ToLower() == ".csv")
            {
                using (var writer = new StreamWriter(exportPath))
                {
                    writer.WriteLine("Timestamp,Filename,Type,Payload");
                    foreach (var entry in entries)
                    {
                        writer.WriteLine($"\"{entry.Timestamp:O}\",\"{entry.Filename}\",\"{entry.Type}\",\"{entry.Payload}\"");
                    }
                }
            }
            else
            {
                var json = System.Text.Json.JsonSerializer.Serialize(entries, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(exportPath, json);
            }
        }
    }

    public class Decoder
    {
        public List<ScanResult> DecodeImage(string imagePath)
        {
            var results = new List<ScanResult>();

            try
            {
                using (var mat = Cv2.ImRead(imagePath))
                {
                    if (mat.Empty())
                    {
                        throw new Exception("Could not load image");
                    }

                    results.AddRange(DetectQRWithStandardMethod(mat));

                    if (results.Count == 0)
                    {
                        results.AddRange(DetectQRWithAlternativeMethods(mat));
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Decode failed for {Path.GetFileName(imagePath)}", ex);
            }

            return results;
        }

        private List<ScanResult> DetectQRWithStandardMethod(Mat mat)
        {
            var results = new List<ScanResult>();

            try
            {
                using (var qrDecoder = new QRCodeDetector())
                {
                    Point2f[] points;
                    string decodedText;

                    decodedText = qrDecoder.DetectAndDecode(mat, out points);

                    if (!string.IsNullOrEmpty(decodedText) && points != null && points.Length == 4)
                    {
                        var result = new ScanResult
                        {
                            Payload = decodedText,
                            Type = DetectPayloadType(decodedText),
                            Timestamp = DateTime.Now,
                            BoundingBox = CalculateBoundingBox(points)
                        };
                        results.Add(result);
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return results;
        }

        private List<ScanResult> DetectQRWithAlternativeMethods(Mat mat)
        {
            var results = new List<ScanResult>();

            try
            {
                var preprocessingMethods = new[]
                {
            new { Name = "Original", Action = new Action<Mat>(m => { }) },
            new { Name = "Grayscale", Action = new Action<Mat>(m => Cv2.CvtColor(m, m, ColorConversionCodes.BGR2GRAY)) },
            new { Name = "Grayscale+Threshold", Action = new Action<Mat>(m =>
            {
                Cv2.CvtColor(m, m, ColorConversionCodes.BGR2GRAY);
                Cv2.Threshold(m, m, 128, 255, ThresholdTypes.Binary);
            })},
            new { Name = "Grayscale+AdaptiveThreshold", Action = new Action<Mat>(m =>
            {
                Cv2.CvtColor(m, m, ColorConversionCodes.BGR2GRAY);
                Cv2.AdaptiveThreshold(m, m, 255, AdaptiveThresholdTypes.GaussianC,
                    ThresholdTypes.Binary, 11, 2);
            })}
        };

                var scales = new[] { 0.5f, 0.75f, 1.0f, 1.25f, 1.5f, 2.0f };

                foreach (var method in preprocessingMethods)
                {
                    if (results.Count > 0) break;

                    foreach (var scale in scales)
                    {
                        using (var processedMat = mat.Clone())
                        using (var resizedMat = new Mat())
                        {
                            try
                            {
                                if (Math.Abs(scale - 1.0) > 0.1)
                                {
                                    var newWidth = (int)(processedMat.Width * scale);
                                    var newHeight = (int)(processedMat.Height * scale);
                                    Cv2.Resize(processedMat, resizedMat, new OpenCvSharp.Size(newWidth, newHeight));
                                }
                                else
                                {
                                    processedMat.CopyTo(resizedMat);
                                }

                                method.Action(resizedMat);

                                using (var qrDecoder = new QRCodeDetector())
                                {
                                    Point2f[] points;
                                    string decodedText = qrDecoder.DetectAndDecode(resizedMat, out points);

                                    if (!string.IsNullOrEmpty(decodedText) && points != null && points.Length == 4)
                                    {
                                        var originalPoints = points.Select(p =>
                                            new Point2f(p.X / scale, p.Y / scale)).ToArray();

                                        var result = new ScanResult
                                        {
                                            Payload = decodedText,
                                            Type = DetectPayloadType(decodedText),
                                            Timestamp = DateTime.Now,
                                            BoundingBox = CalculateBoundingBox(originalPoints)
                                        };

                                        if (!results.Any(r => r.Payload == result.Payload &&
                                                             r.BoundingBox == result.BoundingBox))
                                        {
                                            results.Add(result);
                                        }   

                                        break;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return results;
        }

        private Rectangle CalculateBoundingBox(Point2f[] points)
        {
            if (points == null || points.Length != 4)
                return Rectangle.Empty;

            float minX = points.Min(p => p.X);
            float maxX = points.Max(p => p.X);
            float minY = points.Min(p => p.Y);
            float maxY = points.Max(p => p.Y);

            return new Rectangle(
                (int)minX,
                (int)minY,
                (int)(maxX - minX),
                (int)(maxY - minY)
            );
        }
        private PayloadType DetectPayloadType(string payload)
        {
            if (Uri.TryCreate(payload, UriKind.Absolute, out Uri uri))
                return PayloadType.Url;

            if (payload.StartsWith("WIFI:", StringComparison.OrdinalIgnoreCase))
                return PayloadType.WiFi;

            if (payload.StartsWith("BEGIN:VCARD", StringComparison.OrdinalIgnoreCase))
                return PayloadType.vCard;

            if (payload.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase))
                return PayloadType.Email;

            if (payload.StartsWith("tel:", StringComparison.OrdinalIgnoreCase))
                return PayloadType.Phone;

            if (payload.StartsWith("sms:", StringComparison.OrdinalIgnoreCase))
                return PayloadType.SMS;

            if (payload.StartsWith("geo:", StringComparison.OrdinalIgnoreCase))
                return PayloadType.Geo;

            return PayloadType.Text;
        }
    }

    public class Generator
    {
        public Bitmap GenerateQRCode(string payload, Color foreground, Color background, int size,
            QRCodeGenerator.ECCLevel errorCorrection, int margin)
        {
            try
            {
                using (var qrGenerator = new QRCodeGenerator())
                using (var qrCodeData = qrGenerator.CreateQrCode(payload, errorCorrection))
                using (var qrCode = new QRCode(qrCodeData))
                {
                    
                    var qrBitmap = qrCode.GetGraphic(20, foreground, background, true);

                    var result = new Bitmap(size, size);
                    using (var graphics = Graphics.FromImage(result))
                    {
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphics.DrawImage(qrBitmap, 0, 0, size, size);
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("QR code generation failed", ex);
            }
        }
    }

    public class ImageUtils
    {
        public Bitmap LoadImage(string path)
        {
            return new Bitmap(path);
        }

        public Bitmap ResizeImage(Bitmap image, int width, int height)
        {
            var result = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(result))
            {
                graphics.DrawImage(image, 0, 0, width, height);
            }
            return result;
        }
    }

    public class WorkerManager
    {
        private CancellationTokenSource cancellationTokenSource;

        public async Task RunAsync(Func<CancellationToken, Task> work, Action onComplete = null, Action<Exception> onError = null)
        {
            cancellationTokenSource = new CancellationTokenSource();

            try
            {
                await work(cancellationTokenSource.Token);
                onComplete?.Invoke();
            }
            catch (OperationCanceledException)
            {
                // Task was cancelled, no action needed
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex);
            }
        }

        public void Cancel()
        {
            cancellationTokenSource?.Cancel();
        }
    }
    #endregion
}