using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ColorPicker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Image colorWheel;
    public Texture2D colorWheelTexture;
    public GameObject targetObject;
    private bool isDragging = false;

    private void Start()
    {
        colorWheelTexture = colorWheel.sprite.texture;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        PickColor(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            PickColor(eventData);
        }
    }

    private void PickColor(PointerEventData eventData)
    {
        Vector2 localCursor;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(colorWheel.rectTransform, eventData.position, eventData.pressEventCamera, out localCursor))
        {
            float rectWidth = colorWheel.rectTransform.rect.width;
            float rectHeight = colorWheel.rectTransform.rect.height;
            float x = Mathf.Clamp(localCursor.x, -rectWidth * 0.5f, rectWidth * 0.5f) / rectWidth + 0.5f;
            float y = Mathf.Clamp(localCursor.y, -rectHeight * 0.5f, rectHeight * 0.5f) / rectHeight + 0.5f;

            Color color = colorWheelTexture.GetPixelBilinear(x, y);
            ChangeColor(color);
        }
    }

    private void ChangeColor(Color color)
    {
        targetObject = TouchCamera.Instance.selectedObject;
        
        if (targetObject != null)
        {
            ParticleSystem particle = targetObject.GetComponent<ParticleSystem>();
            if (particle != null)
            {
                ParticleSystem.MainModule mainModule = particle.main;
                mainModule.startColor = color;
            }
        }
    }
}
