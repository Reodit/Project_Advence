using System.Linq;
using UnityEngine;

namespace Utilities
{
    public static class ObjectHelper
    {
        public static GameObject FindGameObjectByName(string name)
        {
            GameObject[] foundObjects = GameObject.FindObjectsOfType<GameObject>().Where(obj => obj.name == name).ToArray();

            if (foundObjects.Length == 0)
            {
                Debug.LogError($"No GameObject found with the name '{name}'.");
                return null;
            }
            else if (foundObjects.Length > 1)
            {
                Debug.LogError($"Multiple GameObjects found with the name '{name}'.");
                return null;
            }

            return foundObjects[0];
        }
    
        public static T FindComponentInGameObject<T>(GameObject gameObject) where T : Component
        {
            if (gameObject == null)
            {
                Debug.LogError("GameObject is null.");
                return null;
            }

            T[] components = gameObject.GetComponents<T>();

            if (components.Length == 0)
            {
                Debug.LogError($"GameObject does not have a component of type {typeof(T)}.");
                return null;
            }
            else if (components.Length > 1)
            {
                Debug.LogError($"GameObject has multiple components of type {typeof(T)}.");
                return null;
            }

            return components[0];
        }
    }
}
