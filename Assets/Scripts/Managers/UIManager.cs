using System.Collections.Generic;
using UnityEngine;
 
namespace Managers
{
    public class UIManager : Singleton<UIManager>
    {
        private Dictionary<string, UIBase> uiElements = new Dictionary<string, UIBase>();
        
        public T GetUI<T>(string id) where T : UIBase
        {
            string key = GetKey<T>(id);
            if (uiElements.TryGetValue(key, out UIBase instance))
            {
                return instance as T;
            }
            return null;
        }

        public T ShowUI<T>(T prefab, string id) where T : UIBase
        {
            string key = GetKey<T>(id);
            if (uiElements.TryGetValue(key, out UIBase instance))
            {
                instance.Show();
                return instance as T;
            }

            T uiInstance = Instantiate(prefab, transform);
            uiElements[key] = uiInstance;
            uiInstance.Show();
            return uiInstance;
        }

        public void HideUI<T>(string id) where T : UIBase
        {
            string key = GetKey<T>(id);
            if (uiElements.TryGetValue(key, out UIBase instance))
            {
                instance.Hide();
                uiElements.Remove(key);
            }
        }

        public void HideAll()
        {
            foreach (var instance in uiElements.Values)
            {
                instance.Hide();
                Destroy(instance.gameObject);
            }
            uiElements.Clear();
        }

        private string GetKey<T>(string id) where T : UIBase
        {
            return $"{typeof(T).Name}_{id}";
        }
    }
}