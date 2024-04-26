using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;

    public bool AlwaysLookAtCamera { get; set; } = true;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        if (AlwaysLookAtCamera)
            transform.rotation = Quaternion.LookRotation(transform.position - GameManager.Instance.MainCamera.transform.position);
    }

    public void SetPercentage(float value)
    {
        LeanTween.value(slider.gameObject, slider.value, value, 0.1f).setOnUpdate((value) =>
        {
            slider.value = value;
        });
    }
}
