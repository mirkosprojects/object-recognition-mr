# Unity Project Template

Welcome to the **Unity Project Template**! 🎮 This repository serves as a base template for Unity projects within the **UnityLab** group in GitLab. It provides a standardized structure, ensuring consistency across all Unity projects.

## 📂 Project Structure
This template follows a typical Unity project layout with additional files for Git and version control management.

```
UnityProjectTemplate/
│── Assets/              # All game assets, scripts, prefabs, and scenes
│── Packages/            # Unity package dependencies
│── ProjectSettings/     # Unity project settings (important for version control)
│── UserSettings/        # Local user settings (ignored in Git)
│── .gitignore           # Defines files to be ignored in version control
│── .gitattributes       # Ensures Git LFS handles large binary files correctly
│── README.md            # Documentation for this template
```

## 📄 File Descriptions

### **1️⃣ Assets/**
Contains all game-related assets, including:
- **Scenes/** - Stores Unity scene files (`.unity`).
- **Scripts/** - Contains C# scripts for game logic.
- **Prefabs/** - Reusable game objects.
- **Textures/**, **Audio/**, **Models/** - Stores graphical, sound, and 3D assets.

### **2️⃣ Packages/**
- Manages dependencies via Unity Package Manager (`manifest.json`).
- If external packages are used, they are listed here.

### **3️⃣ ProjectSettings/**
- Contains Unity project-wide settings like:
  - `EditorBuildSettings.asset` - Defines scene order.
  - `InputManager.asset` - Stores input configurations.
  - `TagManager.asset` - Handles layers and tags.
- Important for ensuring consistency when collaborating in a team.

### **4️⃣ UserSettings/** (Git-Ignored)
- Stores personal user settings, like editor preferences.
- Not included in version control (ignored via `.gitignore`).

### **5️⃣ .gitignore**
- Prevents unnecessary or generated files from being committed.
- Includes:
  ```
  /Library/
  /Temp/
  /Logs/
  /Builds/
  /UserSettings/
  ```

### **6️⃣ .gitattributes**
- Ensures **Git LFS (Large File Storage)** is used for large binary files.
- Tracks:
  ```
  *.psd filter=lfs diff=lfs merge=lfs
  *.fbx filter=lfs diff=lfs merge=lfs
  *.png filter=lfs diff=lfs merge=lfs
  *.unity filter=lfs diff=lfs merge=lfs
  ```
- Prevents Git from bloating with large assets.

## 🛠 How to Use This Template
### **1️⃣ Create a New Project From This Template**
- Clone this repository and rename it:
  ```sh
  git clone https://git.it.hs-heilbronn.de/unitylab/templates/unity-project-template.git new-project
  cd new-project
  ```
- Remove Git history to start fresh:
  ```sh
  rm -rf .git
  git init
  git add .
  git commit -m "Initialize new Unity project"
  ```
- Push to your new GitLab project:
  ```sh
  git remote add origin https://git.it.hs-heilbronn.de/<Group>/new-project.git
  git push -u origin main
  ```

### **2️⃣ Forking Option (Alternative)**
- If cloning is not an option, simply **fork** this repository into your own project.

## ✅ Best Practices
- Use **Git LFS** for large assets.
- Keep **ProjectSettings/** in Git to ensure consistency.
- Do **not** commit **Library/**, **Temp/**, or **Builds/** folders.
- Follow Unity coding standards for **C# scripts**.

## 🚀 Happy Developing!
If you have any issues or suggestions for improving this template, feel free to contribute or open an issue. 🎮
