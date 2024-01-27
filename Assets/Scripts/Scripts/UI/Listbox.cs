using UnityEngine;
using UnityEngine.UI;

public class Listbox : MonoBehaviour
{
    private ScrollRect scrollRect;
    public GameObject content;
    private RectTransform contentRectTransform;
    
    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        contentRectTransform = content.GetComponent<RectTransform>();
        LoadItems();
    }

    public void ScrollLimit()
    {
        float contentHeight = contentRectTransform.rect.height;
        float viewportHeight = scrollRect.viewport.rect.height;

        // 하단 제한 계산
        float lowerBound = 1f - (viewportHeight / contentHeight);
        lowerBound = Mathf.Clamp01(lowerBound);

        if (scrollRect.verticalNormalizedPosition >= 1.0f)
        {
            scrollRect.verticalNormalizedPosition = 1.0f;
        }

        else if (scrollRect.verticalNormalizedPosition == 0)
        {
            scrollRect.verticalNormalizedPosition = 1.0f;
        }
        
        else if (scrollRect.verticalNormalizedPosition < lowerBound)
        {
            scrollRect.verticalNormalizedPosition = 0;
        }
    }

    public void LoadItems()
    {
        string path = "Prefabs/UIPrefabs/Items";
        GameObject[] itemPrefabs = Resources.LoadAll<GameObject>(path);

        foreach (GameObject itemPrefab in itemPrefabs)
        {
            if (itemPrefab != null)
            {
                Instantiate(itemPrefab, contentRectTransform);
            }
        }
    }
}
