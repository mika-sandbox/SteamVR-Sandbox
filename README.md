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

| Scene Name                      | Description                                                                  |
| ------------------------------- | ---------------------------------------------------------------------------- |
| `VRMirror`                      | Mirror in VR World                                                           |
| `AvatarCalibration`             | Calibrate VR Avatar by Player Height                                         |
| `IndexControllerFingerTracking` | Finger Tracking by Valve Index Controller inputs                             |


## Preparation

All of scenes does not contains 3D model prefabs.  
But, all C# scripts that other than VRIK are attached in `VR_Player` GameObject that outside of Prefab.  
Please configure `Animator`, `Transform` and `IK` to your imported 3D model.

* The ViewPosition of the `AvatarCalibrator` needs to be set to an optimal value for each 3D model.
  * Examples
    * Quiche : `(x, y, z) = (0, 1.22, 0.19)`
    * Shapell: `(x, y, z) = (0, 1.18, 0.155)`
  * This is the same value as the View Position setting on VRChat Avatar Descriptor. However, the Z value uses +0.1f.
* In the scene after `FingerTrackingWithAnimation`, attach the `IKPass` C# script to 3D model as shown in the image.


## About included 3D Model

This repository uses "Shapell" that published in CC0 as an example of a 3D model.  
You can download original 3D model from [here](https://booth.pm/ja/items/1349366).