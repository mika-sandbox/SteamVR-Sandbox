using UnityEngine;
using UnityEngine.EventSystems;

using static SteamVR_Sandbox.SteamVR.SteamVRInputModule;

namespace SteamVR_Sandbox.Avatar
{
    public class AvatarLaserPointer : LaserPointerRaycastReceiver
    {
        private GameObject _laser;
        private GameObject _pointer;

        [SerializeField]
        private Material LaserMaterial;

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        [SerializeField]
        private float LaserThickness = 0.001f;

        [SerializeField]
        private Material PointerMaterial;

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        [SerializeField]
        private float PointerRadius = 0.05f;

        public override void OnUpdate(RaycastResult raycast)
        {
            if (raycast.gameObject)
            {
                _laser.transform.localScale = new Vector3(LaserThickness * 4f, LaserThickness * 4f, raycast.distance);
                _laser.transform.localPosition = new Vector3(0f, 0f, raycast.distance / 2f);

                _pointer.transform.position = raycast.worldPosition;
                _pointer.SetActive(true);
            }
            else
            {
                _laser.transform.localScale = new Vector3(LaserThickness, LaserThickness, 0f);
                _laser.transform.localPosition = new Vector3(0f, 0f, 0f);

                _pointer.SetActive(false);
            }
        }

        private void Start()
        {
            if (LaserMaterial == null)
                Debug.LogWarning("No Laser Material found on this component", this);
            if (PointerMaterial == null)
                Debug.LogWarning("No Pointer Material found on this component", this);

            _laser = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _laser.transform.parent = transform;
            _laser.transform.localScale = new Vector3(LaserThickness, LaserThickness, 100f);
            _laser.transform.localPosition = new Vector3(0f, 0f, 50f);
            _laser.transform.localRotation = Quaternion.identity;
            _laser.GetComponent<MeshRenderer>().material = LaserMaterial;

            _pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            _pointer.transform.parent = transform;
            _pointer.transform.localScale = new Vector3(PointerRadius, PointerRadius, PointerRadius);
            _pointer.transform.localPosition = new Vector3(0f, 0f, 0f);
            _pointer.GetComponent<MeshRenderer>().material = PointerMaterial;
            _pointer.SetActive(false);
        }
    }
}