# Object Recognition in Mixed Reality
This Project uses Unity Sentis to detect and track objects in mixed reality.
When the user grabs an object, a door is spawned based on the type of object they grabbed.

## Installation
- Make sure that Git LFS is installed on your machine
- You can check it by using:
  ```sh
  git lfs version
  ```
- Clone the repository:
  ```sh
  git clone https://github.com/mirkosprojects/object-recognition-mr.git
  ```

## Usage
- Install the APK onto your Meta Quest 3
- Make sure you activate the necessary permissions (**TODO**)
- Make sure you have completed the environment setup for your room `(Settings > Environment Setup > Set Up)`
- Launch the App `(Library > Unknown Sources > Object Recognition in MR)`

## Building from Source
- Open the Unity Project
- Go to `File > Build Profiles > New Android Profile`
- Click `Switch Profile`
- Click `Build`
- If prompted to select Input Handling, select `Both`
- Install the APK onto your Meta Quest 3

## Visualization
For debugging purposes, it can be useful to activate visualization. We have the following four visualizations.
- **Bounding Boxes:** Shows the Bounding Boxes of detected objects after every detection
- **Object Tracker Markers:** Shows the tracked objects in 3D space
- **Hand Poses:** Shows particle effects and text for detected hand poses
- **MRUK Room Mesh:** Shows the room outline

### Activating Bounding Boxes
- In the Unity Project, open the Scene `Objectdetection Handgrab`
- Open `SentisInferenceManagerPrefab`
- Activate `Sentis Inference Ui Manager` **AND** `Sentis Object Detected Ui Manager`

### Activating Object Tracker Markers
- In the Unity Project, open the Scene `Objectdetection Handgrab`
- Open `SentisInferenceManagerPrefab`
- Under `Object Tracker Manager` go to `Marker Settings` and activate `Visualize Markers`

### Activate Hand Poses
- In the Unity Project, open the Scene `Objectdetection Handgrab`
- Expand `Poses`
- Activate `PoseRecognizedVisuals`

### Activate MRUK Room Mesh
- In the Unity Project, open the Scene `Objectdetection Handgrab`
- Activate `EffectMesh`




# Unity Project Template

Welcome to the **Unity Project Template**! 🎮 This repository serves as a base template for Unity projects within the **UnityLab** group in GitLab. It provides a standardized structure, ensuring consistency across all Unity projects.

## 📂 Project Structure
This template follows a typical Unity project layout with additional files for Git and version control management.

```
UnityProjectTemplate/    # The root directory of this repository
│── .gitignore           # Defines files to be ignored in version control
│── .gitattributes       # Ensures Git LFS handles large binary files correctly
│── README.md            # Documentation for this template
│── <UnityProjectName>/  # Directory of the Unity project
  │── Assets/              # All game assets, scripts, prefabs, and scenes
  │── Packages/            # Unity package dependencies
  │── ProjectSettings/     # Unity project settings (important for version control)
  │── UserSettings/        # Local user settings (ignored in Git)

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
### **1️⃣ Prerequisites**
- Make sure that Git LFS is installed on your machine
- You can check it by using:
  ```sh
  git lfs version
  ```
  
### **2️⃣ Create a New Project From This Template**
- Clone this repository and rename it:
  ```sh
  git clone https://git.it.hs-heilbronn.de/unitylab/templates/unity-project-template.git new-project
  cd new-project
  ```

- Open Unity Hub and click on "New project" to create a blank Unity project.

- Choose this repository folder as location for the Unity project.

- Click on "Create project".

- After the new Unity project has been created, open a CLI in the root directory of the Git repository

- Remove Git history to start fresh (optional):
  ```sh
  rm -rf .git    # Works only on Linux - on Windows delete the .git folder in the root directory of the repository.
  git init
  git add .
  git commit -m "Initialize new Unity project"
  ```

- If you removed the Git history in the previous (optional) step, use:
  ```sh
  git remote add origin https://git.it.hs-heilbronn.de/<Group>/new-project.git
  git push -u origin main
  ```

- If you have not removed the Git history, use:
  ```sh
  git add .
  git commit -m "Initialize new Unity project"
  git push
  ```

### **3️⃣ Forking Option (Alternative)**
- If cloning is not an option, simply **fork** this repository into your own project.

## ✅ Best Practices
- Use **Git LFS** for large assets.
- Keep **ProjectSettings/** in Git to ensure consistency.
- Do **not** commit **Library/**, **Temp/**, or **Builds/** folders.
- Follow Unity coding standards for **C# scripts**.

## 🚀 Happy Developing!
If you have any issues or suggestions for improving this template, feel free to contribute or open an issue. 🎮