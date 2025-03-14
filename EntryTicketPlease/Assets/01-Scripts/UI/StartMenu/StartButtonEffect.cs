using UnityEngine;
using UnityEngine.UI;

public class StartButtonEffect : MonoBehaviour
{
    public Button startButton;
    public float pulseSpeed = 1f;
    public float scaleAmount = 1.1f;
    private Vector3 originalScale;
    private Color originalColor;
    private Image buttonImage;

    void Start()
    {
        originalScale = startButton.transform.localScale;
        buttonImage = startButton.GetComponent<Image>();
        originalColor = buttonImage.color;
    }

    void Update()
    {
        float scaleFactor = 1 + Mathf.Sin(Time.time * pulseSpeed) * (scaleAmount - 1);
        startButton.transform.localScale = originalScale * scaleFactor;
        Color newColor = originalColor;
        newColor.a = 0.8f + Mathf.Sin(Time.time * pulseSpeed) * 0.2f;
        buttonImage.color = newColor;
    }
}
