# Object Recognition in Mixed Reality
This Project uses Unity Sentis to detect and track objects in mixed reality.
When the user grabs an object, a door is spawned based on the type of object they grabbed.

![Demo](<media/demo.gif>)

## Usage
- Download the APK from [Releases](https://github.com/mirkosprojects/object-recognition-mr/releases/latest)
- Install the APK onto your Meta Quest 3
- Make sure you activate the necessary permissions
- Make sure you have completed the environment setup for your room `(Settings > Environment Setup > Set Up)`
- Launch the App `(Library > Unknown Sources > Object Recognition in MR)`

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

## Building from Source
- Open the Unity Project
- Go to `File > Build Profiles > New Android Profile`
- Click `Switch Profile`
- Click `Build`
- If prompted with the warning `Unsupported Input Handling`, select `Yes`
- Install the APK onto your Meta Quest 3

## Visualization
For debugging purposes, it can be useful to activate visualization. The following visualizations are available.
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

## Acknowledgements
- [PassthroughCameraApiSamples](https://github.com/oculus-samples/Unity-PassthroughCameraApiSamples)
- [Unity Sentis](https://docs.unity3d.com/Packages/com.unity.sentis@2.1/manual/index.html)
