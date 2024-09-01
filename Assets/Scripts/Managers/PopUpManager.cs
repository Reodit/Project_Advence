using System.Collections.Generic;
using Managers;
using UnityEngine;

public class PopupManager : Singleton<PopupManager>
{
    [SerializeField] private Camera uiCamera;
    private Stack<GameObject> popupStack = new Stack<GameObject>();
    
    public void InstantiatePopUp(string path)
    {
        GameObject popUpPrefab = Resources.Load<GameObject>(path);
        if (popUpPrefab != null)
        {
            var popUpInstance = Instantiate(popUpPrefab, this.transform);
            var popUpCanvas = popUpInstance.GetComponent<Canvas>();
            popUpCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            popUpCanvas.worldCamera = uiCamera;
            popupStack.Push(popUpInstance);
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

    public void ClearPopup()
    {
        for (int i = 0; i < popupStack.Count; i++)
        {
            var popup = popupStack.Pop();
            Destroy(popup.gameObject);
        }
    }
}