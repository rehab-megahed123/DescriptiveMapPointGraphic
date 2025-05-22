# Descriptive Map Point Tool

A custom ArcGIS Pro Add-In that allows users to interactively add, describe, and edit points on a map through an intuitive WPF Dockpane interface. Ideal for GIS professionals who need to capture descriptive spatial data directly within ArcGIS Pro.

---

## 📌 Features

- 🗺️ Add custom points on the map.
- 📝 Provide descriptive text for each point.
- ✏️ Edit point descriptions and locations.
- 🧾 View and manage all points through a responsive Dockpane.
- 🔄 Real-time UI updates using MVVM pattern.
- 🎨 Professional WPF design with customized styling.

---

## 🧩 Prerequisites

Before installing or using the add-in, make sure you have the following:

- **ArcGIS Pro** (version 2.9 or later)
- **Visual Studio 2019 or later**
- **.NET Framework 4.8**
- **ArcGIS Pro SDK for .NET** (installed via Visual Studio Extension Manager)

---

## 🚀 Installation

1. **Clone or Download** this repository.
2. Open the solution (`*.sln`) in Visual Studio.
3. Make sure all dependencies and SDK references are restored.
4. Build the project in `Release` mode.
5. The `.esriAddInX` file will be generated in the `\bin\Release` folder.
6. Double-click the `.esriAddInX` file to install the add-in to ArcGIS Pro. or (F5)

---

## 🧠 How to Use

1. Open ArcGIS Pro and load any map project.
2. Activate the **Add-In** from the `Add-Ins` tab.
3. A **Dockpane** titled `Descriptive Map Point Tool` will appear.
4. Type a description in the textbox.
5. Click **"Add Point"** then click on the map to place the point.
6. You can see all added points listed in the pane with options to **Edit** them.
7. Click **"Show Count"** to display the total number of points.

---

## 💡 Technologies Used

- WPF (.NET)
- MVVM Design Pattern
- ArcGIS Pro SDK for .NET
- C#
- XAML

---
## ⚠️ Notes

- Ensure ArcGIS Pro is installed before launching the add-in.
- The add-in currently supports English UI.
- Editing location updates the geometry of the point on the map.

---

## 📬 Contact

For support or inquiries, please contact:
**Rahab Megahed — GIS .NET Developer**  
📧 [rehabmegahed241@gmail.com]

---

## 📄 License

This project is licensed under the [MIT License](LICENSE) — feel free to use and modify.

