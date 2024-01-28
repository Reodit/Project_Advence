using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchCamera : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    public GameObject selectedObject;
    private Vector3 _offset;
    private float _initialDistance;
    private Vector3 _initialRotation;
    private Vector3 _lastMousePosition;
    private float _distanceToCamera;
    private EventSystem _eventSystem;
    static public TouchCamera Instance;
    
    private TouchCamera() { }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            HandleTouch();
        }
        
        else
        {
            HandleMouse();
        }
    }
    
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(_eventSystem)
        {
            position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
        };
        List<RaycastResult> results = new List<RaycastResult>();

        // 현재 활성화된 모든 GraphicRaycaster를 찾습니다.
        GraphicRaycaster[] raycasters = FindObjectsOfType<GraphicRaycaster>();

        foreach (GraphicRaycaster raycaster in raycasters)
        {
            raycaster.Raycast(eventDataCurrentPosition, results);
            if (results.Count > 0) return true;
        }

        return false;
    }
    

    void HandleTouch()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = _cam.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    
                }
                else if (IsPointerOverUIObject())
                {
                    
                }
                
                else
                {
                    
                }
            }
            
            else if (touch.phase == TouchPhase.Moved)
            {
                
            }
        }

        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                
            }
            
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                
            }
        }
    }
    void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
            }

            else if (IsPointerOverUIObject())
            {
            }
            
            else
            {
            }
        }
        
        else if (Input.GetMouseButton(0))
        {
            if (IsPointerOverUIObject())
            {
            }
            
            else if (selectedObject != null)
            {
            }
        }

        if (Input.GetMouseButton(1))
        {
        }
    }
}