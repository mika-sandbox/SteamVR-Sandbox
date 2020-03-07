using System.Globalization;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace SteamVR_Sandbox.UI
{
    public class BindingValueToText : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _labelObject;

        private Slider _slider;

        private void Start()
        {
            _slider = GetComponent<Slider>();
        }

        public void OnValueChanged()
        {
            if (_labelObject)
                _labelObject.text = _slider.value.ToString(CultureInfo.CurrentCulture);
        }
    }
}