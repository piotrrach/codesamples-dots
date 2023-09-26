using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeSamplesDOTS
{
    [RequireComponent(typeof(Animator))]
    public class CustomToggle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField]
        private AnimationTriggers _animationTriggers;
        [SerializeField]
        private UnityEvent _onSetOn;

        private Animator _animator;

        public UnityEvent<CustomToggle, bool> OnValueChange { get; private set; }
        public bool IsOn { get; private set; }

        /// <summary>
        /// Set isOn without invoking onValueChanged callback.
        /// </summary>
        /// <param name="value">New Value for isOn.</param>
        public void SetIsOnWithoutNotify(bool value)
        {
            Set(value, false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (IsOn) return;

            TriggerAnimation(_animationTriggers.highlightedTrigger);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (IsOn) return;

            TriggerAnimation(_animationTriggers.normalTrigger);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Set(IsOn = !IsOn);
            OnPointerEnter(eventData);
        }

        private void Awake()
        {
            OnValueChange = new UnityEvent<CustomToggle, bool>();
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            Set(false);
        }

        private void Set(bool value, bool sendCallback = true)
        {
            // if we are in a group and set to true, do group logic
            IsOn = value;

            if (IsOn)
            {
                _animator.SetTrigger(_animationTriggers.selectedTrigger);
                _onSetOn.Invoke();
            }
            else
            {
                _animator.SetTrigger(_animationTriggers.normalTrigger);
            }

            if (sendCallback)
            {
                OnValueChange.Invoke(this, IsOn);
            }
        }

        private void TriggerAnimation(string triggername)
        {
            if (_animator == null || !_animator.isActiveAndEnabled || !_animator.hasBoundPlayables || string.IsNullOrEmpty(triggername))
                return;

            _animator.ResetTrigger(_animationTriggers.normalTrigger);
            _animator.ResetTrigger(_animationTriggers.highlightedTrigger);
            _animator.ResetTrigger(_animationTriggers.pressedTrigger);
            _animator.ResetTrigger(_animationTriggers.selectedTrigger);
            _animator.ResetTrigger(_animationTriggers.disabledTrigger);

            _animator.SetTrigger(triggername);
        }
    }
}
