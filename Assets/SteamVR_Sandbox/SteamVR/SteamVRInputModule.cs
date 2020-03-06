using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

using Valve.VR;

namespace SteamVR_Sandbox.SteamVR
{
    [AddComponentMenu("Scripts/Mochizuki.VR/SteamVR/SteamVR Input Module")]
    public class SteamVRInputModule : BaseInputModule
    {
        private List<RaycastResult> _raycastResultsCache;
        private Camera _uiCamera;

        [SerializeField]
        private InputSource InputSourceLeft;

        [SerializeField]
        private InputSource InputSourceRight;

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        [SerializeField]
        private SteamVR_Action_Boolean InteractUI = SteamVR_Input.GetBooleanAction("InteractUI");

        private List<InputSource> Poses => new List<InputSource> { InputSourceLeft, InputSourceRight };

        protected override void Start()
        {
            base.Start();

            if (InteractUI == null)
                Debug.LogError("No UI interaction action has been set on this component", this);
            InputSourceLeft.Initialize(eventSystem);
            InputSourceRight.Initialize(eventSystem);

            // Camera for UI detection, not rendering objects
            _uiCamera = new GameObject("UI Camera").AddComponent<Camera>();
            _uiCamera.clearFlags = CameraClearFlags.Nothing;
            _uiCamera.cullingMask = 0;
            _uiCamera.enabled = false;
            _uiCamera.fieldOfView = 1;
            _uiCamera.nearClipPlane = 0.01f;

            foreach (var canvas in Resources.FindObjectsOfTypeAll<Canvas>())
                canvas.worldCamera = _uiCamera;
        }

        protected override void Awake()
        {
            base.Awake();

            _raycastResultsCache = new List<RaycastResult>();
        }

        public override void Process()
        {
            if (!InputSourceLeft.Validate() || !InputSourceRight.Validate())
                return;

            Poses.ForEach(ProcessEvents);
        }

        private void ProcessEvents(InputSource source)
        {
            UpdateCameraPositionTo(source.Pose.transform);

            source.EventData.Reset();
            source.EventData.position = new Vector2(_uiCamera.pixelWidth * 0.5f, _uiCamera.pixelHeight * 0.5f);

            eventSystem.RaycastAll(source.EventData, _raycastResultsCache);
            source.EventData.pointerCurrentRaycast = FindFirstRaycast(_raycastResultsCache);
            source.Receiver?.OnUpdate(source.EventData.pointerCurrentRaycast);
            _raycastResultsCache.Clear();

            HandlePointerExitAndEnter(source.EventData, source.EventData.pointerCurrentRaycast.gameObject);

            if (InteractUI.GetState(source.Pose.inputSource))
            {
                if (source.PreviousContactObject == null)
                {
                    HandlePress(source);
                }
                else if (source.PreviousContactObject != source.EventData.pointerCurrentRaycast.gameObject)
                {
                    HandleRelease(source);
                    HandlePress(source);
                }
                else
                {
                    // drag
                    ExecuteEvents.Execute(source.PreviousContactObject, source.EventData, ExecuteEvents.dragHandler);
                }

                return;
            }

            if (source.PreviousContactObject) HandleRelease(source);
        }

        // ReSharper disable once ParameterHidesMember
        private void UpdateCameraPositionTo(Transform transform)
        {
            _uiCamera.transform.position = transform.position;
            _uiCamera.transform.rotation = transform.rotation;
        }

        private void HandlePress(InputSource source)
        {
            // press
            source.PreviousContactObject = source.EventData.pointerCurrentRaycast.gameObject;
            source.EventData.pointerPressRaycast = source.EventData.pointerCurrentRaycast;

            var pressed = ExecuteEvents.ExecuteHierarchy(source.PreviousContactObject, source.EventData, ExecuteEvents.pointerDownHandler);
            if (pressed == null)
            {
                pressed = ExecuteEvents.GetEventHandler<IPointerClickHandler>(source.PreviousContactObject);
                ExecuteEvents.Execute(source.PreviousContactObject, source.EventData, ExecuteEvents.pointerClickHandler);
                ExecuteEvents.Execute(source.PreviousContactObject, source.EventData, ExecuteEvents.beginDragHandler);
            }
            else
            {
                ExecuteEvents.Execute(pressed, source.EventData, ExecuteEvents.pointerClickHandler);
                ExecuteEvents.Execute(pressed, source.EventData, ExecuteEvents.beginDragHandler);
                ExecuteEvents.Execute(source.PreviousContactObject, source.EventData, ExecuteEvents.pointerClickHandler);
                ExecuteEvents.Execute(source.PreviousContactObject, source.EventData, ExecuteEvents.beginDragHandler);
            }

            if (pressed != null)
            {
                source.EventData.pressPosition = pressed.transform.position;
                eventSystem.SetSelectedGameObject(pressed);
            }

            source.EventData.pointerPress = pressed;
            source.EventData.pointerDrag = pressed;
            source.EventData.rawPointerPress = source.PreviousContactObject;
        }

        private void HandleRelease(InputSource source)
        {
            // release
            ExecuteEvents.Execute(source.EventData.pointerPress, source.EventData, ExecuteEvents.pointerUpHandler);
            ExecuteEvents.Execute(source.EventData.pointerDrag, source.EventData, ExecuteEvents.endDragHandler);

            eventSystem.SetSelectedGameObject(null);

            source.EventData.pressPosition = Vector2.zero;
            source.EventData.pointerPress = null;
            source.EventData.pointerDrag = null;
            source.EventData.rawPointerPress = null;
            source.PreviousContactObject = null;
        }

        [Serializable]
        private class InputSource
        {
            public PointerEventData EventData { get; private set; }
            public GameObject PreviousContactObject { get; set; }

            public void Initialize(EventSystem eventSystem)
            {
                if (Pose == null)
                    Debug.LogError("No SteamVR_Behaviour_Pose component found on this component");

                EventData = new PointerEventData(eventSystem);
                PreviousContactObject = null;
            }

            public bool Validate()
            {
                return Pose != null;
            }

            #region Pose

            [SerializeField]
            private SteamVR_Behaviour_Pose _pose;

            public SteamVR_Behaviour_Pose Pose
            {
                get => _pose;
                set => _pose = value;
            }

            #endregion

            #region Receiver

            [SerializeField]
            private LaserPointerRaycastReceiver _receiver;

            public LaserPointerRaycastReceiver Receiver
            {
                get => _receiver;
                set => _receiver = value;
            }

            #endregion
        }

        public abstract class LaserPointerRaycastReceiver : MonoBehaviour
        {
            public abstract void OnUpdate(RaycastResult raycast);
        }
    }
}