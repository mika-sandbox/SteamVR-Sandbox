using HTC.UnityPlugin.StereoRendering;

using UnityEngine;

[RequireComponent(typeof(StereoRenderer))]
public class StereoRendererAdjuster : MonoBehaviour
{
    private Vector3 previousWorldPos;

    // Update is called once per frame
    private void Update()
    {
        var worldPos = gameObject.transform.position;

        if (worldPos != previousWorldPos)
        {
            previousWorldPos = worldPos;

            var stereoRenderer = gameObject.GetComponent<StereoRenderer>();
            stereoRenderer.canvasOriginPos = worldPos;
            stereoRenderer.anchorPos = worldPos;
        }
    }
}