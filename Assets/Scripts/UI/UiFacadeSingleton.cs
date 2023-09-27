using System;
using UnityEngine;

namespace CodeSamplesDOTS
{
    public class UiFacadeSingleton : MonoBehaviour
    {
        public static UiFacadeSingleton Instance { get; private set; }


        [SerializeField] private GameObject _gameOverWindow;

        public Action OnStartPressed { get; set; }
        public Action<int> OnAgentQuantityChoosen { get; set; }

        /// <summary>
        /// Should be invoked only by quantity selecting button in UI
        /// </summary>
        /// <param name="quantity"></param>
        public void SetChoosenAgentQuantity(int quantity)
        {
            OnAgentQuantityChoosen?.Invoke(quantity);
        }

        /// <summary>
        /// Should be invoked only by start Button
        /// </summary>
        public void StartButtonPress()
        {
            OnStartPressed?.Invoke();
        }

        public void ShowGameOverWindow()
        {
            _gameOverWindow.SetActive(true);
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
    }
}
