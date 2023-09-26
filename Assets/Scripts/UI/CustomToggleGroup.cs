using System;
using System.Linq;
using UnityEngine;

namespace CodeSamplesDOTS
{
    public class CustomToggleGroup : MonoBehaviour
    {
        private CustomToggle[] customToggles;

        public Action OnTogglesChanged { get; set; }
        public bool IsAnySelected { get; private set; }

        private void Awake()
        {
            customToggles = GetComponentsInChildren<CustomToggle>();
        }

        private void Start()
        {
            foreach (CustomToggle toggle in customToggles)
            {
                toggle.OnValueChange.AddListener(OnToggleValueChanged);
            }
        }

        private void OnToggleValueChanged(CustomToggle toggleChanged, bool isOn)
        {

            foreach (CustomToggle toggle in customToggles)
            {
                if (toggle == toggleChanged)
                {
                    continue;
                }
                else
                {
                    toggle.SetIsOnWithoutNotify(false);
                }
                if (toggleChanged.IsOn)
                {
                    IsAnySelected = true;
                }
            }

            IsAnySelected = isOn;
            OnTogglesChanged.Invoke();
        }
    }
}
