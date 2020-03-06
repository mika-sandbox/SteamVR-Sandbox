using System.Linq;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Valve.VR;

namespace SteamVR_Sandbox.Avatar
{
    public class AvatarLaserPointer : MonoBehaviour
    {
        private GameObject _laser;
        private GameObject _pointer;

        private Transform _previousContact;

        private Scene _scene;

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        [SerializeField]
        private SteamVR_Action_Boolean InteractUI = SteamVR_Input.GetBooleanAction("InteractUI");

        [SerializeField]
        private Material LaserMaterial;

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        [SerializeField]
        private float LaserThickness = 0.01f;

        [SerializeField]
        private Material PointerMaterial;

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        [SerializeField]
        private float PointerRadius = 0.05f;

        [SerializeField]
        private SteamVR_Behaviour_Pose Pose;

        private void Start()
        {
            if (InteractUI == null)
                Debug.LogError("No UI interaction action has been set on this component", this);
            if (LaserMaterial == null)
                Debug.LogWarning("No Laser Material found on this component", this);
            if (PointerMaterial == null)
                Debug.LogWarning("No Pointer Material found on this component", this);
            if (Pose == null)
                Debug.LogError("No SteamVR_Behaviour_Pose component found on this component", this);

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

            _scene = SceneManager.GetActiveScene();
        }

        private void Update()
        {
            // is hit to physics objects?
            var ray = new Ray(transform.position, transform.forward);

            // Physics.Raycast(ray, out var hitToObject, 100f);

            // is hit to uGUI objects?
            var hitToGui = RaycastToGuiObjects(ray, 100f);

            var nearestHitObject = GetNearestObjectFromPosition( /* new HitTransform(hitToObject), */ hitToGui);
            if (nearestHitObject == null)
            {
                // no hit
                _laser.transform.localScale = new Vector3(LaserThickness, LaserThickness, 0f);
                _laser.transform.localPosition = new Vector3(0f, 0f, 0f);
                _pointer.SetActive(false);
            }

            // press downed
            var data = new PointerEventData(EventSystem.current);

            if (nearestHitObject != null)
            {
                data.position = nearestHitObject.Position;
                data.button = PointerEventData.InputButton.Left;

                _laser.transform.localScale = new Vector3(LaserThickness * 4f, LaserThickness * 4f, nearestHitObject.Distance);
                _laser.transform.localPosition = new Vector3(0f, 0f, nearestHitObject.Distance / 2f);

                _pointer.transform.position = nearestHitObject.Position;
                _pointer.SetActive(true);

                if (InteractUI?.GetState(Pose.inputSource) == true)
                {
                    if (_previousContact == null)
                    {
                        _previousContact = nearestHitObject.Transform;
                        ExecuteEvents.Execute(_previousContact.gameObject, data, ExecuteEvents.pointerDownHandler);
                        ExecuteEvents.Execute(_previousContact.gameObject, data, ExecuteEvents.pointerClickHandler);
                        ExecuteEvents.Execute(_previousContact.gameObject, data, ExecuteEvents.beginDragHandler);
                    }
                    else if (nearestHitObject.Transform != _previousContact)
                    {
                        ExecuteEvents.Execute(_previousContact.gameObject, data, ExecuteEvents.endDragHandler);
                        ExecuteEvents.Execute(_previousContact.gameObject, data, ExecuteEvents.pointerUpHandler);
                        _previousContact = nearestHitObject.Transform;
                    }
                    else
                    {
                        ExecuteEvents.Execute(_previousContact.gameObject, data, ExecuteEvents.dragHandler);
                    }

                    return;
                }
            }

            if (_previousContact == null)
                return;

            ExecuteEvents.Execute(_previousContact.gameObject, data, ExecuteEvents.endDragHandler);
            ExecuteEvents.Execute(_previousContact.gameObject, data, ExecuteEvents.pointerUpHandler);
            _previousContact = null;
        }

        private HitTransform RaycastToGuiObjects(Ray ray, float maxDistance)
        {
            var canvases = _scene.GetRootGameObjects().SelectMany(w => w.GetComponentsInChildren<Canvas>());

            // see: http://edom18.hateblo.jp/entry/2019/12/20/121928
            foreach (var graphic in canvases.SelectMany(GraphicRegistry.GetGraphicsForCanvas).ToList())
            {
                if (graphic.depth == -1 || !graphic.raycastTarget)
                    continue;

                // ReSharper disable once LocalVariableHidesMember
                var transform = graphic.transform;
                var direction = Vector3.Dot(transform.forward, ray.direction);

                if (direction <= 0)
                    continue;

                var distance = Vector3.Dot(transform.forward, transform.position - ray.origin) / direction;
                if (distance >= maxDistance)
                    continue;

                var position = ray.GetPoint(distance);
                var pointerPos = Camera.main.WorldToScreenPoint(position);

                if (!RectTransformUtility.RectangleContainsScreenPoint(graphic.rectTransform, pointerPos, Camera.main))
                    continue;
                if (!graphic.Raycast(pointerPos, Camera.main))
                    continue;

                return new HitTransform(distance, position, graphic.transform);
            }

            return null;
        }

        private static HitTransform GetNearestObjectFromPosition(params HitTransform[] hits)
        {
            return hits.Where(w => w?.Transform != null).OrderBy(w => w.Distance).FirstOrDefault();
        }

        private class HitTransform
        {
            public float Distance { get; }
            public Vector3 Position { get; }
            public Transform Transform { get; }

            public HitTransform(RaycastHit hit)
            {
                Distance = hit.distance;
                Position = hit.point;
                Transform = hit.transform;
            }

            public HitTransform(float distance, Vector3 position, Transform transform)
            {
                Distance = distance;
                Position = position;
                Transform = transform;
            }
        }
    }
}