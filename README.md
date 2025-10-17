# Qrymien 🧩

**Scan, decode, and create QR Codes — instantly and offline!**  
Qrymien is a modern C# utility built with **.NET 9**, designed for creators, developers, and curious users.  
It can **read any QR Code** from an image and **generate custom QR images** — all fully offline and locally.

---

## Features ✨

- 🧠 **Smart QR Reader** – Detect and decode QR codes from any selected image  
- 🔗 **Auto Link Recognition** – If the QR contains a website link, open it directly in one click  
- 📜 **Text, URL, and Data Detection** – Supports text, URLs, contact cards, and all standard QR formats  
- 🖼️ **Batch Image Support** – Import one or multiple images to scan them all at once  
- ⚡ **Offline & Local** – Works completely offline; no API or internet needed  
- 🪄 **QR Generator** – Create custom QR codes from any text, link, or data instantly  
- 🎨 **Output Formats** – Save QR as **PNG** or **JPG**  
- 🧷 **Transparent Option (PNG Only)** – Choose between **transparent** or **white background**  
- 📏 **Custom Size Support** – Set QR dimensions manually (e.g., `100x100px`, `160x160px`)  
- 🧱 **Resizable & Modern UI** – Clean and responsive WinForms layout built for .NET 9  
- 🕶️ **Dark & Light Themes** – Toggle UI themes to match your Windows mode  
- 💾 **Auto Save Paths** – All data and settings stored in `%localappdata%\ActiveGamers\Qrymien\`  
- 🧭 **Recent Files** – Quickly reopen your last scanned or generated images  
- 🧩 **Integrated Preview** – See live preview of generated QR before saving  
- 🧰 **Logging System** – Logs every scan, generation, or error inside `%localappdata%\ActiveGamers\Qrymien\Logs\`

---

## Installation 💻

1. Download the latest [Release](https://github.com/ActiveGamers/Qrymien/releases) for Windows.  
2. Run the installer and open **Qrymien** from the Start Menu or Desktop.

> ✅ **Qrymien** is fully self-contained — no .NET installation or internet required.

---

## Usage 🧰

1. Launch **Qrymien**.
2. To **scan a QR Code**:
   - Click **“Scan Image”**, select an image, and view the decoded result instantly.
   - If it’s a **link**, press **“Open Link”** to visit it directly.
3. To **generate a QR Code**:
   - Enter text or link in the input box.
   - Choose **PNG** or **JPG** format.
   - (Optional) Enable **Transparent Background** for PNG.
   - Select your preferred **size** (e.g., `120x120px`).
   - Click **Generate**, then **Save**.
4. Check logs or cache in `%localappdata%\ActiveGamers\Qrymien\Logs\`.

---

## Logs Example

2025/10/17-20:01:03 [Info] App Started.
2025/10/17-20:01:06 [Info] User Selected Image: sample_qr.png
2025/10/17-20:01:07 [Info] QR Content Detected: https://activegamers.ir
2025/10/17-20:01:07 [Info] Type: URL | Action: Show “Open Link” Button
2025/10/17-20:02:12 [Info] User Generated New QR | Format: PNG | Size: 160x160 | Transparent: True
2025/10/17-20:02:13 [Info] File Saved To %localappdata%\ActiveGamers\QREasy\Exports\myqr.png

---

## Contributing 🤝

Contributions are always welcome!  
Feel free to open issues, suggest features, or submit pull requests to improve Qrymien.

---

## License 📝

This project is licensed under the **MIT License**.  
See the [LICENSE](LICENSE) file for more details.

---

## Links 🔗

- GitHub Repository: [https://github.com/ActiveGamers/Qrymien](https://github.com/ActiveGamers/Qrymien)  
- Developer: [ActiveGamers](https://github.com/ActiveGamers)  
- Build Toolchain: **.NET 9 + WinForms + ZXing.NET**

---

### Made With ♥ In Iran by [ActiveGamers](https://github.com/ActiveGamers)
