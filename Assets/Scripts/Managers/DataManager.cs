using System;
using UnityEngine;

namespace Managers
{
    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance;
        
        private DataManager() {}

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
        }

        private void ConversionToJsonFailedCallback()
        {
            throw new System.NotImplementedException();
        }

        private void ConversionToJsonSuccessfullCallback()
        {
            throw new System.NotImplementedException();
        }
    }
}
