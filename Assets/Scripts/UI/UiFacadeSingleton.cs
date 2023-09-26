using System;
using UnityEngine;

namespace CodeSamplesDOTS
{
    public class UiFacadeSingleton : MonoBehaviour
    {
        //Akward singleton creation: We can't make this normal way in Awake
        //as we want to use the Instance in OnCreate method within ECS Systems.
        //OnCreate in systems is beeing called before Awakes or in a random order (at least I think so)
        private static UiFacadeSingleton _instance;
        public static UiFacadeSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<UiFacadeSingleton>();
                }
                return _instance;
            }
        }

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
    }
}
