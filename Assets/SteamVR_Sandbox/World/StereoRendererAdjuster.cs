using HTC.UnityPlugin.StereoRendering;

using UnityEngine;

namespace SteamVR_Sandbox.World
{
    [AddComponentMenu("Scripts/Mochizuki.VR/World/Stereo Renderer Adjuster")]
    [RequireComponent(typeof(StereoRenderer))]
    public class StereoRendererAdjuster : MonoBehaviour
    {
        private Vector3 _previousWorldPos;

        // Update is called once per frame
        private void Update()
        {
            var worldPos = gameObject.transform.position;

            if (worldPos != _previousWorldPos)
            {
                _previousWorldPos = worldPos;

                var stereoRenderer = gameObject.GetComponent<StereoRenderer>();
                stereoRenderer.canvasOriginPos = worldPos;
                stereoRenderer.anchorPos = worldPos;
            }
        }
    }
}