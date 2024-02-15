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

        
    }
}
