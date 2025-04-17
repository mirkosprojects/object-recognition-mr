---
library_name: unity-sentis
pipeline_tag: object-detection
---
# YOLOv8, YOLOv9, YOLO11, YOLO12 validated for Sentis 2.1.2 in Unity 6

[YOLO](https://docs.ultralytics.com/models/) is a real-time multi-object recognition model.
Small and Nano model sizes are included for YOLO version 8 and above (except version 10 which uses NMS-free approach).

## How to Use

* Create a new scene in Unity 6;
* Install `com.unity.sentis` version `2.1.2` from the package manager;
* Add the `RunYOLO.cs` script to the Main Camera;
* Drag an appropriate `.onnx` file from the `Models` folder into the `Model Asset` field;
* Drag the `classes.txt` file into the `Classes Asset` field;
* Create a `GameObject > UI > Raw Image` object in the scene, set its width and height to 640, and link it as the `Display Image` field;
* Drag the `Border Texture.png` file into the `Border Texture` field;
* Select an appropriate font in the `Font` field;
* Put a video file in the `Assets/StreamingAssets` folder and set the `Video Filename` field to the filename of the video.

## Preview
If working correctly you should see something like this:

![preview](preview.jpg)

## Information
The NMS selection will be improved in later versions of Sentis. Currently uses singular-class approach.

## Unity Sentis
Unity Sentis is the inference engine that runs in Unity 3D. More information can be found at [here](https://unity.com/products/sentis)

## License
The YOLO models use the GPLv3 license.