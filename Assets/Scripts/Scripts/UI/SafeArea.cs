using UnityEngine;

public class SafeArea : MonoBehaviour
{
    private Rect lastSafeArea = new Rect(0, 0, 0, 0);
    
        private void Awake()
        {
            Refresh();
        }
    
        private void Update()
        {
            Refresh();
        }
    
        private void Refresh()
        {
            Rect safeArea = Screen.safeArea;
    
            if (safeArea != lastSafeArea)
            {
                ApplySafeArea(safeArea);
            }
        }
    
        private void ApplySafeArea(Rect safeArea)
        {
            lastSafeArea = safeArea;
    
            // Convert safe area rectangle from absolute pixels to relative anchor coordinates
            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
    
            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
        }
}