using System.Collections.Generic;
using Managers;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;
    [SerializeField] private Camera uiCamera;
    private Stack<GameObject> popupStack = new Stack<GameObject>();
    
    void Awake()
    {
        // 싱글톤 패턴 구현
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
    
    public void InstantiatePopUp(string path)
    {
        GameObject popUpPrefab = Resources.Load<GameObject>(path);
        if (popUpPrefab != null)
        {
            var popUpCanvas = Instantiate(popUpPrefab, this.transform).GetComponent<Canvas>();
            popUpCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            popUpCanvas.worldCamera = uiCamera;
        }
        else
        {
            Debug.LogError("프리팹을 불러올 수 없습니다.");
        }
    }
     
    // 팝업 열기
    public void OpenPopup(GameObject popup)
    {
        if (popupStack.Count > 0)
        {
            // 현재 팝업 비활성화
            popupStack.Peek().SetActive(false);
        }

        popupStack.Push(popup);
        popup.SetActive(true);
    }

    // 최상위 팝업 닫기
    public void CloseTopPopup()
    {
        if (popupStack.Count > 0)
        {
            GameObject topPopup = popupStack.Pop();
            topPopup.SetActive(false);
        }

        // 이전 팝업 활성화
        if (popupStack.Count > 0)
        {
            popupStack.Peek().SetActive(true);
        }
    }
}