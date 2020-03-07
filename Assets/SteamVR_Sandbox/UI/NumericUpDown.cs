using TMPro;

using UnityEngine;

namespace SteamVR_Sandbox.UI
{
    public class NumericUpDown : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI Label;

        [SerializeField]
        private int MaxValue;

        [SerializeField]
        private int MinValue;

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        [SerializeField]
        private int Step = 1;

        public void OnClickUpButton()
        {
            var newValue = Value + Step;
            if (newValue > MaxValue)
                return;

            Value = newValue;
        }

        public void OnClickDownButton()
        {
            var newValue = Value - Step;
            if (newValue < MinValue)
                return;

            Value = newValue;
        }

        private void SetValueToLabel(int value)
        {
            Label.text = $"{value}";
        }

        #region Value

        [SerializeField]
        private int _value;

        public int Value
        {
            get => _value;
            set
            {
                if (_value == value)
                    return;
                SetValueToLabel(value);
                _value = value;
            }
        }

        #endregion
    }
}