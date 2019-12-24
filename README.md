# SteamVR Sandbox using Unity3D

Sandbox repository for SteamVR.

## Requirements

- Unity 2019.2.x
  - Arktoon Shaders (used in Avatar Material)
  - DynamicBone (used in Avatar)
  - Final IK (required)
  - Gaia
  - MeshBaker
  - SteamVR Plugin (required)
  - Unity-Chan (used as Avatar)
  - Unity-Chan Toon Shader (used in Avatar Material)
  - Vive Stereo Rendering Toolkit (required)
  - Your 3D Model 
    - Samples uses Shapell and Quiche as player character

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
