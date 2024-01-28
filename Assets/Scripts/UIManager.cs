using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        // Singleton 인스턴스
        public static UIManager Instance { get; private set; }

        // 모든 UI 요소를 저장하는 딕셔너리
        private Dictionary<string, UIBase> uiElements = new Dictionary<string, UIBase>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void RegisterUIElement(string key, UIBase uiElement)
        {
            if (!uiElements.ContainsKey(key))
            {
                uiElements[key] = uiElement;
            }
        }

        public void ShowUIElement(string key)
        {
            if (uiElements.TryGetValue(key, out UIBase uiElement))
            {
                uiElement.Show();
            }
        }

        public void HideUIElement(string key)
        {
            if (uiElements.TryGetValue(key, out UIBase uiElement))
            {
                uiElement.Hide();
            }
        }

        public void HideAllUIElements()
        {
            foreach (var uiElement in uiElements.Values)
            {
                uiElement.Hide();
            }
        }
    }
}