using UnityEngine;
using UnityEngine.UI;

namespace CodeSamplesDOTS
{
    [RequireComponent(typeof(Button))]
    public class StartButtonController : MonoBehaviour
    {
        [SerializeField]
        private CustomToggleGroup _quantitySelectingToggleGroup;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _quantitySelectingToggleGroup.OnTogglesChanged += UpdateButton;
        }

        private void UpdateButton()
        {
            _button.interactable = _quantitySelectingToggleGroup.IsAnySelected;
        }

    }
}
