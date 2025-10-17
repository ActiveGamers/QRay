# QRay 🔍

**Scan and generate QR codes — beautifully, locally, and effortlessly!**  
QRay is a modern C# QR code scanner and generator built with **.NET 9**, designed for simplicity and power.  
It processes all your QR codes **locally** on your PC, with zero cloud dependency — just clean, instant QR operations.

---

## Features ✨

- 📷 **Smart QR Scanning** – Detect and decode QR codes in any image with advanced detection algorithms
- 🎨 **Custom QR Generation** – Create beautiful QR codes with custom colors, logos, and transparent backgrounds
- 📁 **Local Storage** – Stores all scan history in a local SQLite database at `%localappdata%\ActiveGamers\QRay\`
- 🔄 **Batch Operations** – Scan multiple images or generate QR codes from CSV files in bulk
- 📊 **History Management** – Full history with filtering, search, and export capabilities
- 🛡️ **Safety Features** – URL safety confirmation before opening links from scanned QR codes
- 🎯 **Content Detection** – Smart detection of QR content types (URL, WiFi, vCard, Email, SMS, etc.)
- 💾 **Multiple Export Formats** – Save QR codes as PNG (with transparency) or JPG
- 📋 **Quick Actions** – Copy, open, or generate new QR codes based on scan results
- 📝 **Comprehensive Logging** – Detailed logs stored in `%localappdata%\ActiveGamers\QRay\Logs\`
- 🎨 **Modern UI** – Clean, intuitive interface with drag & drop support
- ⚡ **Performance** – Async operations with cancellation support for smooth user experience

---

## Installation 💻

1. Download the latest [Release](https://github.com/ActiveGamers/QRay/releases) for Windows.
2. Run the application.
3. Start scanning and generating QR codes!

> ✅ **QRay** is a self-contained application and does **not require .NET to be installed** separately.

---

## Usage 🧰

### Scanning QR Codes
1. Go to the **Scan** tab
2. Drag & drop an image or click **Browse** to select a file
3. Click **Decode** to scan for QR codes
4. View results with type-specific actions (Open URL, Copy, Export vCard, etc.)

### Generating QR Codes
1. Go to the **Generate** tab
2. Select content type (Text, URL, WiFi, vCard, etc.)
3. Customize appearance (colors, size, error correction, margin)
4. Add optional logo with scannability verification
5. Click **Generate** and save your QR code

### Batch Operations
- **Batch Scan**: Process entire folders of images for QR codes
- **Batch Generate**: Create multiple QR codes from CSV files

### History Management
- View all scan results in the **History** tab
- Filter and search through your scan history
- Export history to CSV or JSON formats

---

## Supported Formats 📄

### Input Images (Scanning)
- PNG, JPG/JPEG, BMP, GIF, WebP

### Output Formats (Generation)
- PNG (with transparent background support)
- JPG/JPEG

### QR Code Types
- URLs and web links
- Plain text
- vCard contacts
- WiFi credentials
- Email addresses
- SMS messages
- Phone numbers
- Geographic coordinates
- Calendar events
- Custom text

---

## Technical Details 🔧

- **Platform**: Windows Desktop
- **Framework**: .NET 9.0 (net9.0-windows)
- **UI**: WinForms with modern controls
- **Database**: SQLite with Microsoft.Data.Sqlite
- **QR Generation**: QRCoder library
- **QR Decoding**: OpenCvSharp for computer vision
- **Image Processing**: SkiaSharp/System.Drawing
- **Architecture**: Async/await with cancellation tokens
- **Storage**: All data stored locally in user app data folder

---

## File Structure 📁
%localappdata%\ActiveGamers\QRay
├── DB\ # SQLite database files
├── Logs\ # Application logs with rotation
├── Config\ # Settings and configuration
├── Exports\ # Generated QR code exports
└── Reports\ # Diagnostic reports

---

## Contributing 🤝

Contributions are welcome!
Feel free to open issues, suggest features, or submit pull requests to improve QRay.

---

## License 📝

This project is licensed under the **MIT License**.  
See the [LICENSE](LICENSE) file for more details.

---

## Links 🔗

- GitHub Repository: [https://github.com/ActiveGamers/QRay](https://github.com/ActiveGamers/QRay)
- Developer: [ActiveGamers](https://github.com/ActiveGamers)
- Build Toolchain: **.NET 9 + WinForms + SQLite + OpenCvSharp + QRCoder**

---

### Made With ♥ In Iran by [ActiveGamers](https://github.com/ActiveGamers)
