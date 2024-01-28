using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    [Header("Buttons")] 
    [SerializeField] private Button captureButton;
    [SerializeField] private Button listButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button scaleButton;
    [SerializeField] private Button colorPickerButton;

    [Header("Canvas")] 
    public Canvas listBox;
    
    [Header("RectTransform")]
    public RectTransform mainLeft;
    public RectTransform mainRight;
    public RectTransform mainBottom;

    [Header("ETC")]
    
    public static MainCanvas Instance;
    private MainCanvas() {}

    public void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        scaleButton.onClick.AddListener(() => ToggleUI(mainBottom));

        colorPickerButton.onClick.AddListener(() => ToggleUI(mainRight));
        
    }
    
    /// <summary>
    /// canvas를 갖고 있는 게임 오브젝트 활성화
    /// </summary>
    /// <param name="canvas"></param>
    public void ShowUI(Canvas canvas)
    {
        if (canvas != null)
        {
            canvas.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// canvas를 갖고 있는 게임 오브젝트 비활성화
    /// </summary>
    /// <param name="canvas"></param>
    public void HideUI(Canvas canvas)
    {
        if (canvas != null)
        {
            canvas.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// canvas를 갖고 있는 게임 오브젝트가 활성화 되어있다면, 비활성화
    /// 비활성화 되어있다면 활성화
    /// </summary>
    /// <param name="canvas"></param>
    public void ToggleUI(Canvas canvas)
    {
        if (canvas != null)
        {
            canvas.gameObject.SetActive(!canvas.gameObject.activeSelf);
        }
    }

    /// <summary>
    /// RectTransform를 갖고 있는 게임 오브젝트가 활성화 되어있다면, 비활성화
    /// 비활성화 되어있다면 활성화
    /// </summary>  
    /// <param name="rectTransform"></param>
    public void ToggleUI(RectTransform rectTransform)
    {
        if (rectTransform != null)
        {
            rectTransform.gameObject.SetActive(!rectTransform.gameObject.activeSelf);
        }
    }
}
