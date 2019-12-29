# SteamVR Sandbox using Unity3D

Sandbox repository for SteamVR.


## Requirements

- Unity 2019.2.x
  - Arktoon Shaders (used in Avatar Material)
  - DynamicBone (used in Avatar)
  - Final IK (required)
  - SteamVR Plugin (required)
  - Vive Stereo Rendering Toolkit (required)


## Scenes

Each scene has a new implementation added to implemented in the previous scene (Rows above the table).  
For example, `AvatarCalibration` includes `VRMirror` implementations.  
If you want to try all implementations in this repository, play the last scene.

| Scene Name                      | Description                                                         |
| ------------------------------- | ------------------------------------------------------------------- |
| `VRMirror`                      | Mirror in VR World                                                  |
| `AvatarCalibration`             | Calibrate VR Avatar by Player Height                                |
| `IndexControllerFingerTracking` | Finger Tracking by Valve Index Controller inputs                    |
| `FingerTrackingWithAnimation`   | Finger Tracking by Valve Index Controller with Animation Controller |
| `FingerEmotions`                | Face Emotions using finger pose                                     |


## Preparation

If you want to use your own (original/purchased) 3D model, please configure it as following steps:

* Attach `VRIK` component to your own 3D model and configure it.
  * For some Solver values, set the following values:
    * `Solver/Spine/Head Target` : `SteamVRObjects/[CameraRig]/Camera/Head_Target`
    * `Solver/Left Arm/Target` : `SteamVRObjects/[CameraRig]/Controller (left)/LeftHand_Target`
    * `Solver/Right Arm/Target` : `SteamVRObjects/[CameraRig]/Controller (right)/RightHand_Target`
* Replace `Animator`, `Transform` and `IK` values set in the `World/VR_Player` with your own 3D model.
* The ViewPosition of the `AvatarCalibrator` needs to be set to an optimal value for each 3D model.
  * Examples
    * Quiche : `(x, y, z) = (0, 1.22, 0.19)`
    * Shapell: `(x, y, z) = (0, 1.18, 0.155)`
  * This is the same value as the View Position setting on VRChat Avatar Descriptor. However, the Z value uses +0.1f.
* In the scene after `FingerTrackingWithAnimation`, attach the `IKPass` component to 3D model as shown in the image.<br />
  <img src="https://user-images.githubusercontent.com/10832834/71507116-b4318000-28c6-11ea-8532-8e13fe99a2c9.PNG" width="400px" />


## About included 3D Model

This repository uses "Shapell" that published in CC0 as an example of a 3D model.  
You can download original 3D model from [here](https://booth.pm/ja/items/1349366).
