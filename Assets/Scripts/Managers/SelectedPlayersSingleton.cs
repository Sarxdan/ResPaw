using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class SelectedPlayersSingleton : MonoBehaviour
    {
        public static SelectedPlayersSingleton Instance { get; private set; }
        public EnumAnimals SelectedPlayerOne { get; set; }
        public EnumAnimals SelectedPlayerTwo { get; set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}
