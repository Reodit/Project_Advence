using UnityEngine;
using UnityEngine.UI;

public class ScaleObject : MonoBehaviour
{
    private GameObject _objectToScale;
    private Slider _scaleSlider;
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 2.0f;

    void Start()
    {
        _scaleSlider = GetComponent<Slider>();
        _scaleSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void OnSliderValueChanged(float value)
    {
        _objectToScale = TouchCamera.Instance.selectedObject;

        if (_objectToScale)
        {
            float scale = Mathf.Lerp(minScale, maxScale, value);
            _objectToScale.transform.localScale = new Vector3(scale, scale, scale);    
        }
    }
}