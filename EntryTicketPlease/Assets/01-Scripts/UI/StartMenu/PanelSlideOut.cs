using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PanelSlideOut : MonoBehaviour
{
    [SerializeField] private RectTransform rectPanel;
    [SerializeField] private GameObject gameobjectPanel;
    [SerializeField] private float slideDuration = 0.5f;
    private Vector2 hiddenPosition;
    private Vector2 visiblePosition;

    void Start()
    {
        gameobjectPanel.SetActive(true);
        visiblePosition = rectPanel.anchoredPosition;
        hiddenPosition = new Vector2(rectPanel.anchoredPosition.x, Screen.height);
        rectPanel.gameObject.SetActive(true);
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            SlideOut();
        }
    }

    private void SlideOut()
    {
        rectPanel.DOAnchorPos(hiddenPosition, slideDuration).SetEase(Ease.InOutQuad);
    }
}